using System;
using System.Collections.Generic;
using RovioTest.AI;
using UnityEngine;

namespace RovioTest.Config
{
    public class EnemyModuleConfig : ScriptableObject
    {
        [field: SerializeField]
        public List<EnemyMovementBehaviorByTypes> MovementsBehaviors { get; private set; }
        [field: SerializeField]
        public List<EnemyHitterBehaviorByTypes> HitterBehaviors { get; private set; }
        
    }

    [Serializable]
    public class EnemyMovementBehaviorByTypes
    {
        [field: SerializeField]
        public EnemyMovementTypes MovementType { get; private set; }
        [field: SerializeReference, SubclassSelector]
        public ICharacterMovementBehavior MovementBehavior { get; private set; }
    }
    
    [Serializable]
    public class EnemyHitterBehaviorByTypes
    {
        [field: SerializeField]
        public EnemyHitterTypes HitterType { get; private set; }
        [field: SerializeReference, SubclassSelector]
        public ICharacterHitterBehavior HitterBehavior { get; private set; }
    }
}