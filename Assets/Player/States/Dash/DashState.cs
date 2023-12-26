using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerStateMachine {
    [CreateAssetMenu(fileName = "New DashState", menuName = "States/Player/DashState")]
    public class DashState : PlayerState<Player>
    {
        public float DashForce;
        public Rigidbody2D rb;
        private float curTime = 0;
        public float totalDashTime = 0.5f;
        private Vector3 ForceVector;
        public float dashStopPower;
        public float jumpOutDashStopPower = 2f;
        public PlayerState<Player> IdleState;
        public PlayerState<Player> JumpOutDashState;
        private PlayerInputs movement;
        public TrailRenderer trail;
        public Animator animator;
        public float TimeBeforeDashTurnsOff = 0.1f;
        public string TagName;
        public string PlayerTagName;
        private bool Dashing = false;

        public ParticleSystem ps; 

        public override void Init(Player parent) {
            base.Init(parent);


            movement = parent.GetComponent<PlayerStateRunner>().Inputs;

            curTime = 0f;

            rb = parent.GetComponent<Rigidbody2D>();
            rb.velocity = Vector3.zero;
            rb.gravityScale = 0f;
            animator = parent.GetComponent<PlayerStateRunner>().animator;

            trail = player.DashTrail;
            trail.emitting = true;

            ps = player.DashParticleFX;

            parent.gameObject.tag = TagName;

            player.DashSoundFx.Play();

            Dashing = false;

            Dash();

        }
        public override void Inputs() {
            
        }
        public override void ConstantUpdate() {
            curTime+=Time.deltaTime;
            if (curTime > totalDashTime) {
                rb.velocity = rb.velocity/dashStopPower;
                Debug.Log("NOW");
                runner.GetComponent<PlayerStateRunner>().SetState(IdleState);
            }
        }
        public override void PhysicsUpdate() {
            // Debug.Log(ForceVector);
            if (Dashing) {
                rb.velocity = ForceVector*DashForce;
            }
        }
        public override void ChangeState() {
            if (player.TriggerCollisions.ContainsKey("Enemy")) {
                rb.velocity = rb.velocity/(jumpOutDashStopPower);
                player.canDash = true;
                runner.GetComponent<PlayerStateRunner>().SetState(JumpOutDashState);
            }
        }
        public override void Exit() { 
            trail.emitting = false;

            player.TurnDashOffCall(TimeBeforeDashTurnsOff,PlayerTagName);
            
            
        }
        void Dash() {
            Vector3 CursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            ForceVector = new Vector3(CursorPos.x,CursorPos.y,runner.transform.position.z)-runner.transform.position;

            ForceVector = ForceVector.normalized;

            ps.Play();
            ps.transform.right = Quaternion.Euler(0, 0, -90) * ForceVector;
            Dashing = true;
            // rb.AddForce(ForceVector*DashForce,ForceMode2D.Impulse);
        }
    }
}