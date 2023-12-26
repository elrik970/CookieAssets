using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerStateMachine {
    [CreateAssetMenu(fileName = "New FallingState", menuName = "States/Player/FallingState")]
    public class FallingState : PlayerState<Player>
    {
        public float maxSpeed;
        private float time = 0f;
        public float acceleration;
        public float deAccel = 0.1f;
        public float gravity;
        public Rigidbody2D rb;
        private Animator animator;
        public float dashStopPower = 1.5f;
        public PlayerState<Player> JumpOutDashState;
        public float JumpOutDashPower = 2f;
        public float ApexGravity = 0.5f;
        public float ApexTime = 0.2f;
        public PlayerState<Player> IdleState;
        public PlayerState<Player> DashState;
        public PlayerState<Player> JumpState;
        private PlayerInputs movement;
        public float coyoteTime = 0.1f;
        

        public override void Init(Player parent) {
            base.Init(parent);


            movement = parent.GetComponent<PlayerStateRunner>().Inputs;
            movement.Normal.Dash.performed += OnDash;
            movement.Normal.JumpPress.performed += OnJump;

            rb = parent.GetComponent<Rigidbody2D>();
            animator = parent.GetComponent<PlayerStateRunner>().animator;
            rb.gravityScale = ApexGravity;
            time = 0f;
            
            animator.Play("FallingAnim");

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
            time += Time.deltaTime;
            if (time > ApexTime) {
                rb.gravityScale = gravity;
            }
        }
        public override void PhysicsUpdate() {

        }
        public override void ChangeState() {
            
            if (runner.OnGround) {
                runner.GetComponent<PlayerStateRunner>().SetState(IdleState);
            }

            if (player.TriggerCollisions.ContainsKey("Enemy")&&player.gameObject.tag == "PlayerDashing") {
                player.canDash = true;
                rb.velocity = new Vector2(rb.velocity.x,0f);
                runner.GetComponent<PlayerStateRunner>().SetState(JumpOutDashState);
            }
        }
        public override void Exit() { 
            movement.Normal.Dash.performed -= OnDash;
            movement.Normal.JumpPress.performed -= OnJump;
        }
        void OnDash(InputAction.CallbackContext context) {
            if (player.canDash) {
                runner.GetComponent<PlayerStateRunner>().SetState(DashState);
                player.canDash = false;
            }
        }

        void OnJump(InputAction.CallbackContext context) {
            if (player.TimeSinceOnGround < coyoteTime) {
                runner.GetComponent<PlayerStateRunner>().SetState(JumpState);
            }
        }

        
    }
}