using System;
using AIModule;
using UnityEngine;

namespace Game.Engine
{
    [Serializable]
    public sealed class BTNode_SwitchGameObject : BTNode
    {
        [SerializeField] 
        private GameObject _gameObject;
        
        [SerializeField]
        private bool _active;
        
        protected override BTState OnUpdate(IBlackboard blackboard, float deltaTime)
        {
            _gameObject.SetActive(_active);
            
            return BTState.SUCCESS;
        }
    }
}