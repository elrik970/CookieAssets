using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kill : MonoBehaviour
{
    // Start is called before the first frame update
    public string[] TagsToKill;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D col) {
        foreach (string Tag in TagsToKill) {
            if (col.gameObject.CompareTag(Tag)) {
                Destroy(col.gameObject);
            }
        }
    }
    void OnCollisionEnter2D(Collision2D col) {
        foreach (string Tag in TagsToKill) {
            if (col.gameObject.CompareTag(Tag)) {
                Destroy(col.gameObject);
            }
        }
    }
}
