using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RML
{
    public class ShipInput : IShipInputable
    {
        private bool isAbleToControl;
        private const string horizontalDefaultAxis = "Horizontal";
        private const string verticalDefaultAxis = "Vertical";

        public ShipInput()
        {
            isAbleToControl = true;
        }

        #region INTERFACE_CONTRACTS
        public float GetVerticalInput()
        {
            return Input.GetAxis(verticalDefaultAxis);
        }

        public float GetHorizontalInput()
        {
            return Input.GetAxis(horizontalDefaultAxis);
        }

        public bool isPressedShootButton()
        {
          return Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0);
        }

        public void SetControllPossibility(bool controlState)
        {
            isAbleToControl = controlState;
        }

        public bool isPressedHyperJumpButton()
        {
            return Input.GetKeyDown(KeyCode.V);
        }

        public bool CanControl() => isAbleToControl;

        #endregion

    }

}
