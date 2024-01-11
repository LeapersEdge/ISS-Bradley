using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScenarioMakerUI : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] GameObject playerTankPrefab;
    [SerializeField] GameObject[] terrainPrefabs;
    [SerializeField] GameObject[] enemyTankPrefabs;
    [SerializeField] GameObject enemyTankButtonPrefab;
    [SerializeField] GameObject targetLocationButtonPrefab;
    
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

    GameObject loadedTerrainPrefab;
    GameObject selectedTankPrefab;

    EnemyTankEntry[] enemyTankEntries;
    int enemyTankEntryIndex = -1;
    int targetLocationIndex = -1;
    ScenarioMakerMode scenarioMakerMode = ScenarioMakerMode.None;
    ScenarioMaker scenarioMaker;

    // Start is called before the first frame update
    void Start()
    {
        loadedTerrainPrefab = Instantiate(terrainPrefabs[0]);
        loadedTerrainPrefab.transform.position = new Vector3(0, 0, 0);
        loadedTerrainPrefab.transform.rotation = new Quaternion(0, 0, 0, 0);
        terrainDropdown.GetComponent<Dropdown>().value = 0;
        enemyTanksDropdown.GetComponent<Dropdown>().value = 0;
        scenarioMaker = FindObjectOfType<ScenarioMaker>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (scenarioMakerMode)
        {
            case ScenarioMakerMode.None:
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    scenarioMakerMode = ScenarioMakerMode.Freeroaming;
                    scenarioMaker.locked = true;
                }
                break;
            }
            case ScenarioMakerMode.Freeroaming:
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    scenarioMakerMode = ScenarioMakerMode.None;
                    scenarioMaker.locked = false;
                }
                break;
            }
            case ScenarioMakerMode.TankEdit:
            {
                
                break;
            }
            case ScenarioMakerMode.LocationEdit:
            {

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
    }

    //-------------------------------------------------------
    //-------------------------------------------------------
    //-------------------------------------------------------

    public void SelectPlayerTankButton()
    {

    }

    public void SelectMoveAboveSelectedButton()
    {

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
    }

    void AddToEnemyTankList()
    {

    }

    void RemoveFromEnemyTankList()
    {

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

    void AddTargetLocation()
    {

    }

    void DeleteTargetLocation()
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
    }
}
