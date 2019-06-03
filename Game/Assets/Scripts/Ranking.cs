using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Ranking : MonoBehaviour
{

	public static int rank;
	public GameObject[] playerArray;
    // Start is called before the first frame update
    void Start()
    {
    	rank = 0;   
    }

    // Update is called once per frame
    void Update()
    {
    	Vector3 direction;
    	int[] distance = new int[playerArray.Length];
    	for(int i = 0; i < playerArray.Length; i++){
    		direction = Laps.checkpointA[Laps.currentCheckpoint].position - transform.position;
    		distance[i] = (int) Vector3.Dot(direction, direction);
    	}
    	int min = distance.Min();
    	int minPlayer = distance.ToList().IndexOf(min);

    	
    }
}
