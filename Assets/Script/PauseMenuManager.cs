using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PauseMenuManager : MonoBehaviour
{

    public bool gameIsPaused = false;
    public GameObject pauseMenu;

    public static PauseMenuManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            return;
        }
        instance = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameIsPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void MainMenu()
    {
        if (GameOverManager.instance.CheckVictoryCondition()) 
        {
            StaeNameValue.coin += 20;
            PlayerPrefs.SetInt("coin", StaeNameValue.coin);
            PlayerPrefs.Save();
        }
        else 
        {
            StaeNameValue.coin -= 20;
            PlayerPrefs.SetInt("coin", StaeNameValue.coin);
            PlayerPrefs.Save();

        }
       
        ResumeGame();
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadLobby()
    {

        StaeNameValue.coin = GameOverManager.instance.coin;
        PlayerPrefs.SetInt("coin", GameOverManager.instance.coin);
        PlayerPrefs.Save();
        ResumeGame();
        SceneManager.LoadScene("MainScene");
    }

    public void PauseGame()
    {
        
        gameIsPaused = true;
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
        // L'appel � Pause sera fait via l'�v�nement d'animation
    }

    public void ResumeGame()
    {
        gameIsPaused = false;
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);

        // L'appel � Resume sera fait via l'�v�nement d'animation
    }
}
