using System.Collections.Generic;

[System.Serializable]
public class ScenarioData
{
    public List<EnemyTankEntry> enemy_tanks = new List<EnemyTankEntry>();
    public TransformTargetLocation player_tank = new TransformTargetLocation();
    public string map_name = "terrain1";    
}