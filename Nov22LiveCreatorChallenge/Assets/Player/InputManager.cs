using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CS.CreatorChallenge.Nov22
{
    public class InputManager : MonoBehaviourPunCallbacks
    {

        #region Private Fields

        PlayerControls playerControls;
        PlayerManager playerManager;
        Vector2 movementInput;

        #endregion

        #region Public Variables

        public float verticalInput;
        public float horizontalInput;  
        public bool isFiring;

        #endregion


        #region MonoBehavior Callbacks

        public override void OnEnable()
        {
            if (playerControls == null)
            {
                playerControls = new PlayerControls();
                playerControls.Movement.Move.performed += i => movementInput = i.ReadValue<Vector2>();
                playerControls.Actions.Shoot.performed += i => isFiring = true;
            }
            playerControls.Enable();
        }

        public override void OnDisable()
        {
            playerControls.Disable();
        }

        private void Update()
        {
            //if (photonView.IsMine == false && PhotonView.IsConn)
            if (photonView.IsMine == false && PhotonNetwork.IsConnected == true) return;
            HandleMovementInput();
        }

        #endregion

        #region Private Methods

        private void HandleMovementInput()
        {
            verticalInput = movementInput.y;
            horizontalInput = movementInput.x;  
        }

        #endregion


    }
}
