using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class gameScene : MonoBehaviour {

    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;
    public GameObject help;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
                help.SetActive(false);
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void Back()
    {
        help.SetActive(false);
        pauseMenuUI.SetActive(true);
    }
    public void RestartGame()
    {
        SceneManager.LoadScene("Game");
        Time.timeScale = 1f;
        pauseMenuUI.SetActive(false);
    }

    public void Help()
    {
        help.SetActive(true);
        pauseMenuUI.SetActive(false);
    }

    public void ExitGame()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }
}
