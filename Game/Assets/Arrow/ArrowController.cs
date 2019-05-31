using UnityEngine;
using UnityEngine.Events;


public class ArrowController : MonoBehaviour
{
    public Transform m_Targets;
    //public Transform target;
    public float speed = 5f;

    void LateUpdate()
    {
        Vector3 direction = m_Targets.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, speed * Time.deltaTime);
    }
}
