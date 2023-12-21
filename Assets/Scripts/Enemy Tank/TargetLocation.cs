using UnityEngine;

[System.Serializable]
public class TargetLocation
{
    public Transform location;
    public bool agressive = false;
    public float aimingAccuracyPercentageX = 0.5f;
    public float aimingAccuracyPercentageY = 0.5f;

    // if extra time implement commented
    // bool aimbot = false;
}
