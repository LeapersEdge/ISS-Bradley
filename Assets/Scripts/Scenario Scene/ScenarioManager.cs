using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioManager : MonoBehaviour
{
    ScenarioData scenarioData;

    [Header("Prefabs")]
    [SerializeField] private GameObject playerTank;
    [SerializeField] private GameObject[] enemyTanks;
    [SerializeField] private GameObject[] maps;
    [Header("UI")]
    [SerializeField] private GameObject pauseMenu;

    // Start is called before the first frame update
    void Start()
    {
        Camera.main.gameObject.SetActive(false);

        // check if scenario_text is empty
        string scenario_text = PlayerPrefs.GetString("scenario_text", "");
        if (scenario_text != "")
        {
            scenarioData = JsonUtility.FromJson<ScenarioData>(scenario_text);
        }
        else
        {
            // load scenario from scenario index
            int scenario_index = PlayerPrefs.GetInt("scenario", 1);
            string scenario_json = PlayerPrefs.GetString("scenario" + scenario_index.ToString());
            scenarioData = JsonUtility.FromJson<ScenarioData>(scenario_json);
        }

        // load scenario data into game
        // 1st: load map
        // 2nd: load player tank
        // 3rd: load enemy tanks

        // 1st: load map
        string map_name = scenarioData.map_name;
        // remove (Clone) from map_name
        map_name = map_name.Substring(0, map_name.Length - 7);
        for (int i = 0; i < maps.Length; i++)
        {
            if (maps[i].name == map_name)
            {
                Instantiate(maps[i]);
                break;
            }
        }

        // 2nd: load player tank
        GameObject player_tank = Instantiate(playerTank);
        if (scenarioData.player_tank.position == Vector3.zero)
        {
            scenarioData.player_tank.position = new Vector3(10, 10, 10);
        }

        player_tank.transform.position = scenarioData.player_tank.position;
        player_tank.transform.rotation = scenarioData.player_tank.rotation;

        // 3rd: load enemy tanks
        for (int i = 0; i < scenarioData.enemy_tanks.Count; i++)
        {
            EnemyTankEntry enemy_tank_entry = scenarioData.enemy_tanks[i];
            string enemy_tank_name = enemy_tank_entry.tankPrefabName;
            GameObject enemy_tank = null;
            for (int j = 0; j < enemyTanks.Length; j++)
            {
                if (enemyTanks[j].name == enemy_tank_name)
                {
                    enemy_tank = Instantiate(enemyTanks[j]);
                    break;
                }
            }
            enemy_tank.transform.position = enemy_tank_entry.targetLocations[0].location.position;
            enemy_tank.transform.rotation = enemy_tank_entry.targetLocations[0].location.rotation;
            // TODO: uncommend when enemy tank controller is implemented
            enemy_tank.GetComponent<EnemyTankController>().targetLocations = enemy_tank_entry.targetLocations.ToArray();
            enemy_tank.GetComponent<EnemyTankController>().enabled = true;
        }

        if (map_name == "houses_terrain")
        {
            RenderSettings.fog = true;
            // reduce render distance
            QualitySettings.lodBias = 1.0f;
        }
        else
        {
            RenderSettings.fog = false;
            // increase render distance
            QualitySettings.lodBias = 8.0f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenu.SetActive(!pauseMenu.activeSelf);
            if (pauseMenu.activeSelf)
            {
                Time.timeScale = 0.0f;
            }
            else
            {
                Time.timeScale = 1.0f;
            }
        }
    }

    public void ResumeButton() 
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void ExitScenarioButton() 
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
    
}
