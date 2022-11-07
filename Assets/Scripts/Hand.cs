using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(Animator))]

public class Hand : MonoBehaviour
{

[SerializeField] float animationSpeed = 25f;
Animator animator;
SkinnedMeshRenderer mesh;
float gripTarget;
float triggerTarget;
float gripCurrent;
float triggerCurrent;
string animatorGripParam = "Grip";
string animatorTriggerParam = "Trigger";



    //Physics Movement
    [Space]
    [SerializeField] ActionBasedController controller;
    [SerializeField] float followSpeed = 1800f;
    [SerializeField] float rotateSpeed = 4000f;
    [Space]
    [SerializeField] Vector3 positionOffset;
    [SerializeField] Vector3 rotationOffset;
    [Space]
    [SerializeField] Transform palm;
    [SerializeField] float reachDistance = 0.1f, joinDistance = 0.75f;
    [SerializeField] LayerMask grabbableLayer;

    Transform followTarget;
    Rigidbody body;

    bool isGrabbing;
    GameObject heldObject;
    Transform grabPoint;
    FixedJoint joint1, joint2;

    void Start()
    {
        animator = GetComponent<Animator>();
        mesh = GetComponentInChildren<SkinnedMeshRenderer>();

        //Physics Movement
        followTarget = controller.gameObject.transform;
        body = GetComponent<Rigidbody>();
        body.collisionDetectionMode = CollisionDetectionMode.Continuous;
        body.interpolation = RigidbodyInterpolation.Interpolate;
        body.mass = 20f;
        body.maxAngularVelocity = 30f;
        
        //Inputs Setup
        controller.selectAction.action.started += Grab;
        controller.selectAction.action.canceled += Release;

        //Teleport hands
        body.position = followTarget.position;
        body.rotation = followTarget.rotation;

    }

    void Update()

    {
        AnimateHand();
        PhysicsMove();
    }

        public void SetGrip(float v)
        {
            gripTarget = v;
        }

        public void SetTrigger(float v)
        {
            triggerTarget = v;
        }

        void AnimateHand()
        {
            if(gripCurrent != gripTarget)
            {
                gripCurrent = Mathf.MoveTowards(gripCurrent, gripTarget, Time.deltaTime * animationSpeed);
                animator.SetFloat(animatorGripParam, gripCurrent);
            }
            if(triggerCurrent != triggerTarget)
            {
                triggerCurrent = Mathf.MoveTowards(triggerCurrent, triggerTarget, Time.deltaTime * animationSpeed);
                animator.SetFloat(animatorTriggerParam, triggerCurrent);
            }
        }

    void PhysicsMove()
    {
        // Position
        var positionWithOffset = followTarget.TransformPoint(positionOffset);
        var distance = Vector3.Distance(positionWithOffset, transform.position);
        body.velocity =(positionWithOffset - transform.position).normalized * (followSpeed * distance) * Time.deltaTime;

        //Rotation
        
        var rotationWithOffset = followTarget.rotation * Quaternion.Euler(rotationOffset);
        var q = rotationWithOffset * Quaternion.Inverse(transform.rotation);
        q.ToAngleAxis(out float angle, out Vector3 axis);
        if(angle > 180.0f) {angle -= 360.0f;}
        if(angle == 0) return;
        body.angularVelocity = axis * (angle * Mathf.Deg2Rad * rotateSpeed * Time.deltaTime);
        //Freeze rotation on collision
    }

    void Grab(InputAction.CallbackContext context)
    {
        if(isGrabbing || heldObject) return;

        Collider[] grabbableColliders = Physics.OverlapSphere(palm.position, reachDistance, grabbableLayer);
        {
            if(grabbableColliders.Length < 1) return;
            var objectToGrab = grabbableColliders[0].transform.gameObject;
            var objectBody = objectToGrab.GetComponent<Rigidbody>();

            if(objectBody != null)
            {
                heldObject = objectBody.gameObject;
            }
            else
            {
                objectBody = objectToGrab.GetComponentInParent<Rigidbody>();
                if(objectBody != null)
                {
                    heldObject = objectBody.gameObject;
                }
                else {return;}
            }

            StartCoroutine(GrabObject(grabbableColliders[0], objectBody));
        }

        IEnumerator GrabObject(Collider collider, Rigidbody objectBody)
        {
            isGrabbing = true;


            //Create a grab point
            grabPoint = new GameObject().transform;
            grabPoint.position = collider.ClosestPoint(palm.position);
            grabPoint.parent = heldObject.transform;
            // Move hand to grab point

            followTarget = grabPoint;

            // Wait for hand to reach grab point
            while(grabPoint != null && Vector3.Distance(grabPoint.position, palm.position) > joinDistance && isGrabbing)
            {
                yield return new WaitForEndOfFrame();
            }

            //  Freeze hand and object motion
            body.velocity = Vector3.zero;
            body.angularVelocity = Vector3.zero;
            objectBody.velocity = Vector3.zero;
            objectBody.angularVelocity = Vector3.zero;

            objectBody.collisionDetectionMode = CollisionDetectionMode.Continuous;
            objectBody.interpolation = RigidbodyInterpolation.Interpolate;

            //Attach joints
            joint1 = gameObject.AddComponent<FixedJoint>();
            joint1.connectedBody = objectBody;
            joint1.breakForce = float.PositiveInfinity;
            joint1.breakTorque = float.PositiveInfinity;

            joint1.connectedMassScale = 1;
            joint1.massScale = 1;
            joint1.enableCollision = false;
            joint1.enablePreprocessing = false;

            joint2 = heldObject.AddComponent<FixedJoint>();
            joint2.connectedBody = body;
            joint2.breakForce = float.PositiveInfinity;
            joint2.breakTorque = float.PositiveInfinity;

            joint2.connectedMassScale = 1;
            joint2.massScale = 1;
            joint2.enableCollision = false;
            joint2.enablePreprocessing = false;

            // Reset follow targets
            followTarget = controller.gameObject.transform;
        }
    }

    void Release(InputAction.CallbackContext context)
    {
        if(joint1 != null)
            Destroy(joint1);
        if(joint2 != null)
            Destroy(joint2);
        if(grabPoint != null)
            Destroy(grabPoint.gameObject);

        if(heldObject != null)
        {
            var objectBody = heldObject.GetComponent<Rigidbody>();
            objectBody.collisionDetectionMode = CollisionDetectionMode.Discrete;
            objectBody.interpolation = RigidbodyInterpolation.None;
            heldObject = null;
        }

        isGrabbing = false;
        followTarget = controller.gameObject.transform;
    }
}
