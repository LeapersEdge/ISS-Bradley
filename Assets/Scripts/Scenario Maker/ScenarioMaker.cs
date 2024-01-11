using UnityEngine;
using UnityEngine.UIElements;

public class ScenarioMaker : MonoBehaviour
{
    [Header("Movement metrics")]
    [SerializeField] float speed = 125f;
    [SerializeField] float turnSpeed = 125f;
    [Header("Objects")]
    [SerializeField] public bool locked = false;
    [SerializeField] GameObject crosshairUIPanel;

    bool firstMouseDeltaX = false;
    bool firstMouseDeltaY = false;

    float theta = 0f;
    float phi = 0f;

    void Start()
    {
        Camera camera = Camera.main;
        camera.transform.position = new Vector3(0, 0, 0);
        camera.transform.rotation = Quaternion.Euler(0, 0, 0);
        camera.transform.localScale = new Vector3(1, 1, 1);
        camera.transform.parent = this.gameObject.transform;

        if (locked)
        {

            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            UnityEngine.Cursor.visible = false;
            crosshairUIPanel.SetActive(true);
        }
        else
        {
            UnityEngine.Cursor.lockState = CursorLockMode.None;
            UnityEngine.Cursor.visible = true;
            crosshairUIPanel.SetActive(false);
        }        
    }

    void Update()
    {
        if (locked) {   UpdateLocked(); }
        else        {   UpdateUnlocked();   }
    }

    void UpdateUnlocked()
    {
        float mouseDeltaX = Input.GetAxisRaw("Mouse X");
        float mouseDeltaY = Input.GetAxisRaw("Mouse Y");
    }

    void UpdateLocked()
    {
        float mouseDeltaX = Input.GetAxisRaw("Mouse X");
        float mouseDeltaY = Input.GetAxisRaw("Mouse Y");

        if (!firstMouseDeltaX && mouseDeltaX != 0)
        {
            mouseDeltaX = 0;
            firstMouseDeltaX = true;
        }
        if (!firstMouseDeltaY && mouseDeltaY != 0)
        {
            mouseDeltaY = 0;
            firstMouseDeltaY = true;
        }

        Vector3 translateForward = transform.forward;
        translateForward.y = 0;
        transform.Translate(Input.GetAxisRaw("Vertical") * translateForward * speed * Time.deltaTime, Space.World);
        transform.Translate(Input.GetAxisRaw("Horizontal") * transform.right * speed * Time.deltaTime, Space.World);
        transform.Translate(Input.GetAxisRaw("Flying") * Vector3.up * speed * Time.deltaTime, Space.World);

        phi += -mouseDeltaY * turnSpeed * Time.deltaTime;
        theta += mouseDeltaX * turnSpeed * Time.deltaTime;
        phi = Mathf.Clamp(phi, -85, 85);

        transform.rotation = Quaternion.Euler(phi, theta, 0);

        if (Input.GetButtonDown("Fire1"))
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, 1000f))
            {
                // TODO: other stuff
                // draw a dot on the hit point
                GameObject dot = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                dot.transform.position = hit.point;
                dot.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                dot.GetComponent<MeshRenderer>().material.color = Color.red;
            }
        }
    }
}
