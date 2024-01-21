using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyTankEntry
{
    public GameObject tankPrefab;
    public List<TargetLocation> targetLocations = new List<TargetLocation> ();
}
