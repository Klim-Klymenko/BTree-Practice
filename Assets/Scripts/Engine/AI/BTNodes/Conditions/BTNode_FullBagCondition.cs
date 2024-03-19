using System;
using AIModule;
using Atomic.Objects;
using UnityEngine;

namespace Game.Engine
{
    [Serializable]
    public sealed class BTNode_FullBagCondition : BTNode
    {
        [SerializeField, BlackboardKey]
        private ushort _entity;
        
        protected override BTState OnUpdate(IBlackboard blackboard, float deltaTime)
        {
            if (!blackboard.TryGetObject(_entity, out IAtomicObject character))
                return BTState.FAILURE;
            
            ResourceStorage resourceStorage = character.Get<ResourceStorage>(ObjectAPI.ResourceStorage);

            if (resourceStorage == null)
                return BTState.FAILURE;

            if (resourceStorage.IsNotFull())
                return BTState.FAILURE;
            
            return BTState.SUCCESS;
        }
    }
}