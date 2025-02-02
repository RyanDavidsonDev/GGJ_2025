using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    public static PlayerController playerCont;



    public void setPC(PlayerController pc)
    {
        playerCont = pc;
    }


    public PlayerController getPC()
    {
        return playerCont;
    }




    [SerializeField] private UI_UpgradeToggler upgradeMenu;


    // public int getCurrLevel()
    // {
    //     return Mathf.FloorToInt(Mathf.Log((BubbleResource / InitThreshold) + 1 ,LevelMult) +1);
    // }
    public void LoseGame()
    {
        CurrentGameState = GameState.GameOver;
        SceneManager.LoadScene("GameOver");
    }
    public enum GameState
    {
        Playing, Paused, LevelUp, GameOver, StartMenu
    }
    public GameState CurrentGameState { get; private set; }

    // Start is called before the first frame update
    void Awake()
    {
        //is this the first time we've created this singleton
        if (_instance == null)
        {
            //we're the first gameManager, so assign ourselves to this instance
            _instance = this;

            //keep ourselves between levels
            //DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            //if there's another one, then destroy this one
            Destroy(this.gameObject);

        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void LevelUp()
    {
        upgradeMenu.ToggleUPGRADEMenu();
        Time.timeScale = 0;
        CurrentGameState = GameState.LevelUp;
        Debug.Log("you levelled up (this was called from the gameManager)!");        

    }

    //TODO: PAUSE MENU

    public void UnPause()
    {
        Time.timeScale = 1;
        CurrentGameState = GameState.Playing;
    }


}
