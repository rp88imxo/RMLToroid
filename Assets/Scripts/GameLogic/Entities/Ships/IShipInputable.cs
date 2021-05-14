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
        void SetControllPossibility(bool controlState); // ���������, ���������� ����������� ��������� ��������
        bool isPressedHyperJumpButton();
        bool CanControl();
    }
}
