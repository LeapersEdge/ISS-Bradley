using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Movement")]
    [SerializeField] float speed = 125f;
    [SerializeField] float turnSpeed = 125f;
    [SerializeField] float shellSpeed = 125f;
    [Header("Head Movement")]
    [SerializeField] float headTurnSpeed = 75f;
    [SerializeField] float barrelTurnSpeed = 45f;
    [SerializeField] float barrelMinAngle = -10f;
    [SerializeField] float barrelMaxAngle = 45f;

    TankController tankController;

    // Start is called before the first frame update
    void Start()
    {
        tankController = GetComponent<TankController>();

        tankController.speed = speed;
        tankController.turnSpeed = turnSpeed;
        tankController.shellSpeed = shellSpeed;
        
        tankController.headTurnSpeed = headTurnSpeed;
        tankController.barrelTurnSpeed = barrelTurnSpeed;
        tankController.barrelMinAngle = barrelMinAngle;
        tankController.barrelMaxAngle = barrelMaxAngle;
    }

    // Update is called once per frame
    void Update()
    { 
        tankController.horizontal = (int)Input.GetAxisRaw("Horizontal");
        tankController.vertical = (int)Input.GetAxisRaw("Vertical");
        tankController.horizontalHat = (int)Input.GetAxisRaw("Horizontal Hat");
        tankController.verticalHat = (int)Input.GetAxisRaw("Vertical Hat");
        tankController.fire = Input.GetButtonDown("Fire1");
    }
}
