using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerStateMachine {
    [CreateAssetMenu(fileName = "New JumpState", menuName = "States/Player/JumpState")]
    public class JumpState : PlayerState<Player>
    {
        public float maxSpeed;
        public float acceleration;
        public float deAccel = 0f;
        public float JumpForce;
        public float gravity;
        public Rigidbody2D rb;
        public float RaycastSize;
        private Animator animator;
        public PlayerState<Player> FallingState;
        public PlayerState<Player> IdleState;
        public PlayerState<Player> DashState;
        public float CloseToGround = 0.1f;
        private PlayerInputs movement;
        private bool leftGround = false;
        private RaycastHit2D GroundHit;
        public float VariableJumpHeightMult = 1;
        private bool releaseJump;
        public LayerMask GroundLayerMask;

        public override void Init(Player parent) {
            base.Init(parent);


            movement = parent.GetComponent<PlayerStateRunner>().Inputs;
            movement.Normal.JumpRelease.performed += OnJumpRelease;
            movement.Normal.Dash.performed += OnDash;

            rb = parent.GetComponent<Rigidbody2D>();
            animator = parent.GetComponent<PlayerStateRunner>().animator;
            rb.gravityScale = gravity;
            rb.velocity = new Vector2(rb.velocity.x,0f);
            rb.AddForce(Vector3.up*JumpForce,ForceMode2D.Impulse);
            releaseJump = true;

            animator.Play("Jump");
        }
        public override void Inputs() {
            bool LeftPress = Input.GetKey(KeyCode.A);
            bool RightPress = Input.GetKey(KeyCode.D);
            if (LeftPress&&rb.velocity.x > -maxSpeed) {
                animator.transform.localScale = new Vector3(-1f,1f,1f);
                float SpeedDif = -maxSpeed - rb.velocity.x;
                float movement = SpeedDif * acceleration;
                rb.AddForce(movement*Vector2.right,ForceMode2D.Impulse);
            }
            if (RightPress&&rb.velocity.x < maxSpeed) {
                animator.transform.localScale = new Vector3(1f,1f,1f);
                float SpeedDif = maxSpeed - rb.velocity.x;
                float movement = SpeedDif * acceleration;
                rb.AddForce(movement*Vector2.right,ForceMode2D.Impulse);
            }
            else if (LeftPress == false&&RightPress == false) {
                float SpeedDif = -rb.velocity.x;
                float movement = SpeedDif * deAccel;
                rb.AddForce(movement*Vector2.right,ForceMode2D.Impulse);
            }
        }
        public override void ConstantUpdate() {

            if (!player.OnGround) {
                leftGround = true;
            }

            if (rb.velocity.y < 0) {
                runner.GetComponent<PlayerStateRunner>().SetState(FallingState);
            }

        }
        public override void PhysicsUpdate() {
            
        }
        public override void ChangeState() {
            
            if (player.OnGround&&leftGround) {
                runner.GetComponent<PlayerStateRunner>().SetState(IdleState);
            }
        }
        public override void Exit() { 
            leftGround = false;
            movement.Normal.JumpRelease.performed -= OnJumpRelease;
            movement.Normal.Dash.performed -= OnDash;
        }
        void OnJumpRelease(InputAction.CallbackContext context) {
            if (releaseJump) {
                rb.AddForce((rb.velocity.y*VariableJumpHeightMult)*Vector2.down,ForceMode2D.Impulse);
                releaseJump = false;
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