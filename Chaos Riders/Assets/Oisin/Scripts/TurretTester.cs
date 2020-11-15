using UnityEngine;
using Photon.Pun;

    public class TurretTester : MonoBehaviour
    {
    TurretRotation turret;

    [HideInInspector]
    public Vector3 targetPos;
    public Transform targetTransform;

    private PhotonView pv;


    private Vector3 oriPos;
      

    private void Start()
    {
        oriPos = targetTransform.localPosition;
        pv = GetComponent<PhotonView>();
        turret = GetComponent<TurretRotation>();
    }

    private void Update()
    {
        if(!pv.IsMine && IsThisMultiplayer.Instance.multiplayer) { return; }


        // When a transform is assigned, pass that to the turret. If not,
        // just pass in whatever this is looking at.
        targetPos = transform.TransformPoint(Vector3.forward * 200.0f);

        if (targetTransform == null)
            turret.SetAimpoint(targetPos);
        else
            turret.SetAimpoint(targetTransform.position);


    }

    public void ResetPos()
    {
        targetTransform.localPosition = oriPos;
        turret.SetAimpoint(targetTransform.position);
    }


}
