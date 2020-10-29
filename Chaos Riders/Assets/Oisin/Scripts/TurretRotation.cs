using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


namespace Turrets
{
    public class TurretRotation : MonoBehaviour
    {
        public bool runRotationsInFixed = false;

        
        public Transform turretBase;
        
        public Transform turretBarrels;

        
        public float turnRate = 30.0f;
        
        public bool limitTraverse = false;
        
        [Range(0.0f, 180.0f)]
        public float leftTraverse = 60.0f;
        
        [Range(0.0f, 180.0f)]
        public float rightTraverse = 60.0f;
        
        [Range(0.0f, 90.0f)]
        public float elevation = 60.0f;
        
        [Range(0.0f, 90.0f)]
        public float depression = 5.0f;

        public bool showDebugRay = true;

        private Vector3 aimPoint;

        private bool aiming = false;

        private PhotonView pv;

        private void Start()
        {
            pv = GetComponent<PhotonView>();

            if (!aiming && pv.IsMine)
                aimPoint = transform.TransformPoint(Vector3.forward * 100.0f);
        }

        private void Update()
        {
            if (!runRotationsInFixed && pv.IsMine)
            {
                RotateTurret();
            }

            if (showDebugRay && pv.IsMine)
                DrawDebugRays();
        }

        private void FixedUpdate()
        {
            if (runRotationsInFixed && pv.IsMine)
            {
                RotateTurret();
            }
        }

        public void SetAimpoint(Vector3 position)
        {
            aiming = true;
            aimPoint = position;
        }

        public void SetIdle(bool idle)
        {
            aiming = !idle;
        }


        private void RotateTurret()
        {
            if (aiming)
            {
                RotateBase();
                RotateBarrels();
            }
        }

        private void RotateBase()
        {
            if (turretBase != null)
            {
                Vector3 localTargetPos = transform.InverseTransformPoint(aimPoint);
                localTargetPos.y = 0.0f;

                // Clamp target rotation by creating a limited rotation to the target.
                // Use different clamps depending if the target is to the left or right of the turret.
                Vector3 clampedLocalVec2Target = localTargetPos;
                if (limitTraverse)
                {
                    if (localTargetPos.x >= 0.0f)
                        clampedLocalVec2Target = Vector3.RotateTowards(Vector3.forward, localTargetPos, Mathf.Deg2Rad * rightTraverse, float.MaxValue);
                    else
                        clampedLocalVec2Target = Vector3.RotateTowards(Vector3.forward, localTargetPos, Mathf.Deg2Rad * leftTraverse, float.MaxValue);
                }


                Quaternion rotationGoal = Quaternion.LookRotation(clampedLocalVec2Target);
                Quaternion newRotation = Quaternion.RotateTowards(turretBase.localRotation, rotationGoal, 360);


                turretBase.localRotation = newRotation;
            }
        }

        private void RotateBarrels()
        {
            if (turretBase != null && turretBarrels != null)
            {
                Vector3 localTargetPos = turretBase.InverseTransformPoint(aimPoint);
                localTargetPos.x = 0.0f;

                // Clamp target rotation by creating a limited rotation to the target.
                // Use different clamps depending if the target is above or below the turret.
                Vector3 clampedLocalVec2Target = localTargetPos;
                if (localTargetPos.y >= 0.0f)
                    clampedLocalVec2Target = Vector3.RotateTowards(Vector3.forward, localTargetPos, Mathf.Deg2Rad * elevation, float.MaxValue);
                else
                    clampedLocalVec2Target = Vector3.RotateTowards(Vector3.forward, localTargetPos, Mathf.Deg2Rad * depression, float.MaxValue);

                // Create new rotation towards the target in local space.
                Quaternion rotationGoal = Quaternion.LookRotation(clampedLocalVec2Target);
                Quaternion newRotation = Quaternion.RotateTowards(turretBarrels.localRotation, rotationGoal, 360);

                turretBarrels.localRotation = newRotation;
            }
        }

        private void DrawDebugRays()
        {
            if (turretBarrels != null)
                Debug.DrawRay(turretBarrels.position, turretBarrels.forward * 100.0f);
            else if (turretBase != null)
                Debug.DrawRay(turretBase.position, turretBase.forward * 100.0f);
        }
    }
}