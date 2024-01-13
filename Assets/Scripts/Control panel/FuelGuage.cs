using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuelGuagde : MonoBehaviour {

    private const float MAX_FUEL_ANGLE = 25;
    private const float ZERO_FUEL_ANGLE = 155;

    private Transform needleTransform;
    private Transform fuelLevelLabelTemplateTransform;

    private float fuelMax;
    private float fuel;

    private void Awake() {
        needleTransform = transform.Find("needle");
        fuelLevelLabelTemplateTransform = transform.Find("fuelLevelLabelTemplate");
        fuelLevelLabelTemplateTransform.gameObject.SetActive(false);

        fuelMax = 100f;
        fuel = fuelMax;

        CreateFuelLevelLabels();
    }

    private void Update() {
        HandlePlayerInput();

        needleTransform.eulerAngles = new Vector3(0, 0, GetFuelLevelRotation());
    }

    private void HandlePlayerInput() {
        if (Input.GetKey(KeyCode.UpArrow)){
            float deceleration = 1f;
            fuel -= deceleration * Time.deltaTime;
        }

        fuel = Mathf.Clamp(fuel, 0f, fuelMax);
    }

    private void CreateFuelLevelLabels() {
        int labelAmount = 10;
        float totalAngleSize = ZERO_FUEL_ANGLE - MAX_FUEL_ANGLE;

        for (int i = 0; i <= labelAmount; i++) {
            Transform fuelLevelLabelTransform = Instantiate(fuelLevelLabelTemplateTransform, transform);
            float labelFuelLevelNormalized = (float)i / labelAmount;
            float fuelLevelLabelAngle = ZERO_FUEL_ANGLE - labelFuelLevelNormalized * totalAngleSize;
            fuelLevelLabelTransform.eulerAngles = new Vector3(0, 0, fuelLevelLabelAngle);
            if (fuelLevelLabelAngle == ZERO_FUEL_ANGLE){
                fuelLevelLabelTransform.Find("fuelLevelText").GetComponent<Text>().text = "E";
            }
            else if (fuelLevelLabelAngle == MAX_FUEL_ANGLE){
                fuelLevelLabelTransform.Find("fuelLevelText").GetComponent<Text>().text = "F";
            }
            else{
                fuelLevelLabelTransform.Find("fuelLevelText").GetComponent<Text>().text = "";
            }
            //fuelLevelLabelTransform.Find("fuelLevelText").GetComponent<Text>().text = Mathf.RoundToInt(labelFuelLevelNormalized * fuelMax).ToString();
            fuelLevelLabelTransform.Find("fuelLevelText").eulerAngles = Vector3.zero;
            fuelLevelLabelTransform.gameObject.SetActive(true);
        }

        needleTransform.SetAsLastSibling();
    }

    private float GetFuelLevelRotation() {
        float totalAngleSize = ZERO_FUEL_ANGLE - MAX_FUEL_ANGLE; 

        float fuelLevelNormalized = fuel / fuelMax;

        return ZERO_FUEL_ANGLE - fuelLevelNormalized * totalAngleSize;
    }
}
