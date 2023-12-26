using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AutoLoad : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject SceneLoaderGameObject;
    void Start()
    {
        GameObject.Instantiate(SceneLoaderGameObject, Vector3.zero, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
