using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UiManager : MonoBehaviour
{
    public static UiManager instance;
    public GameObject winPanel, PP, inGame,failpanel;
    public TextMeshProUGUI levelnumbertext;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        levelnumbertext.text = "Level - " + PlayerPrefs.GetInt("levelnumber", 1);
    }

    void Update()
    {
        
    }
    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void Next()
    {
        if (PlayerPrefs.GetInt("level", 1) >= SceneManager.sceneCountInBuildSettings - 1)
        {
            SceneManager.LoadScene(Random.Range(0, SceneManager.sceneCountInBuildSettings - 1));
            PlayerPrefs.SetInt("level", (PlayerPrefs.GetInt("level", 1) + 1));
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            PlayerPrefs.SetInt("level", (PlayerPrefs.GetInt("level", 1) + 1));
        }
        PlayerPrefs.SetInt("levelnumber", PlayerPrefs.GetInt("levelnumber", 1) + 1);
    }
    public void PrivacyPolicy()
    {
       
    }
}
