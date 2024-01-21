using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTankController : MonoBehaviour
{

    [Header("Enemey Tank Movement")]
    [SerializeField] float speed = 5f;
    [SerializeField] float turnSpeed = 75f;
    [SerializeField] float shellSpeed = 125f;
    [SerializeField] float accuracyMaxDistanceMiss = 10f;
    [Header("Head Movement")]
    [SerializeField] float headTurnSpeed = 75f;
    [SerializeField] float barrelTurnSpeed = 45f;
    [SerializeField] float barrelMinAngle = -10f;
    [SerializeField] float barrelMaxAngle = 45f;
    [SerializeField] bool move_head_vertically = false;
    [Header("Tank Parts")]
    [SerializeField] Transform head_pivot;

    public TargetLocation[] targetLocations;
    private TankController controller;

    private int currentTargetIndex = 0;
    bool waiting = false;
    bool player_in_sight = false;
    Vector3 player_sighted_at;

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
        ScanForPlayer();
        if (!player_in_sight)
            RotateHeadToTarget();
        else
            LockOntoPlayer();
        if (!waiting)
            MoveToTarget();
    }

    void ScanForPlayer()
    {
        // rayscan with FOV od 50 degrees horizontally and verically and range of 100 meters to check if player is in sight
        RaycastHit hit;
        for (int i = -25; i < 25; i++)
        {
            for (int j = -25; j < 25; j++)
            {
                if (Physics.Raycast(head_pivot.position, Quaternion.AngleAxis(i, Vector3.up) * Quaternion.AngleAxis(j, Vector3.right) * head_pivot.forward, out hit, 100.0f))
                {
                    if (hit.collider.gameObject.tag == "Player")
                    {
                        player_in_sight = true;
                        player_sighted_at = hit.collider.gameObject.transform.position;
                        // ajust player_sighted_at to compensate for accuracyX and accuracyY in current targetLocation
                        player_sighted_at.x += (1.0f - targetLocations[currentTargetIndex].aimingAccuracyPercentageX) * UnityEngine.Random.Range(-accuracyMaxDistanceMiss, accuracyMaxDistanceMiss);
                        player_sighted_at.y += (1.0f - targetLocations[currentTargetIndex].aimingAccuracyPercentageY) * UnityEngine.Random.Range(-accuracyMaxDistanceMiss, accuracyMaxDistanceMiss);
                        return;
                    }
                }
            }
        }
    }

    void LockOntoPlayer()
    {
        Vector3 directionToTarget = player_sighted_at - transform.position;
        float distanceToTarget = directionToTarget.magnitude;
        float angleToTarget = Vector3.SignedAngle(transform.forward, directionToTarget, Vector3.up);

        if (Math.Abs(distanceToTarget) > 7f)
        {
            controller.vertical = 1;
            if (Math.Abs(angleToTarget) > 5f)
            {
                int horizontalInput = (int)Mathf.Sign(angleToTarget);            
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
            controller.fire = true;
        }        
    }

    void RotateHeadToTarget()
    {
        Quaternion targetRotation = targetLocations[currentTargetIndex].location.rotation;
        float angle = Quaternion.Angle(head_pivot.rotation, targetRotation);

        // Determine the sign of the angle using the dot product of forward vectors
        Vector3 forwardA = head_pivot.forward;
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
        Vector3 directionToTarget = targetLocations[currentTargetIndex].location.position - transform.position;
        float distanceToTarget = directionToTarget.magnitude;
        float angleToTarget = Vector3.SignedAngle(transform.forward, directionToTarget, Vector3.up);

        if (Math.Abs(distanceToTarget) > 7f)
        {
            controller.vertical = 1;
            if (Math.Abs(angleToTarget) > 5f)
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

    IEnumerator WaitForNextMove()
    {
        yield return new WaitForSeconds(targetLocations[currentTargetIndex].cooldownAfterAction);
        waiting = false;
    }
}

