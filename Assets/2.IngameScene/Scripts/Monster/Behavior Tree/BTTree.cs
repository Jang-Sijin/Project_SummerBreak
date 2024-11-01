using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public abstract class BTTree : MonoBehaviour
    {
        private Node _root = null;

        protected void Start()
        {
            _root = SetUpTree();
        }

        protected void Update()
        {
            if (_root != null)
            {
                _root.Evaluate();
            }
        }

        protected abstract Node SetUpTree();
    }
}
