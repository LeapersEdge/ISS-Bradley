using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTankController : MonoBehaviour
{

    [Header("Player Movement")]
    [SerializeField] float speed = 5f;
    [SerializeField] float turnSpeed = 75f;
    [SerializeField] float shellSpeed = 125f;
    [Header("Head Movement")]
    [SerializeField] float headTurnSpeed = 75f;
    [SerializeField] float barrelTurnSpeed = 45f;
    [SerializeField] float barrelMinAngle = -10f;
    [SerializeField] float barrelMaxAngle = 45f;
    

    public TargetLocation[] targetLocations;
    private TankController controller;
    TargetLocation targetLocation, targetLocation1, targetLocation2, targetLocation3;
    private Transform childObject;

    private int currentTargetIndex = 0;
    private float currentMovingCooldown = 100f;
    private float movingCooldown = 100f;

    void Start()
    {
        targetLocations = new TargetLocation[3];
        targetLocation1 = new TargetLocation();
        targetLocation2 = new TargetLocation();
        targetLocation3 = new TargetLocation();
        targetLocation = new TargetLocation();

        targetLocation1.location.position = new Vector3(20f, 1f, 20f);
        targetLocation2.location.position = new Vector3(30f, 1f, 30f);
        targetLocation3.location.position = new Vector3(40f, 1f, 40f);
        targetLocations[0] = targetLocation1;
        targetLocations[1] = targetLocation2;
        targetLocations[2] = targetLocation3;
        Debug.Log(targetLocations[0]);
        childObject = transform.Find("head pivot");
        controller = GetComponent<TankController>();
        controller.speed = speed;
        controller.turnSpeed = turnSpeed;
        controller.shellSpeed = shellSpeed;
        controller.headTurnSpeed = headTurnSpeed;
        controller.barrelTurnSpeed = barrelTurnSpeed;
        controller.barrelMinAngle = barrelMinAngle;
        controller.barrelMaxAngle = barrelMaxAngle;
        targetLocation = targetLocations[0];
        controller.horizontalHat = 0;
        controller.verticalHat = 0;
        controller.vertical = 0;
        controller.horizontal = 0;
        controller.fire = false;

    }

    void Update()
    {
        RotateHeadToTarget();
        MoveToTarget();
    }

    void RotateHeadToTarget()
    {
        Quaternion targetRotation = targetLocation.location.rotation;

        float angle = Quaternion.Angle(childObject.rotation, targetRotation);

        // Determine the sign of the angle using the dot product of forward vectors
        Vector3 forwardA = childObject.forward;
        Vector3 forwardB = targetRotation * Vector3.forward;
        int horizontalInput = (int)Mathf.Sign(Vector3.Dot(Vector3.up, Vector3.Cross(forwardA, forwardB)));


        if (Math.Abs(angle) > 1f)
        {
            controller.horizontalHat = horizontalInput;
        }
        else
        {
            controller.horizontalHat = 0;
        }
    }

    void MoveToTarget()
    {
        Vector3 directionToTarget = targetLocation.location.position - transform.position;
        float distanceToTarget = directionToTarget.magnitude;
        

      
        float angleToTarget = Vector3.SignedAngle(transform.forward, directionToTarget, Vector3.up);


 
        int horizontalInput = (int)Mathf.Sign(angleToTarget);


        if (Math.Abs(distanceToTarget) > 7f)
        {
            controller.vertical = 1;

            if (Math.Abs(angleToTarget) > 5f)
            {

                controller.horizontal = horizontalInput;
            }
            else
            {
                controller.horizontal = 0;
            }
        }else
        {
            //Debug.Log("ahah");
            controller.vertical = 0;
            controller.horizontal = 0;
            
            if (CanMove())
            {
                currentTargetIndex = (currentTargetIndex + 1) % targetLocations.Length;
                SetNextTarget();
            }
        }

    }

    void SetNextTarget()
    {
        targetLocation = targetLocations[currentTargetIndex];
    }

    private bool CanMove()
    {
        if(currentMovingCooldown <= 0f)
        {
            currentMovingCooldown = movingCooldown;
            return true;
        }
        else
        {

            currentMovingCooldown -= 1f;
            return false;
        }
    }

}

