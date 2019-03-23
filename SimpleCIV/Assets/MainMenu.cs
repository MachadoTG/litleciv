using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Canvas newGame;

    public void NewGame()
    {
        newGame.gameObject.SetActive(true);
    }
    public void Options()
    {
    }
    public void Exit()
    {
        Application.Quit();
    }
}
