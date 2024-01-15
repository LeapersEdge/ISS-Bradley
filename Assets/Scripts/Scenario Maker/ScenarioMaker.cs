using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class ScenarioMaker : MonoBehaviour
{
    [Header("Movement metrics")]
    [SerializeField] float speed = 125f;
    [SerializeField] float turnSpeed = 125f;
    [Header("Objects")]
    [SerializeField] public bool cursor_locked = false;
    [SerializeField] GameObject crosshairUIPanel;
    [SerializeField] ScenarioMakerUI scenarioMakerUI;

    GameObject player_tank_instance = null;
    List<GameObject> enemy_tank_instances = new List<GameObject>();
    [HideInInspector] public List<List<GameObject>> target_location_instances = new List<List<GameObject>>();

    public ScenarioMakerMode scenarioMakerMode = ScenarioMakerMode.None;

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

        if (cursor_locked)
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
        if (cursor_locked)
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

        switch (scenarioMakerMode)
        {
            case ScenarioMakerMode.None:
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    scenarioMakerMode = ScenarioMakerMode.Freeroaming;
                    cursor_locked = true;
                }
                break;
            }
            case ScenarioMakerMode.Freeroaming:
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    scenarioMakerMode = ScenarioMakerMode.None;
                    cursor_locked = false;
                }

                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    RaycastHit hit;
                    if (Physics.Raycast(transform.position, transform.forward, out hit, 1000f))
                    {
                        GameObject to_instantiate = scenarioMakerUI.selectedTankPrefab;
                        if (to_instantiate != null)
                        {
                            Vector3 hit_point = hit.point;
                            hit_point.y += 1f;
                            
                            // set the z rotatin to be the same as the camera's

                            // check if instantiated tank is an enemy tank
                            if (to_instantiate.GetComponent<PlayerController>() == null)
                            {
                                GameObject new_tank = Instantiate(to_instantiate, hit_point, Quaternion.identity);
                                new_tank.transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
                                new_tank.GetComponent<TankController>().enabled = false;
                                new_tank.GetComponent<Rigidbody>().isKinematic = true;

                                EnemyTankEntry enemyTankEntry = new EnemyTankEntry();
                                enemyTankEntry.tankPrefabName = to_instantiate.name;
                                enemyTankEntry.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));

                                TargetLocation targetLocation = new TargetLocation();
                                targetLocation.location.position = new_tank.transform.position;
                                targetLocation.location.rotation = new_tank.transform.rotation;
                                targetLocation.agressive = false;
                                targetLocation.aimingAccuracyPercentageX = 0.5f;
                                targetLocation.aimingAccuracyPercentageY = 0.5f;
                                enemyTankEntry.targetLocations.Add(targetLocation);

                                scenarioMakerUI.enemyTankEntries.Add(enemyTankEntry);
                                scenarioMakerUI.RefreshEnemyTankList();

                                enemy_tank_instances.Add(new_tank);
                                scenarioMakerUI.enemyTankEntryIndex = enemy_tank_instances.Count - 1;

                                List<GameObject> target_location_instances_for_new_tank = new List<GameObject>();
                                target_location_instances.Add(target_location_instances_for_new_tank);
                            }
                            else if (player_tank_instance == null)
                            {
                                GameObject new_tank = Instantiate(to_instantiate, hit_point, Quaternion.identity);
                                new_tank.transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
                                new_tank.GetComponent<TankController>().enabled = false;
                                new_tank.GetComponentInChildren<Camera>().enabled = false;
                                new_tank.GetComponent<Rigidbody>().isKinematic = true;
                                new_tank.GetComponentInChildren<AudioListener>().enabled = false;
                                scenarioMakerUI.playerTankTransform = new_tank.transform;
                                player_tank_instance = new_tank;
                            }
                        }
                    }
                }

                break;
            }
            case ScenarioMakerMode.TankEdit:
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    scenarioMakerMode = ScenarioMakerMode.None;
                    cursor_locked = false;
                }

                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    RaycastHit hit;
                    if (Physics.Raycast(transform.position, transform.forward, out hit, 1000f))
                    {
                        if (scenarioMakerUI.move_player && player_tank_instance != null)
                        {
                            scenarioMakerUI.move_player = false;
                            Vector3 hit_point = hit.point;
                            hit_point.y += 1f;
                            scenarioMakerUI.playerTankTransform.position = hit_point;
                            scenarioMakerUI.playerTankTransform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);

                            player_tank_instance.transform.position = hit_point;
                            player_tank_instance.transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
                        }
                        else if (scenarioMakerUI.move_enemy && scenarioMakerUI.enemyTankEntryIndex != -1 && scenarioMakerUI.enemyTankEntryIndex < enemy_tank_instances.Count)
                        {
                            scenarioMakerUI.move_enemy = false;

                            int index = scenarioMakerUI.enemyTankEntryIndex;
                            Vector3 hit_point = hit.point;
                            hit_point.y += 1f;
                            scenarioMakerUI.enemyTankEntries[index].targetLocations[0].location.position = hit_point;
                            scenarioMakerUI.enemyTankEntries[index].targetLocations[0].location.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
                            scenarioMakerUI.RefreshTargetLocationList();

                            enemy_tank_instances[index].transform.position = hit_point;
                            enemy_tank_instances[index].transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
                        }
                        
                    }
                }

                if (Input.GetKeyDown(KeyCode.Delete))
                {
                    // check if selected tank is player tank
                    if (scenarioMakerUI.selectedTankPrefab.GetComponent<PlayerController>() != null && player_tank_instance != null)
                    {
                        scenarioMakerUI.playerTankTransform = null;
                        Destroy(player_tank_instance);
                    }
                    // check if selected tank is enemy tank
                    else if (scenarioMakerUI.enemyTankEntryIndex != -1 && scenarioMakerUI.enemyTankEntryIndex < enemy_tank_instances.Count)
                    {
                        int index = scenarioMakerUI.enemyTankEntryIndex;
                        Destroy(enemy_tank_instances[index]);
                        enemy_tank_instances.RemoveAt(index);

                        for (int i = 0; i < target_location_instances[index].Count; i++)
                        {
                            Destroy(target_location_instances[index][i]);
                        }
                        target_location_instances.RemoveAt(index);

                        scenarioMakerUI.enemyTankEntries.RemoveAt(index);
                        scenarioMakerUI.RefreshEnemyTankList();

                        scenarioMakerUI.enemyTankEntryIndex -= 1;
                    }
                }

                break;
            }
            case ScenarioMakerMode.LocationEdit:
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    cursor_locked = !cursor_locked;
                }

                if (Input.GetKeyDown(KeyCode.Mouse0) && cursor_locked)
                {
                    if (scenarioMakerUI.move_target_location && 
                        scenarioMakerUI.targetLocationIndex != -1 &&
                        scenarioMakerUI.enemyTankEntryIndex != -1 &&
                        scenarioMakerUI.enemyTankEntryIndex < enemy_tank_instances.Count && 
                        scenarioMakerUI.targetLocationIndex < scenarioMakerUI.enemyTankEntries[scenarioMakerUI.enemyTankEntryIndex].targetLocations.Count 
                    )
                    {
                        scenarioMakerUI.move_target_location = false;
                        RaycastHit hit;
                        if (Physics.Raycast(transform.position, transform.forward, out hit, 1000f))
                        {
                            int index = scenarioMakerUI.targetLocationIndex;
                            Vector3 hit_point = hit.point;
                            hit_point.y += 0.5f;
                            scenarioMakerUI.enemyTankEntries[scenarioMakerUI.enemyTankEntryIndex].targetLocations[index].location.position = hit_point;
                            scenarioMakerUI.enemyTankEntries[scenarioMakerUI.enemyTankEntryIndex].targetLocations[index].location.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);

                            enemy_tank_instances[scenarioMakerUI.enemyTankEntryIndex].transform.position = hit_point;
                            enemy_tank_instances[scenarioMakerUI.enemyTankEntryIndex].transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);

                            target_location_instances[scenarioMakerUI.enemyTankEntryIndex][index].transform.position = hit_point;
                            target_location_instances[scenarioMakerUI.enemyTankEntryIndex][index].transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
                        }
                    }
                    else if ( scenarioMakerUI.targetLocationIndex != -1 &&
                        scenarioMakerUI.enemyTankEntryIndex != -1 &&
                        scenarioMakerUI.enemyTankEntryIndex < scenarioMakerUI.enemyTankEntries.Count &&
                        scenarioMakerUI.targetLocationIndex < scenarioMakerUI.enemyTankEntries[scenarioMakerUI.enemyTankEntryIndex].targetLocations.Count    
                    )
                    {
                        RaycastHit hit;
                        if (Physics.Raycast(transform.position, transform.forward, out hit, 1000f))
                        {
                            // add new entry to target location list and instantiage a sphere at the hit point with color from enemytankentry
                            Vector3 hit_point = hit.point;
                            hit_point.y += 0.5f;

                            int tank_index = scenarioMakerUI.enemyTankEntryIndex;
                            Color color = scenarioMakerUI.enemyTankEntries[tank_index].color;
                            GameObject new_target_location = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                            new_target_location.transform.position = hit_point;
                            new_target_location.transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
                            new_target_location.GetComponent<MeshRenderer>().material.color = color;
                            target_location_instances[tank_index].Add(new_target_location);

                            scenarioMakerUI.AddTargetLocation(hit_point, new Vector3(0, this.transform.rotation.eulerAngles.y, 0));
                            scenarioMakerUI.targetLocationIndex = scenarioMakerUI.enemyTankEntries[tank_index].targetLocations.Count - 1;
                        }
                    }
                }

                if (Input.GetKeyDown(KeyCode.Delete))
                {
                    if (scenarioMakerUI.targetLocationIndex != -1 &&
                        scenarioMakerUI.enemyTankEntryIndex != -1 &&
                        scenarioMakerUI.targetLocationIndex != 0 &&
                        scenarioMakerUI.enemyTankEntryIndex < scenarioMakerUI.enemyTankEntries.Count &&
                        scenarioMakerUI.targetLocationIndex < scenarioMakerUI.enemyTankEntries[scenarioMakerUI.enemyTankEntryIndex].targetLocations.Count
                    )
                    {
                        int tank_index = scenarioMakerUI.enemyTankEntryIndex;
                        int target_location_index = scenarioMakerUI.targetLocationIndex;
                        Destroy(target_location_instances[tank_index][target_location_index - 1]);
                        target_location_instances[tank_index].RemoveAt(target_location_index - 1);

                        scenarioMakerUI.enemyTankEntries[tank_index].targetLocations.RemoveAt(target_location_index);
                        scenarioMakerUI.RefreshTargetLocationList();

                        scenarioMakerUI.targetLocationIndex -= 1;
                    }
                }

                break;
            }
            default:
            {
                Debug.LogError("ScenarioMakerMode in scenariomakerUI.cs is in unknown/unhandled state: " + scenarioMakerMode);
                Debug.LogWarning("Setting ScenarioMakerMode to None");
                scenarioMakerMode = ScenarioMakerMode.None;
                break;
            }
        }



        if (cursor_locked)  {   UpdateLocked(); }
        else                {   UpdateUnlocked();   }
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
                /*
                GameObject dot = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                dot.transform.position = hit.point;
                dot.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                dot.GetComponent<MeshRenderer>().material.color = Color.red;
                */
            }
        }
    }
}
