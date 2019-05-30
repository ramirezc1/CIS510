using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Checkpoint : MonoBehaviour
{
	public Text checkpointText;
	public GameObject optionMenu;
	public Text rankText;
	void  Start ()
	{

	}
	
	void  OnTriggerEnter ( Collider other  )
	{
		//Is it the Player who enters the collider?
		if (!other.CompareTag("Player")) 
			return; //If it's not the player dont continue




		if (transform == Laps.checkpointA[Laps.currentCheckpoint].transform) 
		{
			checkpointText.text = "You are at checkpoint " + Laps.currentCheckpoint;
			//Check so we dont exceed our checkpoint quantity
			if (Laps.currentCheckpoint + 1 < Laps.checkpointA.Length) 
			{
				//Add to currentLap if currentCheckpoint is 0
				if(Laps.currentCheckpoint == 0){
					if(Laps.currentLap == Laps.totalLaps){
						
						Time.timeScale = 0f;
						optionMenu.gameObject.SetActive(true);
						rankText.text = "Your rank is " + (Laps.rank + 1);
					}
					Laps.currentLap++;
				}
				Laps.currentCheckpoint++;
			} 
			else 
			{
				//If we dont have any Checkpoints left, go back to 0
				Laps.currentCheckpoint = 0;


			}
		}
 
 
	}

}
