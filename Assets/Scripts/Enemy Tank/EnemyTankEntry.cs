using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyTankEntry
{
    public string tankPrefabName;
    public List<TargetLocation> targetLocations;
    public Color color = Color.white;
}
