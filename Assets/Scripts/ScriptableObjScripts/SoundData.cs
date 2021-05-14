using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RML
{
    [System.Serializable]
    public struct SoundEntity
    {
        public SoundType soundType;
        public AudioClip clip;
    }

    [CreateAssetMenu(menuName = "RML/GameData/Sound Data")]
    public class SoundData : ScriptableObject, ISoundData
    {
        [SerializeField]
        private List<SoundEntity> soundEntities;


        public AudioClip GetClip(SoundType soundType)
        {
            return soundEntities.Find(x => x.soundType == soundType).clip;
        }
    }

}
