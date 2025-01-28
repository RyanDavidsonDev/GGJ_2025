using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Ui_mainmenu : MonoBehaviour
{
    
    // function for UI menu to switch to game scene
    public void StartGame(){
        SceneManager.LoadSceneAsync(1);
        // array starting at 0, where 0 is the main menu scene
        // 1 is the main game scene
        // 2, 3 ... are allocated for other scenes we want
    }
    // function for quitting game
    public void QuitGame(){
        Application.Quit();
    }

    public void Retry(){
        SceneManager.LoadSceneAsync(1);
    }
    // other Scene switches/stuff goes here, 
    // refer to this for UI-based scene switching:
    // https://www.youtube.com/watch?v=DX7HyN7oJjE
}
