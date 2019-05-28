using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laps : MonoBehaviour
{
    public Transform[] checkPointArray;
    public static Transform[] checkpointA;
    public static int currentCheckpoint; 
    public static int currentLap; 
    public Vector3 startPos;
    public int Lap;
    public static int totalLaps;

    void  Start ()
    {
        startPos = transform.position;
        currentCheckpoint = 0;
        currentLap = 0; 

    }

    void  Update ()
    {
        checkpointA = checkPointArray;
        totalLaps = Lap;
    }
     
}

