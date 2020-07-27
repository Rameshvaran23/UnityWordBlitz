using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class StartMenu : MonoBehaviour
{
    public void StartGame()// increase the buid value depend on the build setting by +1
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }
    public void BackMenu()// the code for the game back button function 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);

    }
    public void MainMenu()// the code for the game back button function where change the screen to main menu  
    {
        SceneManager.LoadScene(0);

    }
    public void RestartScene()// the code for the game restart button function, it load the scene Game scene
    {
        SceneManager.LoadScene(1);

    }
    public void WordFallScene()// Word Fall Game Scene
    {
        SceneManager.LoadScene(2);

    }
    public void HardModeScene()// Wordsearch Hard mode scene load 
    {
        SceneManager.LoadScene(3);

    }
    public void QuitGame()
    {
        Debug.Log("Quiting Game");
        Application.Quit();
    }
}
