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

    public static int playerID;


    public WheelCollider[] wc = new WheelCollider[4];
    public Transform[] tyres = new Transform[4];


    public float m_CurrentSpeed;
    public float m_FwdSpeed;


    public float m_Speed;
    public float max_Torque = 2500f;
    public float m_Steer ;
    public float m_Brake = 10000f;
    public Vector3 m_CenterOfMass = new Vector3(0, 0, 0);
    public float m_DecelerationSpeed = 1000f;
    

   

    private float m_OriginalPitch;
    public float m_PitchRange = 0.2f;
    public List<AudioSource> CarSound;

    private float m_MovementInputValue;
    private float m_SteerInputValue;

    public int wc_Torque_Length;
    public bool AllowBrake;


    Rigidbody m_rigidbody;

    public float[] MinRpmTable = { 50.0f, 75.0f, 112.0f, 166.9f, 222.4f, 278.3f, 333.5f, 388.2f, 435.5f, 483.3f, 538.4f, 594.3f, 643.6f, 692.8f, 741.9f, 790.0f };
    public float[] NormalRpmTable = { 72.0f, 93.0f, 155.9f, 202.8f, 267.0f, 314.5f, 377.4f, 423.9f, 472.1f, 519.4f, 582.3f, 631.3f, 680.8f, 729.4f, 778.8f, 826.1f };
    public float[] MaxRpmTable = { 92.0f, 136.0f, 182.9f, 247.4f, 294.3f, 357.5f, 403.6f, 452.5f, 499.3f, 562.5f, 612.3f, 661.6f, 708.8f, 758.9f, 806.0f, 1000.0f };
    public float[] PitchingTable = { 0.12f, 0.12f, 0.12f, 0.12f, 0.11f, 0.10f, 0.09f, 0.08f, 0.06f, 0.06f, 0.06f, 0.06f, 0.06f, 0.06f, 0.06f, 0.06f };
    public float RangeDivider = 4f;
    public float SoundRPM;


    // Start is called before the first frame update
    private void Start()
    {
        playerID = m_PlayerNumber;

        m_rigidbody = GetComponent<Rigidbody>();

        m_rigidbody.centerOfMass = m_CenterOfMass;

        m_MovementAxisName = "Vertical" + m_PlayerNumber;
        m_SteerAxisName = "Horizontal" + m_PlayerNumber;
        m_HandBrakeName = m_BrakeKey + "Shift";



        for(int i = 1; i<=16; ++i)
        {
            CarSound.Add(GameObject.Find(string.Format("CarSound ({0})", i)).GetComponent<AudioSource>());
            CarSound[i - 1].Play();

        }


    }

    // Update is called once per frame
    private void Update()
    {
        m_FwdSpeed = GetComponent<Rigidbody>().velocity.magnitude * 3.6f;
        m_CurrentSpeed = Vector3.Dot(m_rigidbody.velocity, transform.forward);

        SoundRPM = Mathf.Round(m_CurrentSpeed * (1000 / 80));


        HandBrake();
        RotatingRTyres(); 


        m_MovementInputValue = Input.GetAxis(m_MovementAxisName);
        m_SteerInputValue = Input.GetAxis(m_SteerAxisName);
        CARSOUND();
       
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
        if(Input.GetKey(KeyCode.LeftShift))
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

  

    private void CARSOUND()
    {
        for (int i = 0; i < 16; i++)
        {
            if (i == 0)
            {
                //Set CarSound[0]
                if (SoundRPM < MinRpmTable[i])
                {
                    CarSound[0].volume = 0.0f;
                }
                else if (SoundRPM >= MinRpmTable[i] && SoundRPM < NormalRpmTable[i])
                {
                    float Range = NormalRpmTable[i] - MinRpmTable[i];
                    float ReducedRPM = SoundRPM - MinRpmTable[i];
                    CarSound[0].volume = ReducedRPM / Range;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    CarSound[0].pitch = 1f - PitchingTable[i] + PitchMath;
                }
                else if (SoundRPM >= NormalRpmTable[i] && SoundRPM <= MaxRpmTable[i])
                {
                    float Range = MaxRpmTable[i] - NormalRpmTable[i];
                    float ReducedRPM = SoundRPM - NormalRpmTable[i];
                    CarSound[0].volume = 1f;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    CarSound[0].pitch = 1f + PitchMath;
                }
                else if (SoundRPM > MaxRpmTable[i])
                {
                    float Range = (MaxRpmTable[i + 1] - MaxRpmTable[i]) / RangeDivider;
                    float ReducedRPM = SoundRPM - MaxRpmTable[i];
                    CarSound[0].volume = 1f - ReducedRPM / Range;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    //CarSound[0].pitch = 1f + PitchingTable[i] + PitchMath;
                }
            }
            else if (i == 1)
            {
                //Set CarSound[1]
                if (SoundRPM < MinRpmTable[i])
                {
                    CarSound[1].volume = 0.0f;
                }
                else if (SoundRPM >= MinRpmTable[i] && SoundRPM < NormalRpmTable[i])
                {
                    float Range = NormalRpmTable[i] - MinRpmTable[i];
                    float ReducedRPM = SoundRPM - MinRpmTable[i];
                    CarSound[1].volume = ReducedRPM / Range;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    CarSound[1].pitch = 1f - PitchingTable[i] + PitchMath;
                }
                else if (SoundRPM >= NormalRpmTable[i] && SoundRPM <= MaxRpmTable[i])
                {
                    float Range = MaxRpmTable[i] - NormalRpmTable[i];
                    float ReducedRPM = SoundRPM - NormalRpmTable[i];
                    CarSound[1].volume = 1f;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    CarSound[1].pitch = 1f + PitchMath;
                }
                else if (SoundRPM > MaxRpmTable[i])
                {
                    float Range = (MaxRpmTable[i + 1] - MaxRpmTable[i]) / RangeDivider;
                    float ReducedRPM = SoundRPM - MaxRpmTable[i];
                    CarSound[1].volume = 1f - ReducedRPM / Range;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    //CarSound[1].pitch = 1f + PitchingTable[i] + PitchMath;
                }
            }
            else if (i == 2)
            {
                //Set CarSound[2]
                if (SoundRPM < MinRpmTable[i])
                {
                    CarSound[2].volume = 0.0f;
                }
                else if (SoundRPM >= MinRpmTable[i] && SoundRPM < NormalRpmTable[i])
                {
                    float Range = NormalRpmTable[i] - MinRpmTable[i];
                    float ReducedRPM = SoundRPM - MinRpmTable[i];
                    CarSound[2].volume = ReducedRPM / Range;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    CarSound[2].pitch = 1f - PitchingTable[i] + PitchMath;
                }
                else if (SoundRPM >= NormalRpmTable[i] && SoundRPM <= MaxRpmTable[i])
                {
                    float Range = MaxRpmTable[i] - NormalRpmTable[i];
                    float ReducedRPM = SoundRPM - NormalRpmTable[i];
                    CarSound[2].volume = 1f;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    CarSound[2].pitch = 1f + PitchMath;
                }
                else if (SoundRPM > MaxRpmTable[i])
                {
                    float Range = (MaxRpmTable[i + 1] - MaxRpmTable[i]) / RangeDivider;
                    float ReducedRPM = SoundRPM - MaxRpmTable[i];
                    CarSound[2].volume = 1f - ReducedRPM / Range;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    //CarSound[2].pitch = 1f + PitchingTable[i] + PitchMath;
                }
            }
            else if (i == 3)
            {
                //Set CarSound[3]
                if (SoundRPM < MinRpmTable[i])
                {
                    CarSound[3].volume = 0.0f;
                }
                else if (SoundRPM >= MinRpmTable[i] && SoundRPM < NormalRpmTable[i])
                {
                    float Range = NormalRpmTable[i] - MinRpmTable[i];
                    float ReducedRPM = SoundRPM - MinRpmTable[i];
                    CarSound[3].volume = ReducedRPM / Range;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    CarSound[3].pitch = 1f - PitchingTable[i] + PitchMath;
                }
                else if (SoundRPM >= NormalRpmTable[i] && SoundRPM <= MaxRpmTable[i])
                {
                    float Range = MaxRpmTable[i] - NormalRpmTable[i];
                    float ReducedRPM = SoundRPM - NormalRpmTable[i];
                    CarSound[3].volume = 1f;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    CarSound[3].pitch = 1f + PitchMath;
                }
                else if (SoundRPM > MaxRpmTable[i])
                {
                    float Range = (MaxRpmTable[i + 1] - MaxRpmTable[i]) / RangeDivider;
                    float ReducedRPM = SoundRPM - MaxRpmTable[i];
                    CarSound[3].volume = 1f - ReducedRPM / Range;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    //CarSound[3].pitch = 1f + PitchingTable[i] + PitchMath;
                }
            }
            else if (i == 4)
            {
                //Set CarSound[4]
                if (SoundRPM < MinRpmTable[i])
                {
                    CarSound[4].volume = 0.0f;
                }
                else if (SoundRPM >= MinRpmTable[i] && SoundRPM < NormalRpmTable[i])
                {
                    float Range = NormalRpmTable[i] - MinRpmTable[i];
                    float ReducedRPM = SoundRPM - MinRpmTable[i];
                    CarSound[4].volume = ReducedRPM / Range;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    CarSound[4].pitch = 1f - PitchingTable[i] + PitchMath;
                }
                else if (SoundRPM >= NormalRpmTable[i] && SoundRPM <= MaxRpmTable[i])
                {
                    float Range = MaxRpmTable[i] - NormalRpmTable[i];
                    float ReducedRPM = SoundRPM - NormalRpmTable[i];
                    CarSound[4].volume = 1f;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    CarSound[4].pitch = 1f + PitchMath;
                }
                else if (SoundRPM > MaxRpmTable[i])
                {
                    float Range = (MaxRpmTable[i + 1] - MaxRpmTable[i]) / RangeDivider;
                    float ReducedRPM = SoundRPM - MaxRpmTable[i];
                    CarSound[4].volume = 1f - ReducedRPM / Range;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    //CarSound[4].pitch = 1f + PitchingTable[i] + PitchMath;
                }
            }
            else if (i == 5)
            {
                //Set CarSound[5]
                if (SoundRPM < MinRpmTable[i])
                {
                    CarSound[5].volume = 0.0f;
                }
                else if (SoundRPM >= MinRpmTable[i] && SoundRPM < NormalRpmTable[i])
                {
                    float Range = NormalRpmTable[i] - MinRpmTable[i];
                    float ReducedRPM = SoundRPM - MinRpmTable[i];
                    CarSound[5].volume = ReducedRPM / Range;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    CarSound[5].pitch = 1f - PitchingTable[i] + PitchMath;
                }
                else if (SoundRPM >= NormalRpmTable[i] && SoundRPM <= MaxRpmTable[i])
                {
                    float Range = MaxRpmTable[i] - NormalRpmTable[i];
                    float ReducedRPM = SoundRPM - NormalRpmTable[i];
                    CarSound[5].volume = 1f;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    CarSound[5].pitch = 1f + PitchMath;
                }
                else if (SoundRPM > MaxRpmTable[i])
                {
                    float Range = (MaxRpmTable[i + 1] - MaxRpmTable[i]) / RangeDivider;
                    float ReducedRPM = SoundRPM - MaxRpmTable[i];
                    CarSound[5].volume = 1f - ReducedRPM / Range;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    //CarSound[5].pitch = 1f + PitchingTable[i] + PitchMath;
                }
            }
            else if (i == 6)
            {
                //Set CarSound[6]
                if (SoundRPM < MinRpmTable[i])
                {
                    CarSound[6].volume = 0.0f;
                }
                else if (SoundRPM >= MinRpmTable[i] && SoundRPM < NormalRpmTable[i])
                {
                    float Range = NormalRpmTable[i] - MinRpmTable[i];
                    float ReducedRPM = SoundRPM - MinRpmTable[i];
                    CarSound[6].volume = ReducedRPM / Range;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    CarSound[6].pitch = 1f - PitchingTable[i] + PitchMath;
                }
                else if (SoundRPM >= NormalRpmTable[i] && SoundRPM <= MaxRpmTable[i])
                {
                    float Range = MaxRpmTable[i] - NormalRpmTable[i];
                    float ReducedRPM = SoundRPM - NormalRpmTable[i];
                    CarSound[6].volume = 1f;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    CarSound[6].pitch = 1f + PitchMath;
                }
                else if (SoundRPM > MaxRpmTable[i])
                {
                    float Range = (MaxRpmTable[i + 1] - MaxRpmTable[i]) / RangeDivider;
                    float ReducedRPM = SoundRPM - MaxRpmTable[i];
                    CarSound[6].volume = 1f - ReducedRPM / Range;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    //CarSound[6].pitch = 1f + PitchingTable[i] + PitchMath;
                }
            }
            else if (i == 7)
            {
                //Set  
                if (SoundRPM < MinRpmTable[i])
                {
                    CarSound[7].volume = 0.0f;
                }
                else if (SoundRPM >= MinRpmTable[i] && SoundRPM < NormalRpmTable[i])
                {
                    float Range = NormalRpmTable[i] - MinRpmTable[i];
                    float ReducedRPM = SoundRPM - MinRpmTable[i];
                    CarSound[7].volume = ReducedRPM / Range;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    CarSound[7].pitch = 1f - PitchingTable[i] + PitchMath;
                }
                else if (SoundRPM >= NormalRpmTable[i] && SoundRPM <= MaxRpmTable[i])
                {
                    float Range = MaxRpmTable[i] - NormalRpmTable[i];
                    float ReducedRPM = SoundRPM - NormalRpmTable[i];
                    CarSound[7].volume = 1f;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    CarSound[7].pitch = 1f + PitchMath;
                }
                else if (SoundRPM > MaxRpmTable[i])
                {
                    float Range = (MaxRpmTable[i + 1] - MaxRpmTable[i]) / RangeDivider;
                    float ReducedRPM = SoundRPM - MaxRpmTable[i];
                    CarSound[7].volume = 1f - ReducedRPM / Range;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    //CarSound[7].pitch = 1f + PitchingTable[i] + PitchMath;
                }
            }
            else if (i == 8)
            {
                //Set CarSound[8]
                if (SoundRPM < MinRpmTable[i])
                {
                    CarSound[8].volume = 0.0f;
                }
                else if (SoundRPM >= MinRpmTable[i] && SoundRPM < NormalRpmTable[i])
                {
                    float Range = NormalRpmTable[i] - MinRpmTable[i];
                    float ReducedRPM = SoundRPM - MinRpmTable[i];
                    CarSound[8].volume = ReducedRPM / Range;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    CarSound[8].pitch = 1f - PitchingTable[i] + PitchMath;
                }
                else if (SoundRPM >= NormalRpmTable[i] && SoundRPM <= MaxRpmTable[i])
                {
                    float Range = MaxRpmTable[i] - NormalRpmTable[i];
                    float ReducedRPM = SoundRPM - NormalRpmTable[i];
                    CarSound[8].volume = 1f;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    CarSound[8].pitch = 1f + PitchMath;
                }
                else if (SoundRPM > MaxRpmTable[i])
                {
                    float Range = (MaxRpmTable[i + 1] - MaxRpmTable[i]) / RangeDivider;
                    float ReducedRPM = SoundRPM - MaxRpmTable[i];
                    CarSound[8].volume = 1f - ReducedRPM / Range;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    //CarSound[8].pitch = 1f + PitchingTable[i] + PitchMath;
                }
            }
            else if (i == 9)
            {
                //Set CarSound[9]
                if (SoundRPM < MinRpmTable[i])
                {
                    CarSound[9].volume = 0.0f;
                }
                else if (SoundRPM >= MinRpmTable[i] && SoundRPM < NormalRpmTable[i])
                {
                    float Range = NormalRpmTable[i] - MinRpmTable[i];
                    float ReducedRPM = SoundRPM - MinRpmTable[i];
                    CarSound[9].volume = ReducedRPM / Range;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    CarSound[9].pitch = 1f - PitchingTable[i] + PitchMath;
                }
                else if (SoundRPM >= NormalRpmTable[i] && SoundRPM <= MaxRpmTable[i])
                {
                    float Range = MaxRpmTable[i] - NormalRpmTable[i];
                    float ReducedRPM = SoundRPM - NormalRpmTable[i];
                    CarSound[9].volume = 1f;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    CarSound[9].pitch = 1f + PitchMath;
                }
                else if (SoundRPM > MaxRpmTable[i])
                {
                    float Range = (MaxRpmTable[i + 1] - MaxRpmTable[i]) / RangeDivider;
                    float ReducedRPM = SoundRPM - MaxRpmTable[i];
                    CarSound[9].volume = 1f - ReducedRPM / Range;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    //CarSound[9].pitch = 1f + PitchingTable[i] + PitchMath;
                }
            }
            else if (i == 10)
            {
                //Set CarSound[10]
                if (SoundRPM < MinRpmTable[i])
                {
                    CarSound[10].volume = 0.0f;
                }
                else if (SoundRPM >= MinRpmTable[i] && SoundRPM < NormalRpmTable[i])
                {
                    float Range = NormalRpmTable[i] - MinRpmTable[i];
                    float ReducedRPM = SoundRPM - MinRpmTable[i];
                    CarSound[10].volume = ((ReducedRPM / Range) * 2f) - 1f;
                    //CarSound[10].volume = 0.0f;wwww
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    CarSound[10].pitch = 1f - PitchingTable[i] + PitchMath;
                }
                else if (SoundRPM >= NormalRpmTable[i] && SoundRPM <= MaxRpmTable[i])
                {
                    float Range = MaxRpmTable[i] - NormalRpmTable[i];
                    float ReducedRPM = SoundRPM - NormalRpmTable[i];
                    CarSound[10].volume = 1f;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    CarSound[10].pitch = 1f + PitchMath;
                }
                else if (SoundRPM > MaxRpmTable[i])
                {
                    float Range = (MaxRpmTable[i + 1] - MaxRpmTable[i]) / RangeDivider;
                    float ReducedRPM = SoundRPM - MaxRpmTable[i];
                    CarSound[10].volume = 1f - ReducedRPM / Range;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    //CarSound[10].pitch = 1f + PitchingTable[i] + PitchMath;
                }
            }
            else if (i == 11)
            {
                //Set CarSound[11]
                if (SoundRPM < MinRpmTable[i])
                {
                    CarSound[11].volume = 0.0f;
                }
                else if (SoundRPM >= MinRpmTable[i] && SoundRPM < NormalRpmTable[i])
                {
                    float Range = NormalRpmTable[i] - MinRpmTable[i];
                    float ReducedRPM = SoundRPM - MinRpmTable[i];
                    CarSound[11].volume = ReducedRPM / Range;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    CarSound[11].pitch = 1f - PitchingTable[i] + PitchMath;
                }
                else if (SoundRPM >= NormalRpmTable[i] && SoundRPM <= MaxRpmTable[i])
                {
                    float Range = MaxRpmTable[i] - NormalRpmTable[i];
                    float ReducedRPM = SoundRPM - NormalRpmTable[i];
                    CarSound[11].volume = 1f;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    CarSound[11].pitch = 1f + PitchMath;
                }
                else if (SoundRPM > MaxRpmTable[i])
                {
                    float Range = (MaxRpmTable[i + 1] - MaxRpmTable[i]) / RangeDivider;
                    float ReducedRPM = SoundRPM - MaxRpmTable[i];
                    CarSound[11].volume = 1f - ReducedRPM / Range;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    //CarSound[11].pitch = 1f + PitchingTable[i] + PitchMath;
                }
            }
            else if (i == 12)
            {
                //Set CarSound[12]
                if (SoundRPM < MinRpmTable[i])
                {
                    CarSound[12].volume = 0.0f;
                }
                else if (SoundRPM >= MinRpmTable[i] && SoundRPM < NormalRpmTable[i])
                {
                    float Range = NormalRpmTable[i] - MinRpmTable[i];
                    float ReducedRPM = SoundRPM - MinRpmTable[i];
                    CarSound[12].volume = ReducedRPM / Range;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    CarSound[12].pitch = 1f - PitchingTable[i] + PitchMath;
                }
                else if (SoundRPM >= NormalRpmTable[i] && SoundRPM <= MaxRpmTable[i])
                {
                    float Range = MaxRpmTable[i] - NormalRpmTable[i];
                    float ReducedRPM = SoundRPM - NormalRpmTable[i];
                    CarSound[12].volume = 1f;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    CarSound[12].pitch = 1f + PitchMath;
                }
                else if (SoundRPM > MaxRpmTable[i])
                {
                    float Range = (MaxRpmTable[i + 1] - MaxRpmTable[i]) / RangeDivider;
                    float ReducedRPM = SoundRPM - MaxRpmTable[i];
                    CarSound[12].volume = 1f - ReducedRPM / Range;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    //CarSound[12].pitch = 1f + PitchingTable[i] + PitchMath;
                }
            }
            else if (i == 13)
            {
                //Set CarSound[13]
                if (SoundRPM < MinRpmTable[i])
                {
                    CarSound[13].volume = 0.0f;
                }
                else if (SoundRPM >= MinRpmTable[i] && SoundRPM < NormalRpmTable[i])
                {
                    float Range = NormalRpmTable[i] - MinRpmTable[i];
                    float ReducedRPM = SoundRPM - MinRpmTable[i];
                    CarSound[13].volume = ReducedRPM / Range;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    CarSound[13].pitch = 1f - PitchingTable[i] + PitchMath;
                }
                else if (SoundRPM >= NormalRpmTable[i] && SoundRPM <= MaxRpmTable[i])
                {
                    float Range = MaxRpmTable[i] - NormalRpmTable[i];
                    float ReducedRPM = SoundRPM - NormalRpmTable[i];
                    CarSound[13].volume = 1f;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    CarSound[13].pitch = 1f + PitchMath;
                }
                else if (SoundRPM > MaxRpmTable[i])
                {
                    float Range = (MaxRpmTable[i + 1] - MaxRpmTable[i]) / RangeDivider;
                    float ReducedRPM = SoundRPM - MaxRpmTable[i];
                    CarSound[13].volume = 1f - ReducedRPM / Range;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    //CarSound[13].pitch = 1f + PitchingTable[i] + PitchMath;
                }
            }
            else if (i == 14)
            {
                //Set CarSound[14]
                if (SoundRPM < MinRpmTable[i])
                {
                    CarSound[14].volume = 0.0f;
                }
                else if (SoundRPM >= MinRpmTable[i] && SoundRPM < NormalRpmTable[i])
                {
                    float Range = NormalRpmTable[i] - MinRpmTable[i];
                    float ReducedRPM = SoundRPM - MinRpmTable[i];
                    CarSound[14].volume = ReducedRPM / Range;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    CarSound[14].pitch = 1f - PitchingTable[i] + PitchMath;
                }
                else if (SoundRPM >= NormalRpmTable[i] && SoundRPM <= MaxRpmTable[i])
                {
                    float Range = MaxRpmTable[i] - NormalRpmTable[i];
                    float ReducedRPM = SoundRPM - NormalRpmTable[i];
                    CarSound[14].volume = 1f;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    CarSound[14].pitch = 1f + PitchMath;
                }
                else if (SoundRPM > MaxRpmTable[i])
                {
                    float Range = (MaxRpmTable[i + 1] - MaxRpmTable[i]) / RangeDivider;
                    float ReducedRPM = SoundRPM - MaxRpmTable[i];
                    CarSound[14].volume = 1f - ReducedRPM / Range;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    //CarSound[14].pitch = 1f + PitchingTable[i] + PitchMath;
                }
            }
            else if (i == 15)
            {
                //Set CarSound[15]
                if (SoundRPM < MinRpmTable[i])
                {
                    CarSound[15].volume = 0.0f;
                }
                else if (SoundRPM >= MinRpmTable[i] && SoundRPM < NormalRpmTable[i])
                {
                    float Range = NormalRpmTable[i] - MinRpmTable[i];
                    float ReducedRPM = SoundRPM - MinRpmTable[i];
                    CarSound[15].volume = ReducedRPM / Range;
                    float PitchMath = (ReducedRPM * PitchingTable[i]) / Range;
                    CarSound[15].pitch = 1f - PitchingTable[i] + PitchMath;
                }
                else if (SoundRPM >= NormalRpmTable[i] && SoundRPM <= MaxRpmTable[i])
                {
                    float Range = MaxRpmTable[i] - NormalRpmTable[i];
                    float ReducedRPM = SoundRPM - NormalRpmTable[i];
                    CarSound[15].volume = 1f;
                    float PitchMath = (ReducedRPM * (PitchingTable[i] + 0.1f)) / Range;
                    CarSound[15].pitch = 1f + PitchMath;
                }
            }
        }

    }
}



