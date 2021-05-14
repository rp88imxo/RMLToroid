using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RML
{
    public interface IGameStateHelper
    {
        int GetCurrentScore();
        int GetAvailableLives();
    }

}
