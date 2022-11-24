using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CS.CreatorChallenge.Nov22
{
    public class PlayerManager : MonoBehaviourPunCallbacks, IPunObservable
    {
        #region Object References

        InputManager inputManager;
        Rigidbody playerRigidbody;

        public GameObject laserBolt;

        #endregion

        #region Public Fields

        [Header("Player Attributes")]
        public float health;
        public float movementSpeed;

        [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
        public static GameObject LocalPlayerInstance;

        #endregion

        #region Private Fields

        bool isFiring;

        #endregion

        #region MonoBehavior Callbacks

        private void Awake()
        {
            inputManager = GetComponent<InputManager>();
            playerRigidbody = GetComponent<Rigidbody>();

            if (photonView.IsMine)
            {
                LocalPlayerInstance = this.gameObject;
            }
            DontDestroyOnLoad(this.gameObject);
        }

        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (photonView.IsMine == false && PhotonNetwork.IsConnected == true) return;

            if (photonView.IsMine)
            {
                if (health <= 0f)
                {
                    GameManager.Instance.LeaveRoom();
                }
            }

            isFiring = inputManager.isFiring;
            if (isFiring)
            {
                Shoot();
            }
        }

        private void FixedUpdate()
        {
            if (photonView.IsMine == false && PhotonNetwork.IsConnected == true) return;
            
            Movement();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!photonView.IsMine)
            {
                return;
            }

            //check if other is an asteroid

            health -= 1f;
        }

        #endregion

        #region MonoBehaviourPun Callbacks

        #endregion

        #region Public Methods

        public void Shoot()
        {
            Debug.Log("Player " + PhotonNetwork.NickName + " is shooting.");
            isFiring = false;
            inputManager.isFiring = isFiring;
        }

        #endregion

        #region Private Methods

        private void Movement()
        {
            Vector3 movementVelocity = new Vector3(transform.forward.x, 0f, transform.forward.z) * inputManager.verticalInput;
            movementVelocity += transform.right * inputManager.horizontalInput;
            movementVelocity.Normalize();
            movementVelocity.y = 0f;
            movementVelocity *= movementSpeed;

            playerRigidbody.velocity = movementVelocity;
        }



        #endregion

        #region IPunObservable implementation

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(isFiring);
                stream.SendNext(health);
            }
            else
            {
                this.isFiring = (bool)stream.ReceiveNext();
                this.health = (float)stream.ReceiveNext();  
            }
        }

        #endregion

    }

}
