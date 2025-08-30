using System.Collections.Generic;
using System.Linq;
using MyBox;
using RovioTest.Config;

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

namespace RovioTest
{
    public class GamePlayConfig : ScriptableObject
    {
        [field: Header("Default Player Data")]
        [field: SerializeField]
        public CharacterConfig DefaultCharacterConfig { get; private set; }
        
        [field: Header("Game Data")]
        [field: SerializeField, ReadOnly, Tooltip("AutoFilled with All Court Configs")]
        public List<CourtConfig> Courts { get; private set; }
        [field: SerializeField, ReadOnly, Tooltip("AutoFilled with All Character Configs")]
        public List<CharacterConfig> Characters { get; private set; }
        
#if UNITY_EDITOR
        private void OnValidate()
        {
            UpdateCourts();
            UpdateCharacters();
        }

        private void UpdateCharacters()
        {
            string path = AssetDatabase.GetAssetPath(this);
            string parentFolder = path[..path.IndexOfItem('/')];

            const string filter = " t:CharacterConfig";
            string[] searchInFolders = { parentFolder };
            string[] guids = AssetDatabase.FindAssets(filter, searchInFolders);

            Characters = guids
                .Select(AssetDatabase.GUIDToAssetPath)
                .OrderBy(assetPath => assetPath)
                .Select(AssetDatabase.LoadAssetAtPath<Object>)
                .OfType<CharacterConfig>()
                .ToList();
        }

        private void UpdateCourts()
        {
            string path = AssetDatabase.GetAssetPath(this);
            string parentFolder = path[..path.IndexOfItem('/')];

            const string filter = " t:CourtConfig";
            string[] searchInFolders = { parentFolder };
            string[] guids = AssetDatabase.FindAssets(filter, searchInFolders);

            Courts = guids
                .Select(AssetDatabase.GUIDToAssetPath)
                .OrderBy(assetPath => assetPath)
                .Select(AssetDatabase.LoadAssetAtPath<Object>)
                .OfType<CourtConfig>()
                .ToList();
        }
#endif
    }
}