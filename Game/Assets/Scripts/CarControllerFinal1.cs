using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarControllerFinal1 : MonoBehaviour
{
    private string m_MovementAxisName;
    private string m_SteerAxisName;
    private string m_HandBrakeName;

    private float m_horizontalInput;
    private float m_verticalInput;
    private float m_steeringAngle;


    public WheelCollider frontDriverW, frontPassengerW;
    public WheelCollider rearDriverW, rearPassengerW;
    public Transform frontDriverT, frontPassengerT;
    public Transform rearDriverT, rearPassengerT;
    public float maxSteerAngle;
    public float motorSpeed;

    public float max_Torque = 2500f;
    public float m_Brake = 10000f;
    public int m_PlayerNumber;
    public string m_BrakeKey;
    public float m_DecelerationSpeed = 1000f;




    public bool AllowBrake;

    void Start()
    {
       

        m_MovementAxisName = "Vertical" + m_PlayerNumber;
        m_SteerAxisName = "Horizontal" + m_PlayerNumber;
        m_HandBrakeName = m_BrakeKey + "Shift";
    }

    public void GetInput()
    {
        m_horizontalInput = Input.GetAxis(m_SteerAxisName);
        m_verticalInput = Input.GetAxis(m_MovementAxisName);
       
    }

    private void Steer()
    {
        m_steeringAngle = maxSteerAngle * m_horizontalInput;
        frontDriverW.steerAngle = m_steeringAngle;
        frontPassengerW.steerAngle = m_steeringAngle;
    }

    private void Accelerate()
    {
        frontDriverW.motorTorque = m_verticalInput * max_Torque;
        frontPassengerW.motorTorque = m_verticalInput * max_Torque;
        rearDriverW.motorTorque = m_verticalInput * max_Torque;
        rearPassengerW.motorTorque = m_verticalInput * max_Torque;
    }

    private void UpdateWheelPoses()
    {
        UpdateWheelPose(frontDriverW, frontDriverT);
        UpdateWheelPose(frontPassengerW, frontPassengerT);
        UpdateWheelPose(rearDriverW, rearDriverT);
        UpdateWheelPose(rearPassengerW, rearPassengerT);
    }

    private void UpdateWheelPose(WheelCollider _collider, Transform _transform)
    {
        Vector3 _pos = _transform.position;
        Quaternion _quat = _transform.rotation;

        _collider.GetWorldPose(out _pos, out _quat);

        _transform.position = _pos;
        _transform.rotation = _quat;
    }

 

    //private void HandBrake()
    //{
    //    if (Input.GetKey(KeyCode.RightShift))
    //    {
    //        AllowBrake = true;
    //    }
    //    else
    //    {
    //        AllowBrake = false;
    //    }

    //    if (AllowBrake)
    //    {

    //        frontDriverW.brakeTorque = m_Brake;
    //        frontPassengerW.brakeTorque = m_Brake;
    //        rearDriverW.brakeTorque = m_Brake;
    //        rearPassengerW.brakeTorque = m_Brake;

    //        frontDriverW.motorTorque = 0;
    //        frontPassengerW.motorTorque = 0;
    //        rearDriverW.motorTorque = 0;
    //        rearPassengerW.motorTorque = 0;

    //    }
    //    else if (!AllowBrake && Input.GetButton(m_MovementAxisName) == true)
    //    {
    //        frontDriverW.motorTorque = 0;
    //        frontPassengerW.motorTorque = 0;
    //        rearDriverW.motorTorque = 0;
    //        rearPassengerW.motorTorque = 0;
    //    }
    //}

    void Update()
    {
        //HandBrake();


    }
    private void FixedUpdate()
    {
        GetInput();
        Steer();
        Accelerate();
        UpdateWheelPoses();

    }


}




//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class CarControllerFinal1 : MonoBehaviour
//{
//    private string m_MovementAxisName;
//    private string m_SteerAxisName;
//    private string m_HandBrakeName;

//    private float m_horizontalInput;
//    private float m_verticalInput;
//    private float m_steeringAngle;


