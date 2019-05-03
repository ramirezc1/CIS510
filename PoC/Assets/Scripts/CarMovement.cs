using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovement : MonoBehaviour
{
    public int m_PlayerNumber = 1;
    public float m_Speed = 12f;
    public float m_TurnSpeed = 180f;
    public AudioSource m_MovementAudio;
    public AudioClip m_EngineIdling;
    public AudioClip m_Accelerataion;
    public AudioClip m_Deceleration;
    public float m_PitchRange = 0.2f;

    private string m_MovementAxisName;
    private string m_TurnAxisName;
    private Rigidbody m_RigidBody;
    private float m_MovementInputValue;
    private float m_TurnInputValue;
    private float m_OriginalPitch;

    private void Awake()
    {
        // storing a reference to a particular gameobject or rigid body.
        m_RigidBody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        m_RigidBody.isKinematic = false;
        m_MovementInputValue = 0f;
        m_TurnInputValue = 0f; //resetting input speed and turn 
    }
    private void OnDisable()
    {
        //when the car is turned off , it becomes kinematic it stops and all
        // forces acting on it will be nullified
        m_RigidBody.isKinematic = true;
    }
    // Start is called before the first frame update
    void Start()
    {
        //vetical and horizontal movement axis names set to Vertical1, Vertical2,
        // Horizontal1, Horizontal2 for respective players in Edit-> ProjectSettings-> input tab.
        m_MovementAxisName = "Vertical" + m_PlayerNumber;
        m_TurnAxisName = "Horizontal" + m_PlayerNumber;
        //m_MovementAudio is of type Audio Source of Unity
        m_OriginalPitch = m_MovementAudio.pitch;
        
    }

    // Update is called once per frame
    private void Update()
    {
        //Vertical# and Horizontal# keyboard inputs assigned to m_MovementInputValue and m_TurnInputValue
        m_MovementInputValue = Input.GetAxis(m_MovementAxisName);
        m_TurnInputValue = Input.GetAxis(m_TurnAxisName);
    }

    private void FixedUpdate()
    {
        //Fixed update runs every physics step instead of updating in every visual frame
        //rendered which is done in update function.
        //Here we want to move the car and increase its speed linearly and turn linearly aswell
        //Electric vehicles accelerate linearly upto a fixed speed
        Move();
        Turn();
    }

    private void Move()
    {
        // Vector in car's forward direction
        Vector3 movement = transform.forward * m_MovementInputValue * m_Speed * Time.deltaTime;
        //m_Speed = m_Speed + 0.05f;

        // Apply this movement to the rigidbody's position.
        m_RigidBody.MovePosition(m_RigidBody.position + movement);
    }

    private void Turn()
    {
        // calculating the turn speed
        float turn = m_TurnInputValue * m_TurnSpeed * Time.deltaTime;

        // Turning along the Y axis only
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);

        // applying the rotation to the car
        m_RigidBody.MoveRotation(m_RigidBody.rotation * turnRotation);
    }
}
