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
    int FrontSpawnRate, BackSpawnRate;
    float CurrentSpeed, LerpPercentage;
    [SerializeField] float MaxSpeed = 100;
    Vector3 FrontMinVel, FrontMaxVel, BackMinVel, BackMaxVel, TestFrontMinVel, TestFrontMaxVel, TestBackMinVel, TestBackMaxVel;
    // Start is called before the first frame update
    void Start()
    {
        pv = GetComponent<PhotonView>();
        //get original components (front particle use same number and baack particles use same, meaning back and front have diff values)
        //pv.RPC("Init",RpcTarget.All);
        Init();
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
        CurrentSpeed = GetComponent<Controller>().currentSpeed; //gets car speed
    }
    private void FixedUpdate()
    {
        if (pv.IsMine)
        {
            LerpPercentage = CurrentSpeed / MaxSpeed; // gets lerp number
            Debug.Log("LerpPercentage = " + LerpPercentage);
            Debug.Log("Current speeed = " + CurrentSpeed);

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
        //edits the spawn rates for the vfx
        BackLeftDustKickUp.SetInt("Spawn Rate", Mathf.RoundToInt(Mathf.Lerp(0, BackSpawnRate, LerpPercentage)));
        BackRightDustKickUp.SetInt("Spawn Rate", Mathf.RoundToInt(Mathf.Lerp(0, BackSpawnRate, LerpPercentage)));

        FrontLeftDustKickUp.SetInt("Spawn Rate", Mathf.RoundToInt(Mathf.Lerp(0, FrontSpawnRate, LerpPercentage)));
        FrontRightDustKickUp.SetInt("Spawn Rate", Mathf.RoundToInt(Mathf.Lerp(0, FrontSpawnRate, LerpPercentage)));

        //assigns the lerped vector3 to VFX
        BackLeftDustKickUp.SetVector3("MinVelocity", TestBackMinVel);
        BackLeftDustKickUp.SetVector3("MaxVelocity", TestBackMaxVel);

        BackRightDustKickUp.SetVector3("MinVelocity", TestBackMinVel);
        BackRightDustKickUp.SetVector3("MaxVelocity", TestBackMaxVel);

        FrontLeftDustKickUp.SetVector3("MinVelocity", TestFrontMinVel);
        FrontLeftDustKickUp.SetVector3("MaxVelocity", TestFrontMaxVel);

        FrontRightDustKickUp.SetVector3("MinVelocity", TestFrontMinVel);
        FrontRightDustKickUp.SetVector3("MaxVelocity", TestFrontMaxVel);
    }

}
