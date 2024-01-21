using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class EnemyTankController : MonoBehaviour
{
    [Header("Player Tank")]
    public GameObject playerTank;
    public GameObject playerTankHead;
    [Header("Player Movement")]
    [SerializeField] float speed = 5f;
    [SerializeField] float turnSpeed = 75f;
    [SerializeField] float shellSpeed = 125f;
    [Header("Head Movement")]
    [SerializeField] float headTurnSpeed = 75f;
    [SerializeField] float barrelTurnSpeed = 45f;
    [SerializeField] float barrelMinAngle = -10f;
    [SerializeField] float barrelMaxAngle = 45f;
    [SerializeField] bool move_head_vertically = false;
    [Header("Tank Parts")]
    [SerializeField] Transform head_pivot;
    [SerializeField] Transform vertical_head;


    public TargetLocation[] targetLocations;
    private TankController controller;

    private int currentTargetIndex = 0;
    bool waiting = false;

    void Start()
    {
        controller = GetComponent<TankController>();
        controller.speed = speed;
        controller.turnSpeed = turnSpeed;
        controller.shellSpeed = shellSpeed;
        controller.headTurnSpeed = headTurnSpeed;
        controller.barrelTurnSpeed = barrelTurnSpeed;
        controller.barrelMinAngle = barrelMinAngle;
        controller.barrelMaxAngle = barrelMaxAngle;
        controller.horizontalHat = 0;
        controller.verticalHat = 0;
        controller.vertical = 0;
        controller.horizontal = 0;
        controller.fire = false;
    }

    void Update()
    {
        if (IsPlayerVisible())
        {
            RotateHeadToTarget();
        }
        else
        {
            RotateHeadByData();
        }
        
        if (!waiting)
            MoveToTarget();
    }

    void RotateHeadToTarget()
    {
        Vector3 directionToTarget = playerTankHead.transform.position - head_pivot.transform.position;
        Quaternion horizontalRotation = Quaternion.LookRotation(directionToTarget);
        float horizontalRotationY = horizontalRotation.eulerAngles.y;
        float deltaAngleY = Mathf.DeltaAngle(head_pivot.transform.rotation.eulerAngles.y, horizontalRotationY);
        int horizontalInput = (int)Mathf.Sign(deltaAngleY);


        Vector3 directionToTarget2 = playerTank.transform.position - vertical_head.transform.position;
        Quaternion verticalRotation = Quaternion.LookRotation(directionToTarget2);
        float verticalRotationX = verticalRotation.eulerAngles.x;
        float deltaAngleX = Mathf.DeltaAngle(verticalRotationX, vertical_head.transform.rotation.eulerAngles.x);
        int verticalInput = (int)Mathf.Sign(deltaAngleX);


        if (Math.Abs(deltaAngleY) > 1f)
        {
            controller.horizontalHat = horizontalInput;
            if (Math.Abs(deltaAngleY) <= (targetLocations[currentTargetIndex].aimingAccuracyPercentageY+1) * Math.Abs(deltaAngleY))
            {
                controller.fire = true;
            }
        }
        else
        {
            controller.horizontalHat = 0;
            controller.fire = true;
        }

        if (move_head_vertically)
        {
            if (Math.Abs(deltaAngleX) > 1f)
            {
                controller.verticalHat = verticalInput;
                if (Math.Abs(deltaAngleX) <= (targetLocations[currentTargetIndex].aimingAccuracyPercentageX+1) * Math.Abs(deltaAngleX))
                {
                    controller.fire = true;
                }
            }
            else
            {
                controller.verticalHat = 0;
                controller.fire = true;
            }
        }
    }

    void RotateHeadByData()
    {   
        Quaternion targetRotation = targetLocations[currentTargetIndex].location.rotation;
        Quaternion verticalHeadRotation = vertical_head.transform.rotation;
        float angleDiffX = Mathf.DeltaAngle(targetRotation.eulerAngles.x, verticalHeadRotation.eulerAngles.x);
        int sign = (int)Mathf.Sign(angleDiffX);

        Quaternion horizontalHeadRotation = head_pivot.transform.rotation;
        float angleDiffY = Mathf.DeltaAngle(targetRotation.eulerAngles.y, horizontalHeadRotation.eulerAngles.y);
        int sign2 = (int)Mathf.Sign(angleDiffY);



        if(Math.Abs(angleDiffX) > 5f)
        {
            controller.verticalHat = sign;
        }
        else
        {
            controller.verticalHat = 0;
        }

        if(Math.Abs(angleDiffY) > 5f)
        {
            controller.horizontalHat = sign2;
        }
        else
        {
            controller.horizontalHat = 0;
        }
    }

    void MoveToTarget()
    {
        Vector3 directionToTarget = targetLocations[currentTargetIndex].location.position - transform.position;
        float distanceToTarget = directionToTarget.magnitude;
        float angleToTarget = Vector3.SignedAngle(transform.forward, directionToTarget, Vector3.up);

        if (Math.Abs(distanceToTarget) > 1f)
        {
            controller.vertical = 1;
            if (Math.Abs(angleToTarget) > 1f)
            {
                int horizontalInput = (int)Mathf.Sign(angleToTarget);
                controller.horizontal = horizontalInput;
            }
            else
            {
                controller.horizontal = 0;
            }
        }
        else
        {
            controller.vertical = 0;
            controller.horizontal = 0;
            waiting = true;
            currentTargetIndex = (currentTargetIndex + 1) % targetLocations.Length;
            StartCoroutine(WaitForNextMove());
        }
    }

    bool IsPlayerVisible()
    {
        Vector3 directionToPlayer = playerTank.transform.position - head_pivot.transform.position;

        Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
        float angleToPlayer = Quaternion.Angle(head_pivot.transform.rotation, lookRotation);

        Debug.Log(angleToPlayer);
        if (angleToPlayer <= 45f)
        {
            return true;
        }

        return false;
    }

    IEnumerator WaitForNextMove()
    {
        yield return new WaitForSeconds(targetLocations[currentTargetIndex].cooldownAfterAction);
        waiting = false;
    }
}

