using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerStateMachine {
    [CreateAssetMenu(fileName = "New WalkStateRight", menuName = "States/Player/RightWalkState")]
    public class WalkStateRight : PlayerState<Player>
    {
        public float maxSpeed;
        public float acceleration;
        public float PressOtherDirAccel;
        public Rigidbody2D rb;
        private Animator animator;
        public PlayerState<Player> SlideStateRight;
        public PlayerState<Player> IdleState;
        public PlayerState<Player> FallingState;
        public PlayerState<Player> JumpState;
        public PlayerState<Player> DashState;

        public float dashStopPower = 1.5f;
        public PlayerState<Player> JumpOutDashState;

        private PlayerInputs movement;

        public override void Init(Player parent) {
            base.Init(parent);


            movement = parent.GetComponent<PlayerStateRunner>().Inputs;
            movement.Normal.RunRightRelease.performed += OnWalkRelease;
            movement.Normal.JumpPress.performed += OnJump;
            movement.Normal.Dash.performed += OnDash;

            rb = parent.GetComponent<Rigidbody2D>();
            animator = parent.GetComponent<PlayerStateRunner>().animator;
            animator.transform.localScale = new Vector3(1f,1f,1f);

            animator.Play("Running");

            player.WalkParticleFX.Play();
            ParticleSystem.EmissionModule em = player.WalkParticleFX.emission;
            em.enabled = true;

            
        }
        public override void Inputs() {
            
        }
        public override void ConstantUpdate() {
            animator.Play("Running");
            if (player.OnGround) {
                player.canDash = true;
            }
        }
        public override void PhysicsUpdate() {
            
            // if (rb.velocity.x < maxSpeed) {
                float SpeedDif = maxSpeed - rb.velocity.x;
                float movement = SpeedDif * acceleration;
                rb.AddForce(movement*Vector2.right,ForceMode2D.Impulse);
            //}
        }
        public override void ChangeState() {
            // if (rb.velocity.y < 0) {
            if (!player.OnGround) {
                runner.GetComponent<PlayerStateRunner>().SetState(FallingState);
            }
            // }

            if (Input.GetKey(KeyCode.D)&&Input.GetKey(KeyCode.LeftShift)) {
                runner.GetComponent<PlayerStateRunner>().SetState(SlideStateRight);
            }

            if (player.TriggerCollisions.ContainsKey("Enemy")&&player.gameObject.tag == "PlayerDashing") {
                runner.GetComponent<PlayerStateRunner>().SetState(JumpOutDashState);
                
            }
        }
        public override void Exit() { 
            movement.Normal.RunRightRelease.performed -= OnWalkRelease;
            movement.Normal.JumpPress.performed -= OnJump;
            movement.Normal.Dash.performed -= OnDash;

            ParticleSystem.EmissionModule em = player.WalkParticleFX.emission;
            em.enabled = false;
        }

        void OnWalkRelease(InputAction.CallbackContext context) {
            runner.GetComponent<PlayerStateRunner>().SetState(IdleState);
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