using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController : MonoBehaviour
{
    Rigidbody rb;
    AudioSource audioSource;
    AudioSource passiveNoise;
    AudioSource activeNoise;
    ParticleSystem barrelFireParticleSystem;
    float fireCooldownDuration = 1.75f;
    float lastTimeFired = 0f;

    [Header("Objects")]
    [SerializeField] string projectilesParentName = "Projectiles";
    [SerializeField] GameObject tankHead;
    [SerializeField] GameObject tankShellPrefab;
    [SerializeField] GameObject tankShellSpawnLocation;
    [SerializeField] GameObject tankBarrel;
    [SerializeField] GameObject barrelFireParicle;
    [Header("Body Movement")]
    [HideInInspector] public float speed = 125f;
    [HideInInspector] public float turnSpeed = 125f;
    [HideInInspector] public float shellSpeed = 125f;
    [Header("Head Movement")]
    [HideInInspector] public float headTurnSpeed = 75f;
    [HideInInspector] public float barrelTurnSpeed = 45f;
    [HideInInspector] public float barrelMinAngle = -10f;
    [HideInInspector] public float barrelMaxAngle = 45f;

    [HideInInspector] public float horizontal;
    [HideInInspector] public float vertical;
    [HideInInspector] public int horizontalHat;
    [HideInInspector] public int verticalHat;
    [HideInInspector] public bool fire;

    [HideInInspector] public Vector3 velocity;
    Vector3 acceleration;

    GameObject projectilesParent;

    // Start is called before the first frame update
    void Start()
    {
        projectilesParent = GameObject.Find(projectilesParentName);
        if (projectilesParent == null)
        {
            projectilesParent = Instantiate(new GameObject());
            projectilesParent.name = projectilesParentName;
        }
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        barrelFireParticleSystem = barrelFireParicle.GetComponent<ParticleSystem>();
        barrelFireParticleSystem.Stop();
        barrelFireParicle.transform.parent = null;
        barrelFireParicle.transform.localScale = new Vector3(1, 1, 1);

        foreach (Transform child in transform)
        {
            if (child.name == "active")
            {
                activeNoise = child.GetComponent<AudioSource>();
            }
            else if (child.name == "passive")
            {
                passiveNoise = child.GetComponent<AudioSource>();
            }
        }
        
        if (passiveNoise != null)
            passiveNoise.Play();
    }

    // Update is called once per frame
    void Update()
    {
        #region MOVEMENT

        // first we rotate body if needed, than move forward, afterwards rotate head if needed

        if (horizontal != 0.0f && vertical != 0.0f)
        {   
            transform.Rotate(0, horizontal * turnSpeed * Time.deltaTime, 0);
        }
        velocity = transform.forward * vertical * speed;
        
        // active noise playing
        if (activeNoise != null && passiveNoise != null)
        {
            if (vertical != 0.0f && !activeNoise.isPlaying)
            {
                activeNoise.Play();
            }   
            else if (vertical == 0.0f && activeNoise.isPlaying)
            {
                activeNoise.Stop();
            }  
        }

        if (horizontalHat != 0)
        {
            tankHead.transform.Rotate(0, horizontalHat * headTurnSpeed * Time.deltaTime, 0);
        }
        if (verticalHat != 0)
        {
            tankBarrel.transform.Rotate(-verticalHat * barrelTurnSpeed * Time.deltaTime, 0, 0);
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

        if (fire && Time.time - lastTimeFired > fireCooldownDuration)
        {
            lastTimeFired = Time.time;
            GameObject shell = Instantiate(tankShellPrefab, tankShellSpawnLocation.transform.position, tankShellSpawnLocation.transform.rotation);
            shell.transform.SetParent(projectilesParent.transform);
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

    void OnDisable()
    {
        audioSource.Stop();
        activeNoise.Stop();
        passiveNoise.Stop();
    }
}
