using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Ranking : MonoBehaviour
{

	public static int rank;
	public GameObject[] playerArray;
    public static GameObject[] playerA;
    // Start is called before the first frame update
    void Start()
    {
    	rank = 0;  
        playerA = playerArray; 
    }

}
