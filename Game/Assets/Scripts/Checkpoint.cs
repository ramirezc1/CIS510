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

	public static int playerCount = 0;
	void  Start ()
	{

	}
	
	void  OnTriggerEnter ( Collider other  )
	{
		Debug.Log(other.gameObject.name);
		//Is it the Player who enters the collider?
		if (!other.CompareTag("Player")) 
			return; //If it's not the player dont continue

		if(other.gameObject.name == "Car1"){
			if (transform == Laps.checkpointA[Laps.currentCheckpoint1].transform) 
			{
				checkpointText.text = "You are at checkpoint " + Laps.currentCheckpoint1;
				//Check so we dont exceed our checkpoint quantity
				if (Laps.currentCheckpoint1 + 1 < Laps.checkpointA.Length) 
				{
					//Add to currentLap if currentCheckpoint is 0
					if(Laps.currentCheckpoint1 == 0){
						if(Laps.currentLap1 == Laps.totalLaps){
							playerCount++;
							Timer.Finished();

							if(Timer.finished == true){
								Time.timeScale = 0f;
								optionMenu.gameObject.SetActive(true);
								
							}
							rankText.text = "Your rank is " + playerCount;

						}
						Laps.currentLap1++;
					}
					Laps.currentCheckpoint1++;
				} 
				else 
				{
					//If we dont have any Checkpoints left, go back to 0
					Laps.currentCheckpoint1 = 0;


				}
			}
		}

		else if(other.gameObject.name == "Car2"){
			if (transform == Laps.checkpointA[Laps.currentCheckpoint2].transform) 
			{
				checkpointText.text = "You are at checkpoint " + Laps.currentCheckpoint2;
				//Check so we dont exceed our checkpoint quantity
				if (Laps.currentCheckpoint2 + 1 < Laps.checkpointA.Length) 
				{
					//Add to currentLap if currentCheckpoint is 0
					if(Laps.currentCheckpoint2 == 0){
						if(Laps.currentLap2 == Laps.totalLaps){
							playerCount++;
							Timer.Finished();

							if(Timer.finished == true){
								Time.timeScale = 0f;
								optionMenu.gameObject.SetActive(true);
								
							}
							rankText.text = "Your rank is " + playerCount;

						}
						Laps.currentLap2++;
					}
					Laps.currentCheckpoint2++;
				} 
				else 
				{
					//If we dont have any Checkpoints left, go back to 0
					Laps.currentCheckpoint2 = 0;


				}
			}
		}

 
 
	}

}
