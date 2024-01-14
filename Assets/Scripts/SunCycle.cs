using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunCycle : MonoBehaviour
{
    [SerializeField] public Light sun;

    [SerializeField, Range(0, 24)] private float timeOfDay;

    [SerializeField] private float sunRotationSpeed;

    [Header("LightingPreset")]
    [SerializeField] private Gradient skyColor;
    [SerializeField] private Gradient equatorColor;
    [SerializeField] private Gradient sunColor;

    private void Update() {
        timeOfDay += Time.deltaTime * sunRotationSpeed;
        if (timeOfDay > 24) {
            timeOfDay = 0;
        }
        UpdateSunRotation();
        UpdateLighting();
    }

    private void OnValidate() {
        UpdateSunRotation();
        UpdateLighting();
    }

    private void UpdateSunRotation() {
        float sunRotation = Mathf.Lerp(-90, 270, timeOfDay / 24);
        sun.transform.rotation = Quaternion.Euler(sunRotation, sun.transform.rotation.y, sun.transform.rotation.z);
    }

    private void UpdateLighting() {
        float timeFration = timeOfDay / 24;
        RenderSettings.ambientEquatorColor = equatorColor.Evaluate(timeFration);
        RenderSettings.ambientSkyColor = equatorColor.Evaluate(timeFration);
        sun.color = sunColor.Evaluate(timeFration);
    }
}
