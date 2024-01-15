using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class ScenarioMakerUI : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] public GameObject playerTankPrefab;
    [SerializeField] GameObject[] terrainPrefabs;
    [SerializeField] GameObject[] enemyTankPrefabs;
    [SerializeField] GameObject singleButtonPrefab;    
    [SerializeField] GameObject buttonPrefab;    
    [Header("Left UI Elements")]
    [SerializeField] GameObject terrainDropdown;
    [SerializeField] GameObject enemyTanksDropdown;
    [SerializeField] GameObject enemyTanksListContent;
    [SerializeField] GameObject targetLocationsListContent;
    
    [Header("Right UI Elements")]
    [SerializeField] GameObject agressiveTargetLocationToggle;
    [SerializeField] GameObject accuracyXText;
    [SerializeField] GameObject accuracyYText;
    [SerializeField] GameObject positionXText;
    [SerializeField] GameObject positionYText;
    [SerializeField] GameObject positionZText;
    [SerializeField] GameObject rotationXText;
    [SerializeField] GameObject rotationYText;
    [SerializeField] GameObject rotationZText;
    [Header("Big UI Pannels")]
    [SerializeField] GameObject leftUIPanel;
    [SerializeField] GameObject rightUIPanel;
    [SerializeField] GameObject hiddenUIPanel;

    GameObject loadedTerrainPrefab;
    [HideInInspector] public GameObject selectedTankPrefab;

    [HideInInspector] public bool move_player = false;
    [HideInInspector] public bool move_enemy = false;
    [HideInInspector] public bool move_target_location = false;
    [HideInInspector] public List<EnemyTankEntry> enemyTankEntries;
    [HideInInspector] public Transform playerTankTransform;
    ScenarioMaker scenarioMaker;

    [HideInInspector] public int enemyTankEntryIndex = -1;
    [HideInInspector] public int targetLocationIndex = -1;

    // Start is called before the first frame update
    void Start()
    {
        loadedTerrainPrefab = Instantiate(terrainPrefabs[0]);
        loadedTerrainPrefab.transform.position = new Vector3(0, 0, 0);
        loadedTerrainPrefab.transform.rotation = new Quaternion(0, 0, 0, 0);
        terrainDropdown.GetComponent<Dropdown>().value = 0;
        enemyTanksDropdown.GetComponent<Dropdown>().value = 0;

        scenarioMaker = FindObjectOfType<ScenarioMaker>().GetComponent<ScenarioMaker>();

        Dropdown areasDropdown = terrainDropdown.GetComponent<Dropdown>();
        areasDropdown.ClearOptions();
        for (int i = 0; i < terrainPrefabs.Length; i++)
            areasDropdown.options.Add(new Dropdown.OptionData(terrainPrefabs[i].name));

        Dropdown enemyDropdown = enemyTanksDropdown.GetComponent<Dropdown>();
        enemyDropdown.ClearOptions();
        for (int i = 0; i < enemyTankPrefabs.Length; i++)
            enemyDropdown.options.Add(new Dropdown.OptionData(enemyTankPrefabs[i].name));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //-------------------------------------------------------
    //-------------------------------------------------------
    //-------------------------------------------------------

    public void SelectPlayerTankButton()
    {
        selectedTankPrefab = playerTankPrefab;

        scenarioMaker.scenarioMakerMode = ScenarioMakerMode.Freeroaming;
        scenarioMaker.cursor_locked = true;
        leftUIPanel.SetActive(false);
        rightUIPanel.SetActive(false);
        hiddenUIPanel.SetActive(true);

        enemyTankEntryIndex = -1;
        targetLocationIndex = -1;
    }

    public void SelectMoveAboveSelectedButton()
    {
        if (selectedTankPrefab == playerTankPrefab)
        {
            move_player = true;
        }
        else if (selectedTankPrefab != null)
        {
            move_enemy = true;
        }

        scenarioMaker.scenarioMakerMode = ScenarioMakerMode.TankEdit;
        scenarioMaker.cursor_locked = true;
        leftUIPanel.SetActive(false);
        rightUIPanel.SetActive(false);
        hiddenUIPanel.SetActive(true);
    }

    public void ChangeTerrainOnDropdown()
    {
        int newIndex = terrainDropdown.GetComponent<Dropdown>().value;
        Destroy(loadedTerrainPrefab);
        loadedTerrainPrefab = Instantiate(terrainPrefabs[newIndex]);
        loadedTerrainPrefab.transform.position = new Vector3(0, 0, 0);
        loadedTerrainPrefab.transform.rotation = new Quaternion(0, 0, 0, 0);
    }

    public void ChangeEnemyTankOnDropdown()
    {
        int newIndex = enemyTanksDropdown.GetComponent<Dropdown>().value;
        selectedTankPrefab = enemyTankPrefabs[newIndex];

        scenarioMaker.scenarioMakerMode = ScenarioMakerMode.Freeroaming;
        scenarioMaker.cursor_locked = true;
        leftUIPanel.SetActive(false);
        rightUIPanel.SetActive(false);
        hiddenUIPanel.SetActive(true);
    }

    public void AddEnemyTankUIElement(String name, int index)
    {
        GameObject newEnemyTankButton = Instantiate(buttonPrefab);
        newEnemyTankButton.transform.SetParent(enemyTanksListContent.transform);
        newEnemyTankButton.transform.localScale = new Vector3(1, 1, 1);
        newEnemyTankButton.GetComponentsInChildren<Text>()[0].text = name;
        newEnemyTankButton.GetComponentsInChildren<Text>()[1].text = "Target Locations";
        newEnemyTankButton.transform.name = index.ToString();
        Button button1 = newEnemyTankButton.GetComponentsInChildren<Button>()[0];
        Button button2 = newEnemyTankButton.GetComponentsInChildren<Button>()[1];
        button1.onClick.AddListener(() => SelectEnemyTankButton(index));
        button2.onClick.AddListener(() => SelectEnemyTankForTargetLocationButton(index));
    }

    public void RefreshEnemyTankList()
    {
        // delete all children of enemyTanksListContent
        foreach (Transform child in enemyTanksListContent.transform)
        {
            Destroy(child.gameObject);
        }

        // add new children
        for (int i = 0; i < enemyTankEntries.Count; i++)
        {
            AddEnemyTankUIElement(enemyTankEntries[i].tankPrefabName, i);
        }
    }

    public void SelectEnemyTankButton(int index)
    {
        enemyTankEntryIndex = index;
        scenarioMaker.scenarioMakerMode = ScenarioMakerMode.TankEdit;
        rightUIPanel.SetActive(false);
    }

    public void SelectEnemyTankForTargetLocationButton(int index)
    {
        enemyTankEntryIndex = index;
        targetLocationIndex = -1;        
        scenarioMaker.scenarioMakerMode = ScenarioMakerMode.LocationEdit;
        scenarioMaker.cursor_locked = true;
        leftUIPanel.SetActive(false);
        hiddenUIPanel.SetActive(true);
        targetLocationIndex = 0;
        RefreshTargetLocationList();
        rightUIPanel.SetActive(true);
    }

    void SaveScenario()
    {

    }

    void LoadScenario()
    {

    }

    //-------------------------------------------------------
    //-------------------------------------------------------
    //-------------------------------------------------------

    public void ToggleTargetLocationSelection()
    {
        
    }

    public void RefreshTargetLocationList()
    {
        // delete all children of targetLocationsListContent
        foreach (Transform child in targetLocationsListContent.transform)
        {
            Destroy(child.gameObject);
        }

        // add new children
        for (int i = 0; i < enemyTankEntries[enemyTankEntryIndex].targetLocations.Count; i++)
        {
            AddTargetLocationUIElement(i);
        }

        SelectTargetLocationButton(targetLocationIndex);
    }

    public void AddTargetLocationUIElement(int index)
    {
        GameObject newTargetLocationButton = Instantiate(singleButtonPrefab);
        newTargetLocationButton.transform.SetParent(targetLocationsListContent.transform);
        newTargetLocationButton.transform.localScale = new Vector3(1, 1, 1);
        newTargetLocationButton.GetComponentInChildren<Text>().text = "Target Location " + index;
        newTargetLocationButton.transform.name = index.ToString();
        Button button = newTargetLocationButton.GetComponentInChildren<Button>();
        button.onClick.AddListener(() => SelectTargetLocationButton(index));
    }

    public void SelectTargetLocationButton(int index)
    {
        targetLocationIndex = index;
        scenarioMaker.scenarioMakerMode = ScenarioMakerMode.LocationEdit;
        scenarioMaker.cursor_locked = false;
        rightUIPanel.SetActive(true);

        // update UI
        agressiveTargetLocationToggle.GetComponent<Toggle>().isOn = enemyTankEntries[enemyTankEntryIndex].targetLocations[targetLocationIndex].agressive;
        accuracyXText.GetComponent<InputField>().text = enemyTankEntries[enemyTankEntryIndex].targetLocations[targetLocationIndex].aimingAccuracyPercentageX.ToString();
        accuracyYText.GetComponent<InputField>().text = enemyTankEntries[enemyTankEntryIndex].targetLocations[targetLocationIndex].aimingAccuracyPercentageY.ToString();
        positionXText.GetComponent<InputField>().text = enemyTankEntries[enemyTankEntryIndex].targetLocations[targetLocationIndex].location.position.x.ToString();
        positionYText.GetComponent<InputField>().text = enemyTankEntries[enemyTankEntryIndex].targetLocations[targetLocationIndex].location.position.y.ToString();
        positionZText.GetComponent<InputField>().text = enemyTankEntries[enemyTankEntryIndex].targetLocations[targetLocationIndex].location.position.z.ToString();
        rotationXText.GetComponent<InputField>().text = enemyTankEntries[enemyTankEntryIndex].targetLocations[targetLocationIndex].location.rotation.eulerAngles.x.ToString();
        rotationYText.GetComponent<InputField>().text = enemyTankEntries[enemyTankEntryIndex].targetLocations[targetLocationIndex].location.rotation.eulerAngles.y.ToString();
        rotationZText.GetComponent<InputField>().text = enemyTankEntries[enemyTankEntryIndex].targetLocations[targetLocationIndex].location.rotation.eulerAngles.z.ToString();
    }

    public void AddTargetLocation(Vector3 position, Vector3 rotation)
    {
        TargetLocation newTargetLocation = new TargetLocation();
        newTargetLocation.location.position = position;
        newTargetLocation.location.rotation = Quaternion.Euler(rotation);
        newTargetLocation.aimingAccuracyPercentageX = 0.5f;
        newTargetLocation.aimingAccuracyPercentageY = 0.5f;
        newTargetLocation.agressive = false;
        enemyTankEntries[enemyTankEntryIndex].targetLocations.Add(newTargetLocation);
        RefreshTargetLocationList();
    }

    public void DeleteTargetLocation()
    {
        if (enemyTankEntryIndex != -1 && targetLocationIndex != -1)
        {
            enemyTankEntries[enemyTankEntryIndex].targetLocations.RemoveAt(targetLocationIndex);
            RefreshTargetLocationList();
        }
        else
        {
            Debug.LogError("DeleteTargetLocation at scenariomakerUI.cs was called with wrong entry indexes with values: " + enemyTankEntryIndex + " " + targetLocationIndex);
        }
    }

    public void ToggleAgressiveTargetLocation()
    {
        if (enemyTankEntryIndex != -1 && targetLocationIndex != -1)
        {
            enemyTankEntries[enemyTankEntryIndex].targetLocations[targetLocationIndex].agressive = agressiveTargetLocationToggle.GetComponent<Toggle>().isOn;
        }
        else
        {
            Debug.LogError("ToggleAgressiveTargetLocation at scenariomakerUI.cs was called with wrong entry indexes with values: " + enemyTankEntryIndex + " " + targetLocationIndex);
        }
    }

    public void MoveTargetLocationPositionToNextClick()
    {
        if (enemyTankEntryIndex != -1 && targetLocationIndex != -1 && targetLocationIndex != 0)
        {
            move_target_location = true;
        }
        else
        {
            Debug.LogError("MoveTargetLocationPositionToNextClick at scenariomakerUI.cs was called with wrong entry indexes with values: " + enemyTankEntryIndex + " " + targetLocationIndex);
        }

        scenarioMaker.cursor_locked = true;
    }

    public void UpdateAimAccuracyX()
    {
        if (enemyTankEntryIndex == -1 || targetLocationIndex == -1)
        {
            Debug.LogError("updateainaccuracyX at scenariomakerUI.cs was called with wrong entry indexes with values: " + enemyTankEntryIndex + " " + targetLocationIndex);
            return;
        }

        InputField inputField = accuracyXText.GetComponent<InputField>();
        string newText = inputField.text;
        float newAccuracyX = 0;

        bool onlyFloatDigits = true;
        bool needsToShiftCommaToDot = false;
        #region Check if input is valid

        foreach (char c in newText)
        {
            if (c == ',')
            {
                needsToShiftCommaToDot = true;
            }
            else if ((c < '0' || c > '9') && c != '.')
            {
                onlyFloatDigits = false;
            }
        }
        if (needsToShiftCommaToDot)
        {
            newText = newText.Replace(',', '.');
        }

        #endregion  

        if (onlyFloatDigits)
        {
            newAccuracyX = float.Parse(newText);
            inputField.text = newText;

            enemyTankEntries[enemyTankEntryIndex].targetLocations[targetLocationIndex].aimingAccuracyPercentageX = newAccuracyX;
        }
        else 
        {
            // restoring old value if such value exists
            inputField.text = enemyTankEntries[enemyTankEntryIndex].targetLocations[targetLocationIndex].aimingAccuracyPercentageX.ToString();
        }
    }

    public void UpdateAimAccuracyY()
    {
        if (enemyTankEntryIndex == -1 || targetLocationIndex == -1)
        {
            Debug.LogError("updateainaccuracyY at scenariomakerUI.cs was called with wrong entry indexes with values: " + enemyTankEntryIndex + " " + targetLocationIndex);
            return;
        }

        InputField inputField = accuracyYText.GetComponent<InputField>();
        string newText = inputField.text;
        float newAccuracyY = 0;

        bool onlyFloatDigits = true;
        bool needsToShiftCommaToDot = false;
        #region Check if input is valid

        foreach (char c in newText)
        {
            if (c == ',')
            {
                needsToShiftCommaToDot = true;
            }
            else if ((c < '0' || c > '9') && c != '.')
            {
                onlyFloatDigits = false;
            }
        }
        if (needsToShiftCommaToDot)
        {
            newText = newText.Replace(',', '.');
        }

        #endregion

        

        if (onlyFloatDigits)
        {
            newAccuracyY = float.Parse(newText);
            inputField.text = newText;

            enemyTankEntries[enemyTankEntryIndex].targetLocations[targetLocationIndex].aimingAccuracyPercentageX = newAccuracyY;
        }
        else 
        {
            // restoring old value if such value exists
            inputField.text = enemyTankEntries[enemyTankEntryIndex].targetLocations[targetLocationIndex].aimingAccuracyPercentageY.ToString();
        }
    }

    public void UpdatePositionX()
    {
        if (enemyTankEntryIndex == -1 || targetLocationIndex == -1)
        {
            Debug.LogError("updatepositionX at scenariomakerUI.cs was called with wrong entry indexes with values: " + enemyTankEntryIndex + " " + targetLocationIndex);
            return;
        }

        InputField inputField = positionXText.GetComponent<InputField>();
        string newText = inputField.text;
        float newPositionX = 0.0f;

        bool onlyFloatDigits = true;
        bool needsToShiftCommaToDot = false;
        #region Check if input is valid

        foreach (char c in newText)
        {
            if (c == ',')
            {
                needsToShiftCommaToDot = true;
            }
            else if ((c < '0' || c > '9') && c != '.')
            {
                onlyFloatDigits = false;
            }
        }
        if (needsToShiftCommaToDot)
        {
            newText = newText.Replace(',', '.');
        }

        #endregion

        if (onlyFloatDigits)
        {
            newPositionX = float.Parse(newText);
            inputField.text = newText;

            Vector3 newPosition = enemyTankEntries[enemyTankEntryIndex].targetLocations[targetLocationIndex].location.position;
            newPosition.x = newPositionX;
            enemyTankEntries[enemyTankEntryIndex].targetLocations[targetLocationIndex].location.position = newPosition;
        }
        else 
        {
            // restoring old value if such value exists
            inputField.text = enemyTankEntries[enemyTankEntryIndex].targetLocations[targetLocationIndex].location.position.x.ToString();
        }
    
        if (targetLocationIndex == 0)
        {
            return;
        }

        Vector3 position = scenarioMaker.target_location_instances[enemyTankEntryIndex][targetLocationIndex - 1].transform.position;
        position.x = newPositionX;
        scenarioMaker.target_location_instances[enemyTankEntryIndex][targetLocationIndex - 1].transform.position = position;
    }

    public void UpdatePositionY()
    {
        if (enemyTankEntryIndex == -1 || targetLocationIndex == -1)
        {
            Debug.LogError("updatepositionY at scenariomakerUI.cs was called with wrong entry indexes with values: " + enemyTankEntryIndex + " " + targetLocationIndex);
            return;
        }

        InputField inputField = positionYText.GetComponent<InputField>();
        string newText = inputField.text;
        float newPositionY = 0.0f;

        bool onlyFloatDigits = true;
        bool needsToShiftCommaToDot = false;
        #region Check if input is valid

        foreach (char c in newText)
        {
            if (c == ',')
            {
                needsToShiftCommaToDot = true;
            }
            else if ((c < '0' || c > '9') && c != '.')
            {
                onlyFloatDigits = false;
            }
        }
        if (needsToShiftCommaToDot)
        {
            newText = newText.Replace(',', '.');
        }

        #endregion

        if (onlyFloatDigits)
        {
            newPositionY = float.Parse(newText);
            inputField.text = newText;

            Vector3 newPosition = enemyTankEntries[enemyTankEntryIndex].targetLocations[targetLocationIndex].location.position;
            newPosition.y = newPositionY;
            enemyTankEntries[enemyTankEntryIndex].targetLocations[targetLocationIndex].location.position = newPosition;
        }
        else 
        {
            // restoring old value if such value exists
            inputField.text = enemyTankEntries[enemyTankEntryIndex].targetLocations[targetLocationIndex].location.position.y.ToString();
        }
    
        if (targetLocationIndex == 0)
        {
            return;
        }

        Vector3 position = scenarioMaker.target_location_instances[enemyTankEntryIndex][targetLocationIndex - 1].transform.position;
        position.y = newPositionY;
        scenarioMaker.target_location_instances[enemyTankEntryIndex][targetLocationIndex - 1].transform.position = position;
    }

    public void UpdatePositionZ()
    {
        if (enemyTankEntryIndex == -1 || targetLocationIndex == -1)
        {
            Debug.LogError("updatepositionZ at scenariomakerUI.cs was called with wrong entry indexes with values: " + enemyTankEntryIndex + " " + targetLocationIndex);
            return;
        }

        InputField inputField = positionZText.GetComponent<InputField>();
        string newText = inputField.text;
        float newPositionZ = 0.0f;

        bool onlyFloatDigits = true;
        bool needsToShiftCommaToDot = false;
        #region Check if input is valid

        foreach (char c in newText)
        {
            if (c == ',')
            {
                needsToShiftCommaToDot = true;
            }
            else if ((c < '0' || c > '9') && c != '.')
            {
                onlyFloatDigits = false;
            }
        }
        if (needsToShiftCommaToDot)
        {
            newText = newText.Replace(',', '.');
        }

        #endregion

        if (onlyFloatDigits)
        {
            newPositionZ = float.Parse(newText);
            inputField.text = newText;

            Vector3 newPosition = enemyTankEntries[enemyTankEntryIndex].targetLocations[targetLocationIndex].location.position;
            newPosition.z = newPositionZ;
            enemyTankEntries[enemyTankEntryIndex].targetLocations[targetLocationIndex].location.position = newPosition;
        }
        else 
        {
            // restoring old value if such value exists
            inputField.text = enemyTankEntries[enemyTankEntryIndex].targetLocations[targetLocationIndex].location.position.z.ToString();
        }
    
        if (targetLocationIndex == 0)
        {
            return;
        }

        Vector3 position = scenarioMaker.target_location_instances[enemyTankEntryIndex][targetLocationIndex - 1].transform.position;
        position.z = newPositionZ;
        scenarioMaker.target_location_instances[enemyTankEntryIndex][targetLocationIndex - 1].transform.position = position;
    }

    public void UpdateRotationX()
    {
        if (enemyTankEntryIndex == -1 || targetLocationIndex == -1)
        {
            Debug.LogError("updaterotationX at scenariomakerUI.cs was called with wrong entry indexes with values: " + enemyTankEntryIndex + " " + targetLocationIndex);
            return;
        }

        InputField inputField = rotationXText.GetComponent<InputField>();
        string newText = inputField.text;
        float newRotationX = 0.0f;

        bool onlyFloatDigits = true;
        bool needsToShiftCommaToDot = false;
        #region Check if input is valid

        foreach (char c in newText)
        {
            if (c == ',')
            {
                needsToShiftCommaToDot = true;
            }
            else if ((c < '0' || c > '9') && c != '.')
            {
                onlyFloatDigits = false;
            }
        }
        if (needsToShiftCommaToDot)
        {
            newText = newText.Replace(',', '.');
        }

        #endregion

        if (onlyFloatDigits)
        {
            newRotationX = float.Parse(newText);
            inputField.text = newText;

            Vector3 newRotation = enemyTankEntries[enemyTankEntryIndex].targetLocations[targetLocationIndex].location.rotation.eulerAngles;
            newRotation.x = newRotationX;
            enemyTankEntries[enemyTankEntryIndex].targetLocations[targetLocationIndex].location.rotation = Quaternion.Euler(newRotation);
        }
        else 
        {
            // restoring old value if such value exists
            inputField.text = enemyTankEntries[enemyTankEntryIndex].targetLocations[targetLocationIndex].location.rotation.eulerAngles.x.ToString();
        }
    
        if (targetLocationIndex == 0)
        {
            return;
        }

        Vector3 rotation = scenarioMaker.target_location_instances[enemyTankEntryIndex][targetLocationIndex].transform.rotation.eulerAngles;
        rotation.x = newRotationX;
        scenarioMaker.target_location_instances[enemyTankEntryIndex][targetLocationIndex].transform.rotation = Quaternion.Euler(rotation);
    }

    public void UpdateRotationY()
    {
        if (enemyTankEntryIndex == -1 || targetLocationIndex == -1)
        {
            Debug.LogError("updaterotationY at scenariomakerUI.cs was called with wrong entry indexes with values: " + enemyTankEntryIndex + " " + targetLocationIndex);
            return;
        }

        InputField inputField = rotationYText.GetComponent<InputField>();
        string newText = inputField.text;
        float newRotationY = 0.0f;

        bool onlyFloatDigits = true;
        bool needsToShiftCommaToDot = false;
        #region Check if input is valid

        foreach (char c in newText)
        {
            if (c == ',')
            {
                needsToShiftCommaToDot = true;
            }
            else if ((c < '0' || c > '9') && c != '.')
            {
                onlyFloatDigits = false;
            }
        }
        if (needsToShiftCommaToDot)
        {
            newText = newText.Replace(',', '.');
        }

        #endregion

        if (onlyFloatDigits)
        {
            newRotationY = float.Parse(newText);
            inputField.text = newText;

            Vector3 newRotation = enemyTankEntries[enemyTankEntryIndex].targetLocations[targetLocationIndex].location.rotation.eulerAngles;
            newRotation.y = newRotationY;
            enemyTankEntries[enemyTankEntryIndex].targetLocations[targetLocationIndex].location.rotation = Quaternion.Euler(newRotation);
        }
        else 
        {
            // restoring old value if such value exists
            inputField.text = enemyTankEntries[enemyTankEntryIndex].targetLocations[targetLocationIndex].location.rotation.eulerAngles.y.ToString();
        }
    
        if (targetLocationIndex == 0)
        {
            return;
        }

        Vector3 rotation = scenarioMaker.target_location_instances[enemyTankEntryIndex][targetLocationIndex].transform.rotation.eulerAngles;
        rotation.y = newRotationY;
        scenarioMaker.target_location_instances[enemyTankEntryIndex][targetLocationIndex].transform.rotation = Quaternion.Euler(rotation);
    }

    public void UpdateRotationZ()
    {
        if (enemyTankEntryIndex == -1 || targetLocationIndex == -1)
        {
            Debug.LogError("updaterotationZ at scenariomakerUI.cs was called with wrong entry indexes with values: " + enemyTankEntryIndex + " " + targetLocationIndex);
            return;
        }

        InputField inputField = rotationZText.GetComponent<InputField>();
        string newText = inputField.text;
        float newRotationZ = 0.0f;

        bool onlyFloatDigits = true;
        bool needsToShiftCommaToDot = false;
        #region Check if input is valid

        foreach (char c in newText)
        {
            if (c == ',')
            {
                needsToShiftCommaToDot = true;
            }
            else if ((c < '0' || c > '9') && c != '.')
            {
                onlyFloatDigits = false;
            }
        }
        if (needsToShiftCommaToDot)
        {
            newText = newText.Replace(',', '.');
        }

        #endregion
    
        if (onlyFloatDigits)
        {
            newRotationZ = float.Parse(newText);
            inputField.text = newText;

            Vector3 newRotation = enemyTankEntries[enemyTankEntryIndex].targetLocations[targetLocationIndex].location.rotation.eulerAngles;
            newRotation.z = newRotationZ;
            enemyTankEntries[enemyTankEntryIndex].targetLocations[targetLocationIndex].location.rotation = Quaternion.Euler(newRotation);
        }
        else 
        {
            // restoring old value if such value exists
            inputField.text = enemyTankEntries[enemyTankEntryIndex].targetLocations[targetLocationIndex].location.rotation.eulerAngles.z.ToString();
        }

        if (targetLocationIndex == 0)
        {
            return;
        }

        Vector3 rotation = scenarioMaker.target_location_instances[enemyTankEntryIndex][targetLocationIndex].transform.rotation.eulerAngles;
        rotation.z = newRotationZ;
        scenarioMaker.target_location_instances[enemyTankEntryIndex][targetLocationIndex].transform.rotation = Quaternion.Euler(rotation);
    }
}
