using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.VFX;
using Photon.Pun;

public class DustKickVFXScript : MonoBehaviour
{
    PhotonView pv;
    [SerializeField] VisualEffect BackLeftDustKickUp, BackRightDustKickUp, FrontLeftDustKickUp, FrontRightDustKickUp;
    [SerializeField] WheelCollider backLeft, backRight, frontLeft, frontRight;
    int FrontSpawnRate, BackSpawnRate;
    float CurrentSpeed, LerpPercentage;
    [SerializeField] float MaxSpeed;
    Vector3 FrontMinVel, FrontMaxVel, BackMinVel, BackMaxVel, TestFrontMinVel, TestFrontMaxVel, TestBackMinVel, TestBackMaxVel;
    // Start is called before the first frame update
    [SerializeField] bool isAI;
    Controller cont;
    AICarController aiCont;

    void Start()
    {
        if(isAI)
        {
            MaxSpeed = 2000;
        }
        else
        {
            MaxSpeed = 170;
        }
        pv = GetComponent<PhotonView>();
        cont = GetComponent<Controller>();
        aiCont = GetComponent<AICarController>();
        //get original components (front particle use same number and baack particles use same, meaning back and front have diff values)
        //pv.RPC("Init",RpcTarget.All);
        Init();

        InvokeRepeating("Calculate", 0, 0.5f);
    }


    private void Init()
    {
        BackSpawnRate = BackLeftDustKickUp.GetInt("Spawn Rate");
        FrontSpawnRate = FrontLeftDustKickUp.GetInt("Spawn Rate");

        FrontMinVel = FrontLeftDustKickUp.GetVector3("MinVelocity");
        FrontMaxVel = FrontLeftDustKickUp.GetVector3("MaxVelocity");

        BackMinVel = FrontLeftDustKickUp.GetVector3("MinVelocity");
        BackMaxVel = FrontLeftDustKickUp.GetVector3("MaxVelocity");
    }


    private void Update()
    {
        if (!isAI)
        {
            CurrentSpeed = cont.currentSpeed; //gets car speed
        }
        else
        {
            CurrentSpeed = aiCont.currentSpeed;
        }


        if(!backLeft.isGrounded || !backRight.isGrounded || !frontRight.isGrounded || !frontLeft.isGrounded)
        {
            pv.RPC("SendOut", RpcTarget.All);
        }
    }
    private void Calculate()
    {
        if (pv.IsMine)
        {
            LerpPercentage = CurrentSpeed / MaxSpeed; // gets lerp number
            
            //Debug.Log("LerpPercentage = " + LerpPercentage);
            //Debug.Log("Current speeed = " + CurrentSpeed);

            //Calculates the value after lerping BACK
            TestBackMinVel.x = Mathf.Lerp(0, BackMinVel.x, LerpPercentage);
            TestBackMinVel.y = Mathf.Lerp(0, BackMinVel.y, LerpPercentage);
            TestBackMinVel.z = Mathf.Lerp(0, BackMinVel.z, LerpPercentage);

            TestBackMaxVel.x = Mathf.Lerp(0, BackMaxVel.x, LerpPercentage);
            TestBackMaxVel.y = Mathf.Lerp(0, BackMaxVel.y, LerpPercentage);
            TestBackMaxVel.z = Mathf.Lerp(0, BackMaxVel.z, LerpPercentage);

            //Calculates the value after lerping FRONT
            TestFrontMinVel.x = Mathf.Lerp(0, FrontMinVel.x, LerpPercentage);
            TestFrontMinVel.y = Mathf.Lerp(0, FrontMinVel.y, LerpPercentage);
            TestFrontMinVel.z = Mathf.Lerp(0, FrontMinVel.z, LerpPercentage);

            TestFrontMaxVel.x = Mathf.Lerp(0, FrontMaxVel.x, LerpPercentage);
            TestFrontMaxVel.y = Mathf.Lerp(0, FrontMaxVel.y, LerpPercentage);
            TestFrontMaxVel.z = Mathf.Lerp(0, FrontMaxVel.z, LerpPercentage);

            pv.RPC("SendOut", RpcTarget.All);
        }

    }


    [PunRPC]
    void SendOut()
    {
        //edits the spawn rates for the vfx //assigns the lerped vector3 to VFX
        if (backLeft.isGrounded)
        {
            BackLeftDustKickUp.SetInt("Spawn Rate", Mathf.RoundToInt(Mathf.Lerp(0, BackSpawnRate, LerpPercentage)));
            BackLeftDustKickUp.SetVector3("MinVelocity", TestBackMinVel);
            BackLeftDustKickUp.SetVector3("MaxVelocity", TestBackMaxVel);
        }
        else
        {
            BackLeftDustKickUp.SetInt("Spawn Rate",0);
            BackLeftDustKickUp.SetVector3("MinVelocity", Vector3.zero);
            BackLeftDustKickUp.SetVector3("MaxVelocity", Vector3.zero);
        }


        if (backRight.isGrounded)
        {
            BackRightDustKickUp.SetInt("Spawn Rate", Mathf.RoundToInt(Mathf.Lerp(0, BackSpawnRate, LerpPercentage)));
            BackRightDustKickUp.SetVector3("MinVelocity", TestBackMinVel);
            BackRightDustKickUp.SetVector3("MaxVelocity", TestBackMaxVel);
        }
        else
        {
            BackRightDustKickUp.SetInt("Spawn Rate", 0);
            BackRightDustKickUp.SetVector3("MinVelocity", Vector3.zero);
            BackRightDustKickUp.SetVector3("MaxVelocity", Vector3.zero);
        }


        if (frontLeft.isGrounded)
        {
            FrontLeftDustKickUp.SetInt("Spawn Rate", Mathf.RoundToInt(Mathf.Lerp(0, FrontSpawnRate, LerpPercentage)));
            FrontLeftDustKickUp.SetVector3("MinVelocity", TestFrontMinVel);
            FrontLeftDustKickUp.SetVector3("MaxVelocity", TestFrontMaxVel);
        }
        else
        {
            FrontLeftDustKickUp.SetInt("Spawn Rate", 0);
            FrontLeftDustKickUp.SetVector3("MinVelocity", Vector3.zero);
            FrontLeftDustKickUp.SetVector3("MaxVelocity", Vector3.zero);
        }


        if (frontRight.isGrounded)
        {
            FrontRightDustKickUp.SetInt("Spawn Rate", Mathf.RoundToInt(Mathf.Lerp(0, FrontSpawnRate, LerpPercentage)));
            FrontRightDustKickUp.SetVector3("MinVelocity", TestFrontMinVel);
            FrontRightDustKickUp.SetVector3("MaxVelocity", TestFrontMaxVel);
        }
        else
        {
            FrontRightDustKickUp.SetInt("Spawn Rate", 0);
            FrontRightDustKickUp.SetVector3("MinVelocity", Vector3.zero);
            FrontRightDustKickUp.SetVector3("MaxVelocity", Vector3.zero);
        }





    }

}
