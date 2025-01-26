using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class GameManager : MonoBehaviour
{

    public static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }


    public int BubbleResource = 0;
    public int getCurrXP() { return BubbleResource; }

    [Tooltip("the amount of xp the player needs to reach level 1")]
    [SerializeField] private int InitThreshold = 100;
    [Tooltip("the rate at which future levels will need more xp, currently calculated as a power")]
    [SerializeField] private int LevelMult = 2;

    [SerializeField] private UI_UpgradeToggler upgradeMenu;

    private int CurrLevel = 1;
    public int getCurrLevel()
    {
        return Mathf.FloorToInt(Mathf.Log((BubbleResource / InitThreshold) + 1 ,LevelMult) +1);
    }
    public void LoseGame()
    {
        CurrentGameState = GameState.GameOver;
    }
    public enum GameState
    {
        Playing, Paused, LevelUp, GameOver, StartMenu
    }
    public GameState CurrentGameState { get; private set; }

    // Start is called before the first frame update
    void Start()
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


    //TODO: PAUSE MENU

    public void UnPause()
    {
        Time.timeScale = 1;
        CurrentGameState = GameState.Playing;
    }


    public void LevelUp()
    {
        CurrLevel++;
        upgradeMenu.ToggleUPGRADEMenu();
        Time.timeScale = 0;
        CurrentGameState = GameState.LevelUp;
        Debug.Log("you levelled up!");

    }
    public void LevelDown()
    {
        CurrLevel--;
        Debug.Log("you levelled down! pay more attention!");
    }

    public void ChangeBubbles(int amount)
    {
        BubbleResource += amount;
        if (BubbleResource >= InitThreshold*(Mathf.Pow(LevelMult, CurrLevel )-1))
        {
            LevelUp();
        }
        
        if (BubbleResource < InitThreshold * (Mathf.Pow(LevelMult, CurrLevel-1) - 1))
        {
            LevelDown();
        }

    }

}
