using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuControl: MonoBehaviour
{
    public GameObject creditsMenu;
    public GameObject lobbyList;
    public GameObject mainMenu;
    public GameObject lobbyScreen;
    public void StartGame(string num)
    {
        SceneManager.LoadScene(num);
    }
    public void SetCredits()
    {
        creditsMenu.SetActive(!creditsMenu.activeSelf);
        
    }
    public void SetLobbysList()
    {
        lobbyList.SetActive(!lobbyList.activeSelf);
        mainMenu.SetActive(!mainMenu.activeSelf);
    }
    public void SetLobbyScreen()
    {
        lobbyScreen.SetActive(!lobbyScreen.activeSelf);
        lobbyList.SetActive(!lobbyList.activeSelf);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
