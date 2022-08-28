using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainSceneUI : MonoBehaviour
{
    [SerializeField] private GameObject gamePauseUI;
    private bool isPaused = false;

    public void Pause()
    {
        if (!isPaused)
        {
            isPaused = true;
            GameManager.Instance.canRaycastGameObject = false;
            gamePauseUI.SetActive(true);
        }
        else
        {
            isPaused = false;
            GameManager.Instance.canRaycastGameObject = true;
            gamePauseUI.SetActive(false);
        }
    }

    public void Cancel()
    {
        isPaused = false;
        GameManager.Instance.canRaycastGameObject = true;
        gamePauseUI.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void Return()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Title");
    }
}
