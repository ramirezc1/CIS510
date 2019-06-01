using UnityEngine;
using UnityEngine.Events;


public class ArrowController : MonoBehaviour
{

    public float speed = 5f;

    void LateUpdate()
    {
        Vector3 direction = Laps.checkpointA[Laps.currentCheckpoint].position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, speed * Time.deltaTime);
    }
}
