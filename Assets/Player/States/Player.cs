using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    public float RaycastSize = 0.2f;
    private RaycastHit2D GroundHit;
    public LayerMask GroundLayerMask;
    public float CloseToGround = 0.4f;
    public bool OnGround = false;
    public bool canDash = false;

    public Dictionary<string, GameObject> TriggerCollisions = new Dictionary<string,GameObject>(); 

    public TrailRenderer DashTrail;
    public ParticleSystem DashParticleFX;
    public AudioSource DashSoundFx;

    public ParticleSystem WalkParticleFX;

    public Rigidbody2D rb;
    
    public float TimeSinceOnGround = 0f;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        GroundCheck();

        
    }

    public void TurnDashOffCall(float Time, string tag) {
        StartCoroutine(TurnDashOff(Time,tag));
    }

    IEnumerator TurnDashOff(float Time, string Tag) {
        yield return new WaitForSeconds(Time);
        gameObject.tag = Tag;
        
    }

    void GroundCheck() {
        GroundHit = Physics2D.CircleCast(transform.position, RaycastSize,Vector2.down ,CloseToGround,GroundLayerMask);
        if (GroundHit.collider != null) {
            OnGround = true;
            TimeSinceOnGround = 0f;
        }
        else {
            OnGround = false;
            TimeSinceOnGround += Time.deltaTime;
        }
    }

    void OnDestroy() {
        if(gameObject.scene.isLoaded) {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
    }

    void OnTriggerEnter2D(Collider2D col) {
        TriggerCollisions.Add(col.gameObject.tag, col.gameObject); 
        if (col.gameObject.tag == "Kill") {
            Destroy(gameObject);
        }
    }
    void OnTriggerExit2D(Collider2D col) {
        TriggerCollisions.Remove(col.gameObject.tag);

    }


    void OnCollisionEnter2D(Collision2D col) {

    }
}
