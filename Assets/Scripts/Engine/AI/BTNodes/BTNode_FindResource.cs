using System;
using AIModule;
using Atomic.Objects;
using UnityEngine;

namespace Game.Engine
{
    [Serializable]
    public sealed class BTNode_FindResource : BTNode
    {
        public override string Name => "Find Resource";

        [SerializeField, BlackboardKey]
        private ushort character;

        [SerializeField, BlackboardKey]
        private ushort resourceService;

        [SerializeField, BlackboardKey]
        private ushort targetResource;

        protected override BTState OnUpdate(IBlackboard blackboard, float deltaTime)
        {
            if (!blackboard.TryGetObject(this.character, out IAtomicObject character))
                return BTState.FAILURE;
            
            if (!blackboard.TryGetObject(resourceService, out ResourceService resourceServiceObject))
                return BTState.FAILURE;
            
            Transform characterTransform = character.Get<Transform>(ObjectAPI.Transform);

            if (!resourceServiceObject.FindClosestResource(characterTransform.position, out IAtomicObject resource))
                return BTState.FAILURE;
            
            blackboard.SetObject(targetResource, resource);

            return BTState.SUCCESS;
        }
    }
}