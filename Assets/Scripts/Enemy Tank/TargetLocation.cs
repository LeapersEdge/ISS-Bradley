using UnityEngine;

[System.Serializable]
public class TargetLocation
{
    public TransformTargetLocation location = new TransformTargetLocation();
    public bool agressive = false;
    public float aimingAccuracyPercentageX = 0.5f;
    public float aimingAccuracyPercentageY = 0.5f;
    public float cooldownAfterAction = 0.0f;

    // if extra time implement commented
    // bool aimbot = false;
}

public class TransformTargetLocation
{
    public Vector3 position = Vector3.zero;
    public Quaternion rotation = Quaternion.identity;
}