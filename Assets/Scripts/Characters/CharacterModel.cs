using RovioTest.Config;
using UnityEngine;

public class CharacterModel : MonoBehaviour
{
    private CharacterConfig _config;
    
    [field: SerializeField]
    public float CurrentHp { get; private set; }

    [field: SerializeField] public float MaxHP => _config?.Hp ?? 0;
    
    public void SetConfig(CharacterConfig config)
    {
        _config = config;
    }
}
