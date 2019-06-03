using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
	public Text timerText;
	private float startTime;
    public static bool finished;

    private static int minutes;
    private static int seconds;
    private static int milliseconds;
    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
        finished = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(finished){
            return;
        }
        float t = Time.time - startTime;

        minutes = (int) (t/60);
        seconds = (int) (t % 60);
        milliseconds = ((int) (t*100f)%100);

        string minutesText = minutes.ToString("00");
        string secondsText = seconds.ToString("00");
        string millisecondsText = milliseconds.ToString("00");

        timerText.text = minutesText + ":" + secondsText + ":" + millisecondsText;
    }

    public static void Finished(){
        if(Checkpoint.playerCount == Ranking.playerA.Length){
            finished = true;
        }
        else{
            PlayerPrefs.SetInt ("FirstMinSave", minutes);
            PlayerPrefs.SetInt ("FirstSecSave", seconds);
            PlayerPrefs.SetInt ("FirstMilliSave", milliseconds);
    	}
        

    }
}
