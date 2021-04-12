using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseCanvas : GameHUDWidget
{
    public void ResumeGame()
    {
        PauseManager.Instance.UnPauseGame();
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("Menu Scene");
    }

    public void QuitApplication()
    {
        Application.Quit();
    }
}
