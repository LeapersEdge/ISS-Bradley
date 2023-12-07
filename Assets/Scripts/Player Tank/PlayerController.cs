using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;
    AudioSource audioSource;
    ParticleSystem barrelFireParticleSystem;

    [Header("Objects")]
    [SerializeField] GameObject tankHead;
    [SerializeField] GameObject tankShellPrefab;
    [SerializeField] GameObject tankShellSpawnLocation;
    [SerializeField] GameObject tankBarrel;
    [SerializeField] GameObject barrelFireParicle;
    [Header("Player Movement")]
    [SerializeField] float speed = 125f;
    [SerializeField] float turnSpeed = 125f;
    [SerializeField] float shellSpeed = 125f;
    [Header("Head Movement")]
    [SerializeField] float headTurnSpeed = 75f;
    [SerializeField] float barrelTurnSpeed = 45f;
    [SerializeField] float barrelMinAngle = -10f;
    [SerializeField] float barrelMaxAngle = 45f;

    Vector3 velocity;
    Vector3 acceleration;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        barrelFireParticleSystem = barrelFireParicle.GetComponent<ParticleSystem>();
        barrelFireParticleSystem.Stop();
        barrelFireParicle.transform.parent = null;
        barrelFireParicle.transform.localScale = new Vector3(1, 1, 1);
    }

    // Update is called once per frame
    void Update()
    {
    #region MOVEMENT
        
        // first we rotate body if needed, than move forward, afterwards rotate head if needed

        if (Input.GetButton("Horizontal") && Input.GetButton("Vertical"))
        {   
            transform.Rotate(0, Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime, 0);
        }
        velocity = transform.forward * Input.GetAxis("Vertical") * speed;
        
        if (Input.GetButton("Horizontal Hat"))
        {
            tankHead.transform.Rotate(0, Input.GetAxis("Horizontal Hat") * headTurnSpeed * Time.deltaTime, 0);
        }
        if (Input.GetButton("Vertical Hat"))
        {
            tankBarrel.transform.Rotate(-Input.GetAxis("Vertical Hat") * barrelTurnSpeed * Time.deltaTime, 0, 0);
            if (tankBarrel.transform.rotation.eulerAngles.x + barrelMinAngle > 0 && tankBarrel.transform.rotation.eulerAngles.x < 180)
            {
                tankBarrel.transform.rotation = Quaternion.Euler(-barrelMinAngle, tankBarrel.transform.rotation.eulerAngles.y, tankBarrel.transform.rotation.eulerAngles.z);
            }
            else if (tankBarrel.transform.rotation.eulerAngles.x < 360 - barrelMaxAngle && tankBarrel.transform.rotation.eulerAngles.x > 180)
            {
                tankBarrel.transform.rotation = Quaternion.Euler(360 - barrelMaxAngle, tankBarrel.transform.rotation.eulerAngles.y, tankBarrel.transform.rotation.eulerAngles.z);
            }
        }
    
    #endregion

        if (Input.GetButtonDown("Fire1"))
        {
            GameObject shell = Instantiate(tankShellPrefab, tankShellSpawnLocation.transform.position, tankShellSpawnLocation.transform.rotation);
            shell.GetComponent<TankShellController>().shellSpeed = shellSpeed;
            audioSource.Play();
            barrelFireParicle.transform.position = tankShellSpawnLocation.transform.position;
            barrelFireParicle.transform.rotation = tankShellSpawnLocation.transform.rotation;
            barrelFireParticleSystem.Play();
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(transform.position + velocity * Time.fixedDeltaTime);
    }
}
