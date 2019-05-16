using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedStartControl : MonoBehaviour
{

	public GameObject countDown1;
    public GameObject countDown2;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("StartDelay");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator StartDelay(){
    	Time.timeScale = 0;
    	float pauseTime = Time.realtimeSinceStartup + 3f;
    	while(Time.realtimeSinceStartup < pauseTime){
    		yield return 0;
    	}
    	countDown1.gameObject.SetActive(false);
        countDown2.gameObject.SetActive(false);
    	Time.timeScale = 1;
    }
}
