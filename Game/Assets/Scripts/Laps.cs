using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laps : MonoBehaviour
{
    public Transform[] checkPointArray;
    public static Transform[] checkpointA;
    public static int currentCheckpoint1; 
    public static int currentLap1; 
    public static int currentCheckpoint2; 
    public static int currentLap2; 
    public Vector3 startPos;
    public int Lap;
    public static int totalLaps;

    void  Start ()
    {
        startPos = transform.position;
        currentCheckpoint1 = 0;
        currentLap1 = 0; 
        currentCheckpoint2 = 0;
        currentLap2 = 0; 
    }

    void  Update ()
    {
        checkpointA = checkPointArray;
        totalLaps = Lap;
    }
     
}

