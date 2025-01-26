using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class GameManager : MonoBehaviour
{


    public static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }


    private int BubbleResource = 0;
    public int getCurrXP() { return BubbleResource; }

    [Tooltip("the amount of xp the player needs to reach level 1")]
    [SerializeField] private int InitThreshold = 100;
    [Tooltip("the rate at which future levels will need more xp, currently calculated as a power")]
    [SerializeField] private int LevelMult = 2;


    private int CurrLevel = 1;
    public int getCurrLevel()
    {
        return Mathf.FloorToInt(Mathf.Log((BubbleResource / InitThreshold) + 1 ,LevelMult) +1);
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

    public void LevelUp()
    {
        Debug.Log("you levelled up (still need to implement that tho lol");
    }

    public void ChangeBubbles(int amount)
    {
        BubbleResource += amount;
        if (BubbleResource >= 100*(Mathf.Pow(LevelMult, CurrLevel -1)-1))
        {
            LevelUp();
        }

    }

}
