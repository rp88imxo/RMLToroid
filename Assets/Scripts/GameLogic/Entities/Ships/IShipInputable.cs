using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RML
{
    public interface IShipInputable
    {
        bool isPressedShootButton();
        float GetHorizontalInput();
        float GetVerticalInput();
        void SetControllPossibility(bool controlState); // Включение, выключение возможности управлять кораблем
        bool isPressedHyperJumpButton();
        bool CanControl();
    }
}
