using System;
using AIModule;
using Atomic.Objects;
using UnityEngine;

namespace Game.Engine
{
    [Serializable]
    public sealed class BTNode_UnloadResources : BTNode
    {
        public override string Name => "Unload Resources";

        [SerializeField, BlackboardKey]
        private ushort character;

        [SerializeField, BlackboardKey]
        private ushort targetStorage;
        
        protected override BTState OnUpdate(IBlackboard blackboard, float deltaTime)
        {
            if (!blackboard.TryGetObject(targetStorage, out IAtomicObject targetStorageObject))
                return BTState.FAILURE;
            
            if (!blackboard.TryGetObject(character, out IAtomicObject characterObject))
                return BTState.FAILURE;
            
            ResourceStorage characterResourceStorage = characterObject.Get<ResourceStorage>(ObjectAPI.ResourceStorage);
            ResourceStorage targetResourceStorage = targetStorageObject.Get<ResourceStorage>(ObjectAPI.ResourceStorage);

            if (characterResourceStorage.IsEmpty())
                return BTState.FAILURE;

            if (targetResourceStorage.IsFull())
                return BTState.FAILURE;

            int resourcesToPut = characterResourceStorage.ExtractAllResources();
            targetResourceStorage.PutResources(resourcesToPut);
            
            return BTState.SUCCESS;
        }
    }
}