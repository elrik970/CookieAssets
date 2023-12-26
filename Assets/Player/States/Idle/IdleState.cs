using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerStateMachine {
    [CreateAssetMenu(fileName = "New WalkState", menuName = "States/Player/IdleState")]
    public class IdleState : PlayerState<Player>
    {
        public float acceleration;
        public Rigidbody2D rb;
        public float Gravity;
        public PlayerState<Player> WalkStateRight;
        public PlayerState<Player> WalkStateLeft;
        public PlayerState<Player> SlideStateLeft;
        public PlayerState<Player> SlideStateRight;
        public PlayerState<Player> JumpState;
        public PlayerState<Player> FallingState;
        public PlayerState<Player> DashState;
        public float dashStopPower = 1.5f;
        public PlayerState<Player> JumpOutDashState;

        private PlayerInputs movement;
        public string leftWalkButton;
        public string rightWalkButton;
        public Animator animator;

        public override void Init(Player parent) {
            base.Init(parent);


            movement = parent.GetComponent<PlayerStateRunner>().Inputs;
            movement.Normal.JumpPress.performed += OnJump;
            movement.Normal.Dash.performed += OnDash;

            rb = parent.GetComponent<Rigidbody2D>();
            rb.gravityScale = Gravity;

            animator = parent.GetComponent<PlayerStateRunner>().animator;

            animator.Play("Idle");
        }
        public override void Inputs() {
            
        }
        public override void ConstantUpdate() {
            if (player.OnGround) {
                player.canDash = true;
            }
        }
        public override void PhysicsUpdate() {
            float SpeedDif = -rb.velocity.x;
            float movement = SpeedDif * acceleration;
            rb.AddForce(movement*Vector2.right,ForceMode2D.Impulse);
        }
        public override void ChangeState() {
            if (Input.GetKey(KeyCode.D)&&Input.GetKey(KeyCode.LeftShift)) {
                runner.GetComponent<PlayerStateRunner>().SetState(SlideStateRight);
            }
            else if (Input.GetKey(KeyCode.D)) {
                runner.GetComponent<PlayerStateRunner>().SetState(WalkStateRight);
                
            }
            
            if (Input.GetKey(KeyCode.A)&&Input.GetKey(KeyCode.LeftShift)) {
                runner.GetComponent<PlayerStateRunner>().SetState(SlideStateLeft);
            }
            else if (Input.GetKey(KeyCode.A)) {
                runner.GetComponent<PlayerStateRunner>().SetState(WalkStateLeft);
            }
            if (!player.OnGround) {
                runner.GetComponent<PlayerStateRunner>().SetState(FallingState);
            }

            if (player.TriggerCollisions.ContainsKey("Enemy")&&player.gameObject.tag == "PlayerDashing") {
                runner.GetComponent<PlayerStateRunner>().SetState(JumpOutDashState);
            }
        }
        public override void Exit() { 
            movement.Normal.JumpPress.performed -= OnJump;
            movement.Normal.Dash.performed -= OnDash;

        }
        void OnJump(InputAction.CallbackContext context) {
            if (player.OnGround) {
                runner.GetComponent<PlayerStateRunner>().SetState(JumpState);
            }
        }
        void OnDash(InputAction.CallbackContext context) {
            if (player.canDash) {
                runner.GetComponent<PlayerStateRunner>().SetState(DashState);
                player.canDash = false;
            }
        }
    }
}