using System.Collections.Generic;
using System.Linq;
using MyBox;

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using Object = UnityEngine.Object;

namespace RovioTest.Config
{
    public class GamePlayConfig : ScriptableObject
    {
        [field: Header("Default Player Data")]
        [field: SerializeField]
        public CharacterConfig DefaultCharacterConfig { get; private set; }
        
        [field: Header("Snimations & Effects")]
        [field: SerializeField]
        public float InitialAnimationDuration { get; private set; }
        [field: SerializeField]
        public float WaitTimeAfterEnemyServe { get; private set; }
        [field: SerializeField]
        public float WaitTimeAfterSmash { get; private set; }
        
        [field: Header("Game Data")]
        [field: SerializeField, ReadOnly, Tooltip("AutoFilled with All Court Configs")]
        public List<CourtConfig> Courts { get; private set; }
        
        [field: SerializeField, ReadOnly, Tooltip("AutoFilled with All Character Configs")]
        public List<CharacterConfig> Characters { get; private set; }
        
        [field: SerializeField, ReadOnly, Tooltip("AutoFilled with All Balls Configs")]
        public List<BallConfig> Balls { get; private set; }
        
        
        
#if UNITY_EDITOR
        private void OnValidate()
        {
            UpdateCourts();
            UpdateCharacters();
            UpdateBalls();
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
                .Where(c => c.IsSelectableToEnemy)
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
        
        private void UpdateBalls()
        {
            string path = AssetDatabase.GetAssetPath(this);
            string parentFolder = path[..path.IndexOfItem('/')];

            const string filter = " t:BallConfig";
            string[] searchInFolders = { parentFolder };
            string[] guids = AssetDatabase.FindAssets(filter, searchInFolders);

            Balls = guids
                .Select(AssetDatabase.GUIDToAssetPath)
                .OrderBy(assetPath => assetPath)
                .Select(AssetDatabase.LoadAssetAtPath<Object>)
                .OfType<BallConfig>()
                .ToList();
        }
#endif
    }
}