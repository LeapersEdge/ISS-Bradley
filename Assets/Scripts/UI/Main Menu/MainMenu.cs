using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject scenarioMenu;
    [SerializeField] private GameObject scenarioList;
    [SerializeField] private InputField scenarioText;
    [Header("Prefabs")]
    [SerializeField] private GameObject scenarioButton;

    // Start is called before the first frame update
    void Start()
    {
        mainMenu.SetActive(true);
        scenarioMenu.SetActive(false);
        PlayerPrefs.SetString("scenario_text", "");
        AddAllToScenarioList();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddAllToScenarioList()
    {
        int max_index = PlayerPrefs.GetInt("index", 0);
        for (int i = 1; i <= max_index; i++)
        {
            GameObject button = Instantiate(scenarioButton);
            button.transform.SetParent(scenarioList.transform);
            button.GetComponentInChildren<Text>().text = "Scenario " + i.ToString();
            AddListenersToButton(button, i);
        }
    }

    public void AddListenersToButton(GameObject button, int index)
    {
        button.GetComponent<Button>().onClick.AddListener(() => LoadScenario(index));
    }

    public void LoadScenario(int index) {
        PlayerPrefs.SetInt("scenario", index);
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    public String LoadScenarioFromText()
    {
        // check if string text is a valid json of ScenarioData class
        // if not, return
        try
        {
            ScenarioData scenario = JsonUtility.FromJson<ScenarioData>(scenarioText.text);
        }
        catch (Exception e)
        {
            Debug.Log(e);
            scenarioText.text = "invalid json";
            return "invalid json";
        }

        PlayerPrefs.SetString("scenario_text", scenarioText.text);
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);

        return "success";
    }

    public void PlayScenario() {
        mainMenu.SetActive(false);
        scenarioMenu.SetActive(true);
    }

    public void MakeScenario() {
        UnityEngine.SceneManagement.SceneManager.LoadScene(2);
    }

    public void QuitGame() {
        Debug.Log("Quit");
        Application.Quit();
    }
}
