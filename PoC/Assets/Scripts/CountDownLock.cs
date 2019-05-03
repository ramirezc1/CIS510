using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountDownLock : MonoBehaviour
{
    private Vector3 offset;

    void Start()
    {
        offset = transform.position;
    }

    void LateUpdate()
    {
        transform.position = transform.position;
    }
}

