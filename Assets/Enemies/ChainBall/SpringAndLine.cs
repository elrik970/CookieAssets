using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringAndLine : MonoBehaviour
{
    // Start is called before the first frame update;
    public SpringJoint2D spring;
    public LineRenderer lineRend;
    void Start()
    {
        lineRend = GetComponent<LineRenderer>();
        spring = GetComponent<SpringJoint2D>();
        spring.connectedAnchor = transform.parent.position;
        lineRend.SetPosition(0,transform.parent.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
