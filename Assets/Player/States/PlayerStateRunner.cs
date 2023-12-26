using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace PlayerStateMachine {
    // public class PlayerStateRunner<T> : MonoBehaviour where T : MonoBehaviour
    public class PlayerStateRunner : MonoBehaviour 
    {
        [SerializeField] PlayerState<Player>[] States;
        public PlayerState<Player> curState;
        public PlayerInputs Inputs;
        public Animator animator;

        void Awake() {
            Inputs = new PlayerInputs();
            
            
        }
        void OnEnable() {
            Inputs.Enable();
        }

        void OnDisable() {
            if (Inputs != null) Inputs.Disable();
        }
        void Start()
        {
            curState.Init(GetComponent<Player>());
        }

        // Update is called once per frame
        void Update()
        {
            // Debug.Log(curState);
            if (curState != null) {
                curState.ConstantUpdate();
                curState.ChangeState();
                curState.Inputs();
            }
        }
        void FixedUpdate() {
            if (curState != null) {
                curState.PhysicsUpdate();
            }
        }
        public void SetState(PlayerState<Player> changeStateTo) {
            // Debug.Log("ChangedState");
            if (curState != null) {
                curState.Exit();
            }
            curState = changeStateTo;
            curState.Init(GetComponent<Player>());
        }
    }
}