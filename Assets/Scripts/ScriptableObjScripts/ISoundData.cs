using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RML
{
    public interface ISoundData
    {
        AudioClip GetClip(SoundType soundType);    
    }

}
