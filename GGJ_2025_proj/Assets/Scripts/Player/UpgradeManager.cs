using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.iOS;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{

    [SerializeField] public int BubbleResource { get; private set; } = 30;


    [Tooltip("the amount of xp the player needs to reach level 1")]
    [SerializeField] private int InitThreshold = 100;
    [Tooltip("the rate at which future levels will need more xp, currently calculated as a power")]
    [SerializeField] private int LevelMult = 2;


    [SerializeField] private Stack<GameObject> Upgrades;
    [SerializeField] private PlayerFirer firer;
    public int CurrLevel { get; private set; } = 1;

    GameManager gm = GameManager.Instance;

    public enum GunName{bubbleGun, DoubleShot, Shotgun, ChainShotgun, SMG, LMG, Minigun, GrenadeLauncher, MachineGrenadeLauncher, RPG, MLRS};

    [System.Serializable]
    public class GunStruct{
        [Tooltip("The player facing name of the gun - we should be using this in code for direct comparisons. Instead use e_name")]
        [SerializeField] public string name;//player facing
        [Tooltip("the internal facing name of the gun")]
        [SerializeField] public GunName e_name;
        [Tooltip("prefab reference from which the gun will load")]
        [SerializeField] GameObject gunPrefab;
        //(probably not) Track track

        public GunStruct(string name, GunName e_name, GameObject gunPrefab) { 
            this.name = name;
            this.gunPrefab = gunPrefab;
            this.e_name = e_name;
        }
    }


    [SerializeField] public List<GunStruct> A_track;
    [SerializeField] public List<GunStruct> B_track;
    [SerializeField] public List<GunStruct> E_track;
    //  new List<string>(){"test", "tes2"};

    public struct UpgradeStruct
    {
        GunName enumName;
        string name;
        GameObject gun;
        
    }
    enum Track {e, b ,a}

    public enum TrackName{A_track, B_track, E_track}
    // public struct TrackStruct{
        
    //     public TrackName trackName;
    //     public DoublyLinkedList<String> listOfUpgrades;//new 

    //     public UpgradeStruct currentLeaf;

    //     public TrackStruct(TrackName name, List<string> list)
    //     {
    //         trackName = name;
    //         listOfUpgrades = list;
    //         currentLeaf = listOfUpgrades.Head.Value;
    //     }
    // }





    // private DoublyLinkedList<UpgradeStruct> Etrack_list = 
    //     new DoublyLinkedList<string>(new List<string> { 
    //     new UpgradeStruct(), "MGL", "RPG", "MLRS" 
    //     });

    
    // private TrackStruct A_Track_UpgradeRef = new TrackStruct(TrackName.A_track, 
    //         new DoublyLinkedList<string>(new List<string> { "SMG", "LMG", "Minigun" })
    // );
    // private DoublyLinkedList<string> Btrack_list = 
    private DoublyLinkedList<string> Atrack_list = new DoublyLinkedList<string>(new List<string> { "DBpistol", "Shotgun", "Chain" });
    private DoublyLinkedListNode<string> Etrack_head;
    private DoublyLinkedListNode<string> Btrack_head;
    private DoublyLinkedListNode<string> Atrack_head;


    public static UpgradeManager _instance;
    public static UpgradeManager Instance { get { return _instance; } }
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
        
    //private List<string> Etrack_list = new List<string> { "GL","MGL", "RPG", "MLRS" };
    //private List<string> Btrack_list = new List<string> { "SMG", "LMG", "Minigun" };
    //private List<string> Atrack_list = new List<string> { "DBpistol","Shotgun", "Chain" };

        
        // A_track= new List<GunStruct>(){
        //     new GunStruct("Bubble Gun", GunName.bubbleGun,
        // Resources.Load<GameObject>("../../Prefabs/Barrels/Bubble Gun Line/1_BubbleGun")),
        //     new GunStruct("Double Barrel Pistol", GunName.DoubleShot,
        // Resources.Load<GameObject>("../../Prefabs/Barrels/Bubble Gun Line/2_Double Bubble Trouble.prefab")),
        //     new GunStruct("Shotgun",  GunName.Shotgun,
        // Resources.Load<GameObject>("../../Prefabs/Barrels/Bubble Gun Line/1_BubbleGun")),
        //     new GunStruct("Chain Shotgun",  GunName.ChainShotgun,
        // Resources.Load<GameObject>("../../Prefabs/Barrels/Bubble Gun Line/1_BubbleGun")),
        // };

        Debug.Log("size of atrack" + A_track.Count);
    }



    public void LevelUp()
    {
        CurrLevel++;
        Time.timeScale = 0;
        gm.LevelUp();
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
        
        if (BubbleResource < InitThreshold*(Mathf.Pow(LevelMult, CurrLevel-1) - 1))
        {
            LevelDown();
        }
    }

 public void Upgrade(string button)
    {

        Debug.Log("Player is upgrading" + button);

        // if(button == "BUTTON_ATRACK")
        // {

        //     A_Track_GameObjs[0].gameObject.SetActive(true);
        //     var gun_controller_a = A_Track_GameObjs[0].GetComponent<FiringController>();
        //     firer.Guns.Add(gun_controller_a);
        //     A_Track_GameObjs.RemoveAt(0);
        //     Debug.Log("upgrade A track from playercont");
            
        // }
        // if (button == "BUTTON_BTRACK")
        // {
        //     B_Track_GameObjs[0].gameObject.SetActive(true);
        //     var gun_controller_b = B_Track_GameObjs[0].GetComponent<FiringController>();
        //     firer.Guns.Add(gun_controller_b);
        //     B_Track_GameObjs.RemoveAt(0);
        //     Debug.Log("upgrade B track from playercont");
        // }
        // if (button == "BUTTON_ETRACK")
        // {
        //     E_Track_GameObjs[0].SetActive(true);
        //     var gun_controller_e = E_Track_GameObjs[0].GetComponent<FiringController>();
        //     firer.Guns.Add(gun_controller_e);
        //     E_Track_GameObjs.RemoveAt(0);
        //     Debug.Log("upgrade E track from playercont");
        // }

        // else
        // {
        //     Debug.Log("There is no upgrade");
        // }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