//    public WheelCollider frontDriverW, frontPassengerW;
//    public WheelCollider rearDriverW, rearPassengerW;
//    public Transform frontDriverT, frontPassengerT;
//    public Transform rearDriverT, rearPassengerT;
//    public float maxSteerAngle;
//    public float motorSpeed;

//    public float max_Torque = 2500f;
//    public float m_Brake = 10000f;
//    public int m_PlayerNumber;
//    public string m_BrakeKey;
//    public float m_DecelerationSpeed = 1000f;




//    public bool AllowBrake;

//    void Start()
//    {


//        m_MovementAxisName = "Vertical" + m_PlayerNumber;
//        m_SteerAxisName = "Horizontal" + m_PlayerNumber;
//        m_HandBrakeName = m_BrakeKey + "Shift";
//    }

//    public void GetInput()
//    {
//        m_horizontalInput = Input.GetAxis(m_SteerAxisName);
//        m_verticalInput = Input.GetAxis(m_MovementAxisName);

//    }

//    private void Steer()
//    {
//        m_steeringAngle = maxSteerAngle * m_horizontalInput;
//        frontDriverW.steerAngle = m_steeringAngle;
//        frontPassengerW.steerAngle = m_steeringAngle;
//    }

//    private void Accelerate()
//    {
//        frontDriverW.motorTorque = m_verticalInput * max_Torque;
//        frontPassengerW.motorTorque = m_verticalInput * max_Torque;
//        rearDriverW.motorTorque = m_verticalInput * max_Torque;
//        rearPassengerW.motorTorque = m_verticalInput * max_Torque;
//    }

//    private void UpdateWheelPoses()
//    {
//        UpdateWheelPose(frontDriverW, frontDriverT);
//        UpdateWheelPose(frontPassengerW, frontPassengerT);
//        UpdateWheelPose(rearDriverW, rearDriverT);
//        UpdateWheelPose(rearPassengerW, rearPassengerT);
//    }

//    private void UpdateWheelPose(WheelCollider _collider, Transform _transform)
//    {
//        Vector3 _pos = _transform.position;
//        Quaternion _quat = _transform.rotation;

//        _collider.GetWorldPose(out _pos, out _quat);

//        _transform.position = _pos;
//        _transform.rotation = _quat;
//    }

//    private void DecelerationSpeed()
//    {
//        if (!AllowBrake && Input.GetButton(m_MovementAxisName) == false)
//        {
//            frontDriverW.brakeTorque = m_DecelerationSpeed;
//            frontPassengerW.brakeTorque = m_DecelerationSpeed;
//            rearDriverW.brakeTorque = m_DecelerationSpeed;
//            rearPassengerW.brakeTorque = m_DecelerationSpeed;

//            frontDriverW.motorTorque = 0;
//            frontPassengerW.motorTorque = 0;
//            rearDriverW.motorTorque = 0;
//            rearPassengerW.motorTorque = 0;
//        }
//    }

//    //private void HandBrake()
//    //{
//    //    if (Input.GetKey(KeyCode.RightShift))
//    //    {
//    //        AllowBrake = true;
//    //    }
//    //    else
//    //    {
//    //        AllowBrake = false;
//    //    }

//    //    if (AllowBrake)
//    //    {

//    //        frontDriverW.brakeTorque = m_Brake;
//    //        frontPassengerW.brakeTorque = m_Brake;
//    //        rearDriverW.brakeTorque = m_Brake;
//    //        rearPassengerW.brakeTorque = m_Brake;

//    //        frontDriverW.motorTorque = 0;
//    //        frontPassengerW.motorTorque = 0;
//    //        rearDriverW.motorTorque = 0;
//    //        rearPassengerW.motorTorque = 0;

//    //    }
//    //    else if (!AllowBrake && Input.GetButton(m_MovementAxisName) == true)
//    //    {
//    //        frontDriverW.motorTorque = 0;
//    //        frontPassengerW.motorTorque = 0;
//    //        rearDriverW.motorTorque = 0;
//    //        rearPassengerW.motorTorque = 0;
//    //    }
//    //}

//    void Update()
//    {
//        //HandBrake();


//    }
//    private void FixedUpdate()
//    {
//        GetInput();
//        Steer();
//        Accelerate();
//        UpdateWheelPoses();
//        DecelerationSpeed();
//    }


//}