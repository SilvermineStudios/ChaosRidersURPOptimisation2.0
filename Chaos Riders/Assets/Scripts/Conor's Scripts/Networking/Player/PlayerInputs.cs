using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerInputs : MonoBehaviourPun, IPunObservable
{
    [HideInInspector]
    public InputStr Input;
    public struct InputStr
    {
        public float LookX;
        public float LookZ;
        public float DriveX; //in controller GetInput() <-------------------------------------------------------------------
        public float DriveZ; //in controller GetInput() <-------------------------------------------------------------------
        public bool Jump;
    }

    public const float Speed = 10f;
    public const float Jumpforce = 5f;

    protected Rigidbody Rigidbody;
    protected Animator Animator;
    protected Quaternion LookRotation;

    protected bool Grounded;

    void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
        Animator = GetComponent<Animator>();

        //destroys the controller if it does not belong to you
        if(!photonView.IsMine && GetComponent<Controller>() != null)
        {
            Destroy(GetComponent<Controller>());
        }
    }

    void Update()
    {
        //Animator.SetBool("Grounded", Grounded);

        //var localVelocity = Quaternion.Inverse(transform.rotation) * (Rigidbody.velocity / Speed);
        //Animator.SetFloat("RunX", localVelocity.x);
        //Animator.SetFloat("RunZ", localVelocity.z);
    }

    void FixedUpdate()
    {
        //position
        var inputDrive = Vector3.ClampMagnitude(new Vector3(Input.DriveX, Input.DriveZ), 1);
        var inputLook = Vector3.ClampMagnitude(new Vector3(Input.LookX, 0, Input.LookZ), 1);

        /*
        Rigidbody.velocity = new Vector3(inputDrive.x * Speed, Rigidbody.velocity.y, inputDrive.z * Speed);

        //rotation
        if(inputLook.magnitude > 0.01f)
            LookRotation = Quaternion.AngleAxis(Vector3.SignedAngle(Vector3.forward, inputLook, Vector3.up), Vector3.up);

        transform.rotation = LookRotation;

        //jump
        if(Input.Jump)
        {
            var grounded = Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, 0.2f, 1);
            if(grounded)
            {
                Rigidbody.velocity = new Vector3(Rigidbody.velocity.x, Jumpforce, Rigidbody.velocity.z);  
            }
        }
        */
    }

    /*
    public static void RefreshInstance(ref PlayerInputs player, PlayerInputs Prefab)
    {
        var position = Vector3.zero;
        var rotation = Quaternion.identity;

        if(player != null)
        {
            position = player.transform.position;
            rotation = player.transform.rotation;
            PhotonNetwork.Destroy(player.gameObject);
        }

        player = PhotonNetwork.Instantiate(Prefab.gameObject.name, position, rotation).GetComponent<PlayerInputs>();
    }
    */

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(Input.DriveX);
            stream.SendNext(Input.DriveZ);
            stream.SendNext(Input.LookX);
            stream.SendNext(Input.LookZ);
        }
        else
        {
            Input.DriveX = (float)stream.ReceiveNext();
            Input.DriveZ = (float)stream.ReceiveNext();
            Input.LookX = (float)stream.ReceiveNext();
            Input.LookZ = (float)stream.ReceiveNext();
        }
    }
}
