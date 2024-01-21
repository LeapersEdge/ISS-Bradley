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

    [HideInInspector] public float start_fuel = 900f;
    [HideInInspector] public float fuel;
    [HideInInspector] public float speedometer = 0f;
    float start_time = 0f;

    HitPoints hp;
    TankController tankController;

    // Start is called before the first frame update
    void Start()
    {
        tankController = GetComponent<TankController>();
        hp = GetComponent<HitPoints>();

        tankController.speed = speed;
        tankController.turnSpeed = turnSpeed;
        tankController.shellSpeed = shellSpeed;
        
        tankController.headTurnSpeed = headTurnSpeed;
        tankController.barrelTurnSpeed = barrelTurnSpeed;
        tankController.barrelMinAngle = barrelMinAngle;
        tankController.barrelMaxAngle = barrelMaxAngle;

        start_time = Time.time;
        fuel = start_fuel;
    }

    // Update is called once per frame
    void Update()
    { 
        fuel = start_fuel - (Time.time - start_time);

        if (fuel >= 0)
        {
            tankController.horizontal = Input.GetAxis("Horizontal");
            tankController.vertical = Input.GetAxis("Vertical");
            tankController.horizontalHat = (int)Input.GetAxisRaw("Horizontal Hat");
            tankController.verticalHat = (int)Input.GetAxisRaw("Vertical Hat");
            tankController.fire = Input.GetButtonDown("Fire1");
            speedometer = tankController.speed * tankController.vertical * 8.0f;
        }
        else
        {
            tankController.horizontal = 0;
            tankController.vertical = 0;
            tankController.horizontalHat = 0;
            tankController.verticalHat = 0;
            tankController.fire = false;
            speedometer = 0f;
        }

        if (hp.HP <= 0)
        {
            speedometer = 0f;
        }
    }
}
