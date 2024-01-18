using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitPoints : MonoBehaviour
{
    [SerializeField] public float HP = 100.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (HP <= 0.0f)
        {
            // TODO: explosion particle effects

            // disable tank controller
            GetComponent<TankController>().enabled = false;
            
            // make material partially black
            GetComponent<Renderer>().material.color = new Color(0.0f, 0.0f, 0.0f, 0.5f);

            // disable audio
            GetComponent<AudioSource>().enabled = false;

            // disable hitpoint
            enabled = false;
        }
    }
}
