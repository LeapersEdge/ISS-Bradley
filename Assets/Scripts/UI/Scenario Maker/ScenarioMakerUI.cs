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

    // Start is called before the first frame update
    void Start()
    {
        
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

    }

    public void SelectMoveAboveSelectedButton()
    {

    }

    public void ChangeTerrainOnDropdown()
    {
        int newIndex = terrainDropdown.GetComponent<Dropdown>().value;

    }

    public void ChangeEnemyTankOnDropdown()
    {
        int newIndex = enemyTanksDropdown.GetComponent<Dropdown>().value;

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
        InputField inputField = accuracyXText.GetComponent<InputField>();
        string newText = inputField.text;

    }

    public void UpdateAimAccuracyY()
    {
        InputField inputField = accuracyYText.GetComponent<InputField>();
        string newText = inputField.text;

    }

    public void UpdatePositionX()
    {
        InputField inputField = positionXText.GetComponent<InputField>();
        string newText = inputField.text;

    }

    public void UpdatePositionY()
    {
        InputField inputField = positionYText.GetComponent<InputField>();
        string newText = inputField.text;

    }

    public void UpdatePositionZ()
    {
        InputField inputField = positionZText.GetComponent<InputField>();
        string newText = inputField.text;

    }

    public void UpdateRotationX()
    {
        InputField inputField = rotationXText.GetComponent<InputField>();
        string newText = inputField.text;

    }

    public void UpdateRotationY()
    {
        InputField inputField = rotationYText.GetComponent<InputField>();
        string newText = inputField.text;

    }

    public void UpdateRotationZ()
    {
        InputField inputField = rotationZText.GetComponent<InputField>();
        string newText = inputField.text;

    }
}
