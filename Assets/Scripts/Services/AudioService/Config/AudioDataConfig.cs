using Urd.Audio;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioDataConfig", menuName = "Urd/Services/Audio Data Config", order = 1)]
public class AudioDataConfig : ScriptableObject
{
    [field: SerializeReference, SubclassSelector]
    public IAudioData AudioData { get; private set; }
}
