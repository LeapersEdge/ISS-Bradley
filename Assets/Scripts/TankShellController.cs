using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TankShellController : MonoBehaviour
{
    ParticleSystem particleSys;

    Vector3 velocity;
    public float shellSpeed = 100f;

    void Awake()
    {
        particleSys = GetComponent<ParticleSystem>();
        particleSys.Stop();
    }

    void Start()
    {
        velocity = transform.forward * shellSpeed;
    }

    void Update()
    {
        RaycastHit hit;        
        if (Physics.Raycast(transform.position, transform.forward, out hit, shellSpeed * Time.deltaTime))
        {
            particleSys.Play();
            this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
            this.gameObject.transform.position = hit.point;
            GetComponent<Rigidbody>().useGravity = false;
            velocity = Vector3.zero;
        }

        transform.position += velocity * Time.deltaTime;
    }

    void LateUpdate()
    {
        if (velocity == Vector3.zero && !particleSys.isPlaying)
        {
            Destroy(this.gameObject);
        }
    }
}
