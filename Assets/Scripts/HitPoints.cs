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
            if (GetComponent<TankController>() != null)
                GetComponent<TankController>().enabled = false;
            
            // make material partially black
            ChangeColor(gameObject, new Color(0.0f, 0.0f, 0.0f, 0.5f));

            // disable audio
            if (GetComponent<AudioSource>() != null)
                GetComponent<AudioSource>().enabled = false;

            // disable hitpoint
            enabled = false;
        }
    }

    void ChangeColor(GameObject obj, Color color)
    {
        if (obj == null)
        {
            return;
        }

        if (obj.GetComponent<Renderer>() != null)
        {
            obj.GetComponent<Renderer>().material.color = color;
        }

        foreach (Transform child in obj.transform)
        {
            ChangeColor(child.gameObject, color);
        }
    }
}
