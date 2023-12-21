using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioMakerUI : MonoBehaviour
{
    [SerializeField] GameObject playerTankPrefabs;
    [SerializeField] GameObject[] terrainPrefabs;
    [SerializeField] GameObject[] enemyTankPrefabs;
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
}
