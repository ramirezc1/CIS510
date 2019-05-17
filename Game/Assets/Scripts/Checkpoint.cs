using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
	
	void  Start ()
	{

	}
	
	void  OnTriggerEnter ( Collider other  )
	{
		//Is it the Player who enters the collider?
		if (!other.CompareTag("Player")) 
			return; //If it's not the player dont continue
		


	}

}
