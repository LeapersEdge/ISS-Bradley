using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TankShellController : MonoBehaviour
{
    ParticleSystem particleSys;
    AudioSource audioSource;

    Vector3 velocity;
    public float shellSpeed = 100f;

    void Awake()
    {
        particleSys = GetComponent<ParticleSystem>();
        audioSource = GetComponent<AudioSource>();
        particleSys.Stop();
        audioSource.Stop();
    }

    void Start()
    {
        velocity = transform.forward * shellSpeed;
    }

    void Update()
    {
        RaycastHit hit;        
        if (Physics.Raycast(transform.position, transform.forward, out hit, shellSpeed * Time.deltaTime) && velocity != Vector3.zero)
        {
            velocity = Vector3.zero;

            particleSys.Play();
            audioSource.Play();
            this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
            this.gameObject.transform.position = hit.point;
            GetComponent<Rigidbody>().useGravity = false;

            // check if player is within trigger collider that sits on this object
            Collider[] colliders = Physics.OverlapSphere(transform.position, 12.0f);    
            foreach (Collider collider in colliders)
            {
                // check if any of the collider parents have tank tag and if it does, deal damage to it and add it to list so that we know not to stack damage to it again
                //
                // list of tanks that have already been damaged by this explosion
                List<GameObject> damagedTanks = new List<GameObject>();
                // check if any of the parents (recursivly up) of the collider have tank tag
                damagedTanks = RecursiveParentDigging(damagedTanks, collider.gameObject.transform.parent);

                foreach (GameObject tank in damagedTanks)
                {
                    // damage player so that the center deals 125 damage and outer radius deals 0 damage (12.0f number is just the radius of the explosion)
                    float damage = 125.0f - 125.0f * (tank.transform.position - transform.position).magnitude / 12.0f;
                    tank.GetComponent<HitPoints>().HP -= damage;
                }
            }
        }

        transform.position += velocity * Time.deltaTime;
    }

    List<GameObject> RecursiveParentDigging(List<GameObject> recursiveList, Transform obj)
    {
        if (obj == null)
        {
            return recursiveList;
        }
        else
        {
            if (!recursiveList.Contains(obj.gameObject) && (obj.tag == "Tank" || obj.tag == "Player"))
                recursiveList.Add(obj.gameObject);
            return RecursiveParentDigging(recursiveList, obj.parent);
        }
    }

    void LateUpdate()
    {
        if (velocity == Vector3.zero && !particleSys.isPlaying)
        {
            Destroy(this.gameObject);
        }
    }
}
