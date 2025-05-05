using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    static GameManager instance;
    SceneFader sceneFader;
    List<Orb> orbs;

    [HideInInspector]
    public Door door;
    //last scene
    public  int lastSceneIndex;

    public float gameTimeMax;
    private float gameTime;
    public int deathNum;
    public bool isGameOver = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }else
        {
            Destroy(gameObject);
            return;
        }

        orbs = new List<Orb>();
    }

    private void Start()
    {
        gameTime = gameTimeMax;
    }

    private void Update()
    {
        if(isGameOver)
            return;
        gameTime -= Time.deltaTime;
        UIManager.UpdateTimeUI(gameTime);
        if (gameTime <= 0)
        {
            gameTime = 0;
            TimeOut();
        }
    }


    public static void RegisterDoor(Door door)
    {
        instance.door = door;
    }
    public static void RegisterSceneFader(SceneFader sceneFader)
    {
        instance.sceneFader = sceneFader;
    }

    public static void RegisterOrb(Orb orb)
    {
        if (instance == null)
            return;
        if(!instance.orbs.Contains(orb))
        {
            instance.orbs.Add(orb);
        }
        UIManager.UpdateOrbUI(instance.orbs.Count);
    }

    public static void PlayerGrabbedOrb(Orb orb)
    {
        if(instance.orbs.Contains(orb))
        {
            instance.orbs.Remove(orb);
            UIManager.UpdateOrbUI(instance.orbs.Count);
        }
        
    }
    public static void TimeOut()
    {
        instance.isGameOver = true;
        instance.sceneFader.FadeOut();
        UIManager.DisplayGameOverUI();
        instance.Invoke("RestartScene", 3f);
        
    }

    public static void PlayerDied()
    {
        instance.sceneFader.FadeOut();
        instance.deathNum++;
        UIManager.UpdateDeathUI(instance.deathNum);
        instance.Invoke("RestartScene", 1f);
    }
    public static void OpenDoor()
    {
        instance.door.Open();
    }
    public static void PlayerWon()
    {
        instance.sceneFader.FadeOut();
        instance.isGameOver = true;
        AudioManager.PlayerWonAudio();
        if(instance.lastSceneIndex == 4)
            SceneManager.LoadScene(0);
        else
            SceneManager.LoadScene(instance.lastSceneIndex);
        instance.gameTime = instance.gameTimeMax;
        instance.isGameOver = false;
        if(instance.lastSceneIndex == 4)
        {
            UIManager.HideUI();
            Destroy(instance.gameObject);
        }
        instance.lastSceneIndex++;
    }
    public static bool IsGameOver()
    {
        return instance.isGameOver;
    }
    private void RestartScene()
    {
        instance.orbs.Clear();
        instance.isGameOver = false;
        instance.gameTime = gameTimeMax;
        UIManager.HideGameOverUI();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
