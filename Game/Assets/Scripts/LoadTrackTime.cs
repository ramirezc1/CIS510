using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadTrackTime : MonoBehaviour
{
	public int MinCount;
	public int SecCount;
	public int MilliCount;

	public Text SaveTime;
	private string MinDisplay;
	private string SecDisplay;
	private string MilliDisplay;

    // Start is called before the first frame update
    void Start()
    {
        MinCount = PlayerPrefs.GetInt("FirstMinSave");
        SecCount = PlayerPrefs.GetInt("FirstSecSave");
        MilliCount = PlayerPrefs.GetInt("FirstMilliSave");

        MinDisplay = MinCount.ToString("00");
        SecDisplay = SecCount.ToString("00");
        MilliDisplay = MilliCount.ToString("00");

        SaveTime.text = MinDisplay + ":" + SecDisplay + ":" + MilliDisplay;
    }

}
