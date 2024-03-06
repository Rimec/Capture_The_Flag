using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuControl: MonoBehaviour
{
    public GameObject creditsMenu;

    public void StartGame(string num)
    {
        SceneManager.LoadScene(num);
    }
    public void SetCredits()
    {
        creditsMenu.SetActive(!creditsMenu.activeSelf);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
