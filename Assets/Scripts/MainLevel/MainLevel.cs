using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainLevel : MonoBehaviour
{
    Canvas mainMenu;
    Canvas levelMenu;
    private void Start()
    {
        mainMenu = GameObject.Find("MainMenu").GetComponent<Canvas>();
        levelMenu = GameObject.Find("LevelMenu").GetComponent<Canvas>();
        levelMenu.enabled = false;
        mainMenu.enabled = true;
    }
    public void PlayOnClick()
    {
        SceneManager.LoadScene(0);
    }

    public void LevelOnClick()
    {
        mainMenu.enabled = false;
        levelMenu.enabled = true;
    }

    public void ExitOnClick()
    {
        Application.Quit();

    }

    public void Level1OnClick()
    {
        SceneManager.LoadScene(0);
    }

    public void Level2OnClick()
    {
        SceneManager.LoadScene(1);
    }

    public void Level3OnClick()
    {
        SceneManager.LoadScene(2);
    }
    public void BackOnClick()
    {
        GameObject.Find("MainMenu").SetActive(true);
        GameObject.Find("LevelMenu").SetActive(false);
    }
}
