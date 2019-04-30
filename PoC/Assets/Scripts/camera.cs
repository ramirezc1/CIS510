using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera : MonoBehaviour
{
    public GameObject car;

    private Vector3 offset;

    void Start()
    {
        offset = transform.position - car.transform.position;
    }

    void LateUpdate()
    {
        transform.position = car.transform.position + offset;
    }
}


