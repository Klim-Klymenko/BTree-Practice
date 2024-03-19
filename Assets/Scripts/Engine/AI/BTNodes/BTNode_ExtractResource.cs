using System;
using AIModule;
using Atomic.Elements;
using Atomic.Extensions;
using Atomic.Objects;
using UnityEngine;

namespace Game.Engine
{
    [Serializable]
    public sealed class BTNode_ExtractResource : BTNode
    {
        public override string Name => "Extract Resource";

        [SerializeField, BlackboardKey]
        private ushort character;
        
        [SerializeField, BlackboardKey]
        private ushort resource;

        [SerializeField, BlackboardKey]
        private ushort minDistance;

        protected override BTState OnUpdate(IBlackboard blackboard, float deltaTime)
        {
            if (!blackboard.TryGetObject(character, out IAtomicObject characterObject))
                return BTState.FAILURE;
            
            if (!blackboard.TryGetObject(resource, out IAtomicObject resourceObject))
                return BTState.FAILURE;

            if (!blackboard.TryGetFloat(minDistance, out float minDistanceValue))
                return BTState.FAILURE;
            
            Transform characterTransform = characterObject.Get<Transform>(ObjectAPI.Transform);
            Transform resourceTransform = resourceObject.Get<Transform>(ObjectAPI.Transform);
            
            Vector3 characterPosition = characterTransform.position;
            Vector3 resourcePosition = resourceTransform.position;
            
            Vector3 directionVector = resourcePosition - characterPosition;
            
            if (directionVector.sqrMagnitude > minDistanceValue * minDistanceValue)
                return BTState.FAILURE;
         
            ResourceStorage characterResourceStorage = characterObject.Get<ResourceStorage>(ObjectAPI.ResourceStorage);
            ResourceStorage treeResourceStorage = resourceObject.Get<ResourceStorage>(ObjectAPI.ResourceStorage);
            
            if (treeResourceStorage.IsEmpty())
                return BTState.SUCCESS;
            
            if (characterResourceStorage.IsFull())
                return BTState.SUCCESS;
            
            IAtomicAction gatherRequest = characterObject.GetAction(ObjectAPI.GatherRequest);
            
            gatherRequest.Invoke();
            
            return BTState.RUNNING;
        }
    }
}