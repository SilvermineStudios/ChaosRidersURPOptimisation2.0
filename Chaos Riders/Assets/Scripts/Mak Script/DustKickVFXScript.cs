using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.VFX;
public class DustKickVFXScript : MonoBehaviour
{
    [SerializeField] VisualEffect BackLeftDustKickUp, BackRightDustKickUp, FrontLeftDustKickUp, FrontRightDustKickUp;
    int FrontSpawnRate, BackSpawnRate;
    float CurrentSpeed, LerpPercentage, MaxSpeed = 170;
    Vector3 FrontMinVel, FrontMaxVel, BackMinVel, BackMaxVel, TestFrontMinVel, TestFrontMaxVel, TestBackMinVel, TestBackMaxVel;
    // Start is called before the first frame update
    void Start()
    {
        BackSpawnRate = BackLeftDustKickUp.GetInt("Spawn Rate");
        FrontSpawnRate = FrontLeftDustKickUp.GetInt("Spawn Rate");
        FrontMinVel = FrontLeftDustKickUp.GetVector3("MinVelocity");
        FrontMaxVel = FrontLeftDustKickUp.GetVector3("MaxVelocity");
        BackMinVel = FrontLeftDustKickUp.GetVector3("MinVelocity");
        BackMaxVel = FrontLeftDustKickUp.GetVector3("MaxVelocity");
        CurrentSpeed = GetComponent<Controller>().currentSpeed;
    }

    private void FixedUpdate()
    {
        LerpPercentage = CurrentSpeed / MaxSpeed;

        BackLeftDustKickUp.SetInt("Spawn Rate", Mathf.RoundToInt(Mathf.Lerp(0, BackSpawnRate, LerpPercentage)));
        BackRightDustKickUp.SetInt("Spawn Rate", Mathf.RoundToInt(Mathf.Lerp(0, BackSpawnRate, LerpPercentage)));
        FrontLeftDustKickUp.SetInt("Spawn Rate", Mathf.RoundToInt(Mathf.Lerp(0, FrontSpawnRate, LerpPercentage)));
        FrontRightDustKickUp.SetInt("Spawn Rate", Mathf.RoundToInt(Mathf.Lerp(0, FrontSpawnRate, LerpPercentage)));
        ////
        TestBackMinVel.x = Mathf.Lerp(0, BackLeftDustKickUp.GetVector3("MinVelocity").x, LerpPercentage);
        TestBackMinVel.y = Mathf.Lerp(0, BackLeftDustKickUp.GetVector3("MinVelocity").y, LerpPercentage);
        TestBackMinVel.z = Mathf.Lerp(0, BackLeftDustKickUp.GetVector3("MinVelocity").z, LerpPercentage);
        TestBackMaxVel.x = Mathf.Lerp(0, BackLeftDustKickUp.GetVector3("MaxVelocity").x, LerpPercentage);
        TestBackMaxVel.y = Mathf.Lerp(0, BackLeftDustKickUp.GetVector3("MaxVelocity").y, LerpPercentage);
        TestBackMaxVel.z = Mathf.Lerp(0, BackLeftDustKickUp.GetVector3("MaxVelocity").z, LerpPercentage);

        TestBackMinVel.x = Mathf.Lerp(0, BackLeftDustKickUp.GetVector3("MinVelocity").x, LerpPercentage);
        TestBackMinVel.y = Mathf.Lerp(0, BackLeftDustKickUp.GetVector3("MinVelocity").y, LerpPercentage);
        TestBackMinVel.z = Mathf.Lerp(0, BackLeftDustKickUp.GetVector3("MinVelocity").z, LerpPercentage);
        TestBackMaxVel.x = Mathf.Lerp(0, BackLeftDustKickUp.GetVector3("MaxVelocity").x, LerpPercentage);
        TestBackMaxVel.y = Mathf.Lerp(0, BackLeftDustKickUp.GetVector3("MaxVelocity").y, LerpPercentage);
        TestBackMaxVel.z = Mathf.Lerp(0, BackLeftDustKickUp.GetVector3("MaxVelocity").z, LerpPercentage);
        /////

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
