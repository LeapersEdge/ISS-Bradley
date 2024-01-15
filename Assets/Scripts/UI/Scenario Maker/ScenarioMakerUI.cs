using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScenarioMakerUI : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] public GameObject playerTankPrefab;
    [SerializeField] GameObject[] terrainPrefabs;
    [SerializeField] GameObject[] enemyTankPrefabs;
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
        newEnemyTankButton.GetComponentInChildren<Text>().text = name;
        newEnemyTankButton.transform.name = index.ToString();
        Button button = newEnemyTankButton.GetComponent<Button>();
        button.onClick.AddListener(() => SelectEnemyTankButton(index));
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
        scenarioMaker.cursor_locked = true;
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

    }

    public void AddTargetLocation(Vector3 position, Vector3 rotation)
    {

    }

    public void DeleteTargetLocation()
    {

    }

    public void ToggleAgressiveTargetLocation()
    {

    }

    public void MoveTargetLocationPositionToNextClick()
    {
        
    }

    public void UpdateAimAccuracyX()
    {
        if (enemyTankEntryIndex != -1 && targetLocationIndex != -1)
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
        if (enemyTankEntryIndex != -1 && targetLocationIndex != -1)
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
        if (enemyTankEntryIndex != -1 && targetLocationIndex != -1)
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
    
        Vector3 position = scenarioMaker.target_location_instances[enemyTankEntryIndex][targetLocationIndex].transform.position;
        position.x = newPositionX;
        scenarioMaker.target_location_instances[enemyTankEntryIndex][targetLocationIndex].transform.position = position;
    }

    public void UpdatePositionY()
    {
        if (enemyTankEntryIndex != -1 && targetLocationIndex != -1)
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
    
        Vector3 position = scenarioMaker.target_location_instances[enemyTankEntryIndex][targetLocationIndex].transform.position;
        position.y = newPositionY;
        scenarioMaker.target_location_instances[enemyTankEntryIndex][targetLocationIndex].transform.position = position;
    }

    public void UpdatePositionZ()
    {
        if (enemyTankEntryIndex != -1 && targetLocationIndex != -1)
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
    
        Vector3 position = scenarioMaker.target_location_instances[enemyTankEntryIndex][targetLocationIndex].transform.position;
        position.z = newPositionZ;
        scenarioMaker.target_location_instances[enemyTankEntryIndex][targetLocationIndex].transform.position = position;
    }

    public void UpdateRotationX()
    {
        if (enemyTankEntryIndex != -1 && targetLocationIndex != -1)
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
    
        Vector3 rotation = scenarioMaker.target_location_instances[enemyTankEntryIndex][targetLocationIndex].transform.rotation.eulerAngles;
        rotation.x = newRotationX;
        scenarioMaker.target_location_instances[enemyTankEntryIndex][targetLocationIndex].transform.rotation = Quaternion.Euler(rotation);
    }

    public void UpdateRotationY()
    {
        if (enemyTankEntryIndex != -1 && targetLocationIndex != -1)
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
    
        Vector3 rotation = scenarioMaker.target_location_instances[enemyTankEntryIndex][targetLocationIndex].transform.rotation.eulerAngles;
        rotation.y = newRotationY;
        scenarioMaker.target_location_instances[enemyTankEntryIndex][targetLocationIndex].transform.rotation = Quaternion.Euler(rotation);
    }

    public void UpdateRotationZ()
    {
        if (enemyTankEntryIndex != -1 && targetLocationIndex != -1)
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

        Vector3 rotation = scenarioMaker.target_location_instances[enemyTankEntryIndex][targetLocationIndex].transform.rotation.eulerAngles;
        rotation.z = newRotationZ;
        scenarioMaker.target_location_instances[enemyTankEntryIndex][targetLocationIndex].transform.rotation = Quaternion.Euler(rotation);
    }
}
