using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScr : MonoBehaviour
{
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

    }

    public void RulesClick() 
    {

    }
}
