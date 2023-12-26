using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashOn : MonoBehaviour
{
    // Start is called before the first frame update
    public float TimeTurnedOff;
    public AudioSource HitFX;
    public ParticleSystem HitParticleFX;

    void Start()
    {
        HitFX = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.tag == "PlayerDashing") {
            // col.GetComponent<Player>().canDash = true;
            HitFX.Play();
            HitParticleFX.Play();
        }
    }
}
