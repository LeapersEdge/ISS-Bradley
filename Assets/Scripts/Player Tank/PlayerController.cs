using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;

    [Header("Objects")]
    [SerializeField] GameObject tankHead;
    [SerializeField] GameObject tankShell;
    GameObject tankBarell;
    [Header("Player Movement")]
    [SerializeField] float speed = 125f;
    [SerializeField] float turnSpeed = 125f;
    [SerializeField] float shellSpeed = 125f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        tankBarell = tankHead.transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        // movement
        rb.velocity = new Vector3(0, 0, Input.GetAxis("Vertical") * speed);
        if (Input.GetButton("Horizontal"))
        {
            this.transform.rotation = Quaternion.Euler(0, Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime, 0);
        }
        if (Input.GetButton("Horizontal Hat"))
        {
            tankHead.transform.Rotate(0, Input.GetAxis("Horizontal Hat") * turnSpeed * Time.deltaTime, 0);
        }

        if (Input.GetButtonDown("Fire1"))
        {
            GameObject shellSpawnLocation = tankBarell.transform.GetChild(0).gameObject;
            GameObject shell = Instantiate(tankShell, shellSpawnLocation.transform.position, shellSpawnLocation.transform.rotation);
            shell.GetComponent<TankShellController>().shellSpeed = shellSpeed;
            shellSpawnLocation.GetComponent<AudioSource>().Play();
        }
    }
}
