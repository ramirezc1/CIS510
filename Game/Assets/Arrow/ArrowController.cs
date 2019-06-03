using UnityEngine;
using UnityEngine.Events;


public class ArrowController : MonoBehaviour
{

    public float speed = 5f;
    public int arrowID;

    void LateUpdate()
    {
    	if(arrowID == 1){
    		Vector3 direction = Laps.checkpointA[Laps.currentCheckpoint1].position - transform.position;
	        Quaternion rotation = Quaternion.LookRotation(direction);
	        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, speed * Time.deltaTime);
    	}
    	else if(arrowID == 2){
    		Vector3 direction = Laps.checkpointA[Laps.currentCheckpoint2].position - transform.position;
	        Quaternion rotation = Quaternion.LookRotation(direction);
	        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, speed * Time.deltaTime);
    	}
        
    }
}
