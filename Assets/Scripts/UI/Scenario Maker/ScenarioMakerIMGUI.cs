using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioMakerIMGUI : MonoBehaviour
{
    int buttonWidth = 150;
    int buttonHeight = 20;
    bool menuColapsed = false;

    [SerializeField] GameObject playerTankPrefabs;
    [SerializeField] GameObject[] terrainPrefabs;
    [SerializeField] GameObject[] enemyTankPrefabs;
    GameObject loadedTerrainPrefab;
    GameObject selectedTankPrefab;
    GameObject[] enemyTanks;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnGUI()
    {
        if (!menuColapsed)
        {
            RenderMenuWindow(1);
        }
        else
        {
            if (GUI.Button(new Rect(0, 0, buttonWidth, buttonHeight), "Menu"))
            {
                menuColapsed = !menuColapsed;
            }
        }
    }

    bool dropdownTerrainGUI = false;
    bool dropdownTankGUI = false;

    void RenderMenuWindow(int windowID)
    {
        GUI.BeginGroup(new Rect(0, 0, buttonWidth * 2, 160));
        
        if (GUI.Button(new Rect(0, 0, buttonWidth, buttonHeight), "Close Menu"))
        {
            menuColapsed = !menuColapsed;
        }
        DropdownTerrainGUI();
        DropdownTankGUI();

        GUI.EndGroup();
    }

    void TurnOffAllDropdowns()
    {
        dropdownTerrainGUI = false;
        dropdownTankGUI = false;
    }

    void DropdownTankGUI()
    {
        if (!dropdownTankGUI)
        {
            if (GUI.Button(new Rect(0, buttonHeight * 2, buttonWidth, buttonHeight), "Enemy Tanks"))
            {
                TurnOffAllDropdowns();
                dropdownTankGUI = true;
            }
        }
        else
        {
            if (GUI.Button(new Rect(0, buttonHeight * 2, buttonWidth, buttonHeight), "Enemy Tank >"))
            {
                dropdownTankGUI = false;
            }

            for (int i = 0; i < enemyTankPrefabs.Length; i++)
            {
                string tankName = enemyTankPrefabs[i].name;
                if (selectedTankPrefab != null && selectedTankPrefab.name == enemyTankPrefabs[i].name)
                {
                    tankName += " X";
                }

                if (GUI.Button(new Rect(buttonWidth, buttonHeight * 2 * (i + 1), buttonWidth, buttonHeight), tankName))
                {
                    selectedTankPrefab = enemyTankPrefabs[i];
                }
            }
        }
    }


    void DropdownTerrainGUI()
    {
        if (!dropdownTerrainGUI)
        {
            if (GUI.Button(new Rect(0, buttonHeight, buttonWidth, buttonHeight), "Terrain"))
            {
                TurnOffAllDropdowns();
                dropdownTerrainGUI = true;
            }
        }
        else
        {
            if (GUI.Button(new Rect(0, buttonHeight, buttonWidth, buttonHeight), "Terrain >"))
            {
                dropdownTerrainGUI = false;
            }

            for (int i = 0; i < terrainPrefabs.Length; i++)
            {
                string terrainName = terrainPrefabs[i].name;
                if (loadedTerrainPrefab != null && loadedTerrainPrefab.name == terrainPrefabs[i].name + "(Clone)")
                {
                    terrainName += " X";
                }
                if (GUI.Button(new Rect(buttonWidth, buttonHeight * 1 * (i + 1), buttonWidth, buttonHeight), terrainName))
                {
                    // destroys current terrain and loads new one
                    if (loadedTerrainPrefab != null)
                    {
                        Destroy(loadedTerrainPrefab);
                        loadedTerrainPrefab = null;
                    }

                    if (!(loadedTerrainPrefab != null && loadedTerrainPrefab.name == terrainPrefabs[i].name + "(Clone)"))
                    {
                        loadedTerrainPrefab = Instantiate(terrainPrefabs[i]);
                        loadedTerrainPrefab.transform.position = new Vector3(0, 0, 0);
                    }
                }
            }
        }
    }
}
