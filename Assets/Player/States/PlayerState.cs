using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerStateMachine {
    public abstract class PlayerState<T> : ScriptableObject where T : MonoBehaviour
    {
        public T runner; 
        public Player player;
        public virtual void Init(T parent) {
            runner = parent;
            player = parent.GetComponent<Player>();
        }
        public abstract void Inputs();
        public abstract void ConstantUpdate();
        public abstract void PhysicsUpdate();
        public abstract void ChangeState();
        public abstract void Exit();
    }
}