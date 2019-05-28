using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EscapeMenu : MonoBehaviour
{
    public GameObject optionMenu;
    public static bool GameIsPaused = true;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            optionMenu.gameObject.SetActive(!optionMenu.gameObject.activeSelf);
            GameIsPaused = !GameIsPaused;
            if (GameIsPaused)
            {
                Time.timeScale = 1.0f;
            }
            else
            {
                Time.timeScale = 0f;
            }
        }
    }
    public void Resume()
    {
        optionMenu.gameObject.SetActive(!optionMenu.gameObject.activeSelf);
        GameIsPaused = !GameIsPaused;
        if (GameIsPaused)
        {
            Time.timeScale = 1.0f;
        }
        else
        {
            Time.timeScale = 0f;
        }
    }
    public void Escape()
    {
        Debug.Log("Quit Game!");
        Application.Quit();
    }
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
