using System.Collections.Generic;
using UnityEngine;
using Urd.Audio;

namespace Urd.Services
{
    [CreateAssetMenu(fileName = "AudioClipConfig", menuName = "Urd/Services/Audio/Audio Clip Config", order = 1)]

    public class AudioClipConfig : ScriptableObject
    {
        [field: SerializeReference, SubclassSelector]

        public IAudioData Audio { get; private set; }
    }
}