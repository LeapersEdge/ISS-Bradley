using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TankShellController : MonoBehaviour
{
    Vector3 velocity;
    public float shellSpeed = 100f;

    void Start()
    {
        velocity = transform.right * shellSpeed;
    }

    void Update()
    {
        RaycastHit hit;        
        if (Physics.Raycast(transform.position, transform.right, out hit, shellSpeed * Time.deltaTime))
        {
            Destroy(this.gameObject);
        }

        transform.position += velocity * Time.deltaTime;
    }
}
