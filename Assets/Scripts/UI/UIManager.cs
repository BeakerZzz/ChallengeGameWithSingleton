using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    static UIManager instance;

    public TextMeshProUGUI orbText,timeText,deathText,gameOverText;

    Canvas canvas;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

    }

    private void Start()
    {
        canvas = GetComponent<Canvas>();
    }

    public static void UpdateOrbUI(int orbCount)
    {
        instance.orbText.text = orbCount.ToString();
    }

    public static void UpdateDeathUI(int deathCount)
    {
        instance.deathText.text = deathCount.ToString();
    }

    public static void UpdateTimeUI(float time)
    {
        instance.timeText.text = time.ToString("00");
    }
    public static void DisplayGameOverUI()
    {
        instance.gameOverText.enabled = true;
    }

    public static void HideGameOverUI()
    {
        instance.gameOverText.enabled = false;
    }

    public static void hideUI()
    {
        instance.canvas.enabled = false;
    }
}
