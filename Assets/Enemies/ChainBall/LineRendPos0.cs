using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRendPos0 : MonoBehaviour
{
    // Start is called before the first frame update
    public LineRenderer Rend;
    void Start()
    {
        Rend = GetComponent<LineRenderer>();
        Rend.SetPosition(1,transform.parent.position);
    }

    // Update is called once per frame
    void Update()
    {
        Rend.SetPosition(0,transform.position);
    }
}
