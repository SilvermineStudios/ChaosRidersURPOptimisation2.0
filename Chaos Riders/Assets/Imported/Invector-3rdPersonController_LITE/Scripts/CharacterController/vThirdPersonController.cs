using UnityEngine;
using Photon.Pun;

namespace Invector.vCharacterController
{
    public class vThirdPersonController : vThirdPersonAnimator
    {

        public virtual void ControlAnimatorRootMotion()
        {
            if (this.GetComponent<PhotonView>().IsMine) //check if the object is yours
            {
                if (!this.enabled) return;

                if (inputSmooth == Vector3.zero)
                {
                    transform.position = animator.rootPosition;
                    transform.rotation = animator.rootRotation;
                }

                if (useRootMotion)
                    MoveCharacter(moveDirection);
            }
            else return;
        }

        public virtual void ControlLocomotionType()
        {
            if (this.GetComponent<PhotonView>().IsMine) //check if the object is yours
            {
                if (lockMovement) return;

                if (locomotionType.Equals(LocomotionType.FreeWithStrafe) && !isStrafing || locomotionType.Equals(LocomotionType.OnlyFree))
                {
                    SetControllerMoveSpeed(freeSpeed);
                    SetAnimatorMoveSpeed(freeSpeed);
                }
                else if (locomotionType.Equals(LocomotionType.OnlyStrafe) || locomotionType.Equals(LocomotionType.FreeWithStrafe) && isStrafing)
                {
                    isStrafing = true;
                    SetControllerMoveSpeed(strafeSpeed);
                    SetAnimatorMoveSpeed(strafeSpeed);
                }

                if (!useRootMotion)
                    MoveCharacter(moveDirection);
            }
            else return;
        }

        public virtual void ControlRotationType()
        {
            if (this.GetComponent<PhotonView>().IsMine) //check if the object is yours
            {
                if (lockRotation) return;

                bool validInput = input != Vector3.zero || (isStrafing ? strafeSpeed.rotateWithCamera : freeSpeed.rotateWithCamera);

                if (validInput)
                {
                    // calculate input smooth
                    inputSmooth = Vector3.Lerp(inputSmooth, input, (isStrafing ? strafeSpeed.movementSmooth : freeSpeed.movementSmooth) * Time.deltaTime);

                    Vector3 dir = (isStrafing && (!isSprinting || sprintOnlyFree == false) || (freeSpeed.rotateWithCamera && input == Vector3.zero)) && rotateTarget ? rotateTarget.forward : moveDirection;
                    RotateToDirection(dir);
                }
            }
            else return;
        }

        public virtual void UpdateMoveDirection(Transform referenceTransform = null)
        {
            if (this.GetComponent<PhotonView>().IsMine) //check if the object is yours
            {
                if (input.magnitude <= 0.01)
                {
                    moveDirection = Vector3.Lerp(moveDirection, Vector3.zero, (isStrafing ? strafeSpeed.movementSmooth : freeSpeed.movementSmooth) * Time.deltaTime);
                    return;
                }

                if (referenceTransform && !rotateByWorld)
                {
                    //get the right-facing direction of the referenceTransform
                    var right = referenceTransform.right;
                    right.y = 0;
                    //get the forward direction relative to referenceTransform Right
                    var forward = Quaternion.AngleAxis(-90, Vector3.up) * right;
                    // determine the direction the player will face based on input and the referenceTransform's right and forward directions
                    moveDirection = (inputSmooth.x * right) + (inputSmooth.z * forward);
                }
                else
                {
                    moveDirection = new Vector3(inputSmooth.x, 0, inputSmooth.z);
                }
            }
            else return; 
        }

        public virtual void Sprint(bool value)
        {
            if (this.GetComponent<PhotonView>().IsMine) //check if the object is yours
            {
                var sprintConditions = (input.sqrMagnitude > 0.1f && isGrounded &&
                !(isStrafing && !strafeSpeed.walkByDefault && (horizontalSpeed >= 0.5 || horizontalSpeed <= -0.5 || verticalSpeed <= 0.1f)));

                if (value && sprintConditions)
                {
                    if (input.sqrMagnitude > 0.1f)
                    {
                        if (isGrounded && useContinuousSprint)
                        {
                            isSprinting = !isSprinting;
                        }
                        else if (!isSprinting)
                        {
                            isSprinting = true;
                        }
                    }
                    else if (!useContinuousSprint && isSprinting)
                    {
                        isSprinting = false;
                    }
                }
                else if (isSprinting)
                {
                    isSprinting = false;
                }
            }
            else return;        }

        public virtual void Strafe()
        {
            if (this.GetComponent<PhotonView>().IsMine) //check if the object is yours
            {
                isStrafing = !isStrafing;
            }
            else return;
        }

        public virtual void Jump()
        {
            if (this.GetComponent<PhotonView>().IsMine) //check if the object is yours
            {
                // trigger jump behaviour
                jumpCounter = jumpTimer;
                isJumping = true;

                // trigger jump animations
                if (input.sqrMagnitude < 0.1f)
                    animator.CrossFadeInFixedTime("Jump", 0.1f);
                else
                    animator.CrossFadeInFixedTime("JumpMove", .2f);
            }
            else return;
        }
   
    }
}