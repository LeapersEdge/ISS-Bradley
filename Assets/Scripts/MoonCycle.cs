using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonCycle : MonoBehaviour
{
    [SerializeField] public Light moon;

    [SerializeField, Range(0, 24)] private float timeOfDay;

    [SerializeField] private float moonRotationSpeed;

    [Header("LightingPreset")]
    [SerializeField] private Gradient skyColor;
    [SerializeField] private Gradient equatorColor;
    [SerializeField] private Gradient moonColor;

    private void Update() {
        timeOfDay += Time.deltaTime * moonRotationSpeed;
        if (timeOfDay > 24) {
            timeOfDay = 0;
        }
        UpdateMoonRotation();
        UpdateLighting();
    }

    private void OnValidate() {
        UpdateMoonRotation();
        UpdateLighting();
    }

    private void UpdateMoonRotation() {
        float moonRotation = Mathf.Lerp(90, 450, timeOfDay / 24);
        moon.transform.rotation = Quaternion.Euler(moonRotation, moon.transform.rotation.y, moon.transform.rotation.z);
    }

    private void UpdateLighting() {
        float timeFration = timeOfDay / 24;
        RenderSettings.ambientEquatorColor = equatorColor.Evaluate(timeFration);
        RenderSettings.ambientSkyColor = equatorColor.Evaluate(timeFration);
        moon.color = moonColor.Evaluate(timeFration);
    }
}
