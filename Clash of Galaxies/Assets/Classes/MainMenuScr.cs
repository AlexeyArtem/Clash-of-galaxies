using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScr : MonoBehaviour
{
    public GameObject BackMenuPanel, GameRulesPanel, SettingsPanel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NewGameClick() 
    {
        SceneManager.LoadScene("GameScene");
    }

    public void ExitGameClick() 
    {
        Application.Quit();
    }

    public void SettingsClick() 
    {
        BackMenuPanel.SetActive(true);
        SettingsPanel.SetActive(true);
    }

    public void RulesClick() 
    {
        BackMenuPanel.SetActive(true);
        GameRulesPanel.SetActive(true);
    }

    public void BackMenuClick() 
    {
        GameRulesPanel.SetActive(false);
        SettingsPanel.SetActive(false);
        BackMenuPanel.SetActive(false);
    }
}
