using System;
using AIModule;
using Atomic.Elements;
using Atomic.Extensions;
using Atomic.Objects;
using UnityEngine;

namespace Game.Engine
{
    [Serializable]
    public sealed class BTNode_MoveToTarget : BTNode
    {
        public override string Name => "Move To Target";

        [SerializeField, BlackboardKey]
        private ushort character;

        [SerializeField, BlackboardKey]
        private ushort target;

        [SerializeField, BlackboardKey]
        private ushort stoppingDistance;

        protected override BTState OnUpdate(IBlackboard blackboard, float deltaTime)
        {
            if (!blackboard.TryGetObject(character, out IAtomicObject characterObject))
                return BTState.FAILURE;

            if (!blackboard.TryGetObject(target, out IAtomicObject targetObject))
                return BTState.FAILURE;
            
            if (!blackboard.TryGetFloat(stoppingDistance, out float stoppingDistanceValue))
                return BTState.FAILURE;
            
            Transform characterTransform = characterObject.Get<Transform>(ObjectAPI.Transform);
            Transform targetTransform = targetObject.Get<Transform>(ObjectAPI.Transform);
            
            Vector3 characterPosition = characterTransform.position;
            Vector3 targetPosition = targetTransform.position;
            
            Vector3 directionVector = targetPosition - characterPosition;
            
            if (directionVector.sqrMagnitude <= stoppingDistanceValue * stoppingDistanceValue)
                return BTState.SUCCESS;

            IAtomicAction<Vector3> moveStepRequest = characterObject.GetAction<Vector3>(ObjectAPI.MoveStepRequest);
            
            Vector3 direction = directionVector.normalized;
            moveStepRequest.Invoke(direction);
            
            return BTState.RUNNING;
        }
    }
}