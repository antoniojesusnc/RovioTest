using System.Collections.Generic;
using System.Linq;
using MyBox;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Audio;
using Urd.Audio;

namespace Urd.Services
{
    [CreateAssetMenu(fileName = "AudioConfig", menuName = "Urd/Services/Audio Config", order = 1)]
    public class AudioConfig : ScriptableObject
    {
        [field: SerializeField] public float TimeToPlayAgain { get; private set; } = 0.05f;
        [field: SerializeField] public List<AudioMixerData> Mixers { get; private set; } = new List<AudioMixerData>();

        [field: SerializeField, ReadOnly]
        public List<AudioClipConfig> Audios { get; private set; } = new List<AudioClipConfig>();

        public AudioMixerGroup GetMixer(AudioMixerType mixerType)
        {
            return Mixers?.Find(mixer => mixer.MixerType == mixerType)?.Mixer;
        }

        public bool TryGetAudioData(AudioModel audioModel, out IAudioData audioData)
        {
            audioData = Audios.Find(audioDataConfig => audioDataConfig.Audio.Type.Equals(audioModel.AudioType))
                ?.Audio;
            return audioData != null;
        }


#if UNITY_EDITOR
        private void OnValidate()
        {
            UpdateAudios();
        }

        private void UpdateAudios()
        {
            string path = AssetDatabase.GetAssetPath(this);
            string parentFolder = path[..path.IndexOfItem('/')];

            const string filter = " t:AudioClipConfig";
            string[] searchInFolders = { parentFolder };
            string[] guids = AssetDatabase.FindAssets(filter, searchInFolders);

            Audios = guids
                .Select(AssetDatabase.GUIDToAssetPath)
                .OrderBy(assetPath => assetPath)
                .Select(AssetDatabase.LoadAssetAtPath<Object>)
                .OfType<AudioClipConfig>()
                .ToList();
        }
#endif
    }
}