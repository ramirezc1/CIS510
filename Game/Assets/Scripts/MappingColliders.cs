using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MappingColliders : MonoBehaviour
{
    public WheelCollider wc;
    private Vector3 wc_Center;
    private RaycastHit hit;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        wc_Center = wc.transform.TransformPoint(wc.center);

        if(Physics.Raycast(wc_Center, -wc.transform.up, out hit, wc.suspensionDistance + wc.radius))
        {
            transform.position = hit.point + (wc.transform.up * wc.radius);
        }
        else
        {
            transform.position = wc_Center - (wc.transform.up * wc.suspensionDistance);
        }
    }
}
