using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Checkpoint : MonoBehaviour
{
	public int laps;
	private int checkpoints;

	void  Start ()
	{
		checkpoints = 0;
	}
	
	void  OnTriggerEnter ( Collider other  )
	{
		//Is it the Player who enters the collider?
		if (other.CompareTag("Player")) {
			checkpoints++;
		}
		if (checkpoints == laps){
			GameObject.Find("Player").SendMessage("Finish");
		}


	}

}
