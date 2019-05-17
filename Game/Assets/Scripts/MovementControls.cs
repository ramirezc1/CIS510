using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementControls : MonoBehaviour
{
    private string m_MovementAxisName;
    private string m_SteerAxisName;
    private string m_HandBrakeName;

    public string m_BrakeKey;
    public int m_PlayerNumber;

    public WheelCollider[] wc = new WheelCollider[4];
    public Transform[] tyres = new Transform[4];

    //public List<Transform> tyres = new List<Transform>();


    public float m_Speed;
    public float max_Torque = 2500f;
    public float m_Steer ;
    public float m_Brake = 10000f;
    public Vector3 m_CenterOfMass = new Vector3(0, 0, 0);
    public float m_DecelerationSpeed = 1000f;


   


    public int wc_Torque_Length;
    public bool AllowBrake;


    Rigidbody m_rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        m_rigidbody.centerOfMass = m_CenterOfMass;

        m_MovementAxisName = "Vertical" + m_PlayerNumber;
        m_SteerAxisName = "Horizontal" + m_PlayerNumber;
        m_HandBrakeName = m_BrakeKey + "Shift";
    }

    // Update is called once per frame
    void Update()
    {
        HandBrake();
        RotatingRTyres();
       
    }

    private void FixedUpdate()
    {


        CarMovement();
        DecelerationSpeed();

    }

    private void RotatingRTyres()
    {
        for( int i = 0; i < tyres.Length; i++)
        {

            tyres[i].Rotate(wc[i].rpm / 60 * 360 * Time.deltaTime, 0f, 0f);
        }

        for ( int i =0; i < tyres.Length; i++)
        {
            tyres[i].localEulerAngles = new Vector3(tyres[i].localEulerAngles.x, wc[i].steerAngle - tyres[i].localEulerAngles.z, tyres[i].localScale.z);
        }
    }

    private void CarMovement()
    {



            for (int i = 0; i < wc_Torque_Length; i++)
            {
                wc[i].motorTorque = Input.GetAxis(m_MovementAxisName) * max_Torque;
            }
        
        wc[0].steerAngle = Input.GetAxis(m_SteerAxisName) * m_Steer;
        wc[1].steerAngle = Input.GetAxis(m_SteerAxisName) * m_Steer;

    }

    private void DecelerationSpeed()
    {
        if(!AllowBrake && Input.GetButton(m_MovementAxisName) == false)
        {
            for(int i =0; i < wc_Torque_Length; i++)
            {
                wc[i].brakeTorque = m_DecelerationSpeed;
                wc[i].motorTorque = 0;
            }
        }
    }

    private void HandBrake()
    {
        if(Input.GetKey(KeyCode.RightShift))
        {
            AllowBrake = true;
        }
        else
        {
            AllowBrake = false;
        }

        if (AllowBrake)
        {
            for (int i = 0; i < wc_Torque_Length; i++)
            {
                wc[i].brakeTorque = m_Brake;
                wc[i].motorTorque = 0f;
            }
        } 
        else if (!AllowBrake && Input.GetButton(m_MovementAxisName) == true)
        {
            for (int i = 0; i < wc_Torque_Length; i++)
            {
                wc[i].brakeTorque = 0f;
            }
        }
    }

  
}
