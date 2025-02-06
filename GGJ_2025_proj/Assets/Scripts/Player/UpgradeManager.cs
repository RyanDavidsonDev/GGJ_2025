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
    public enum TrackName{A_track, B_track, E_track};

    [System.Serializable]
    public class GunStruct{
        [Tooltip("The player facing name of the gun - we should be using this in code for direct comparisons. Instead use e_name")]
        [SerializeField] public string name;//player facing
        [Tooltip("the internal facing name of the gun")]
        [SerializeField] public GunName e_name;
        [Tooltip("prefab reference from which the gun will load")]
        [SerializeField] public GameObject gunPrefab;
        //(probably not) Track track
        [Tooltip("which upgrade track this weapon is under - is")]
        [SerializeField] public TrackName track;
        public GunStruct(string name, GunName e_name, GameObject gunPrefab, TrackName track) { 

            this.name = name;
            this.gunPrefab = gunPrefab;
            this.e_name = e_name;
            this.track = track;
        }
    }


    [SerializeField] public List<GunStruct> A_track;
    [SerializeField] public List<GunStruct> B_track;
    [SerializeField] public List<GunStruct> E_track;


//note - the first index of B and E will be null because the player does not start with them 
    [Tooltip("indicates which weapon in the A Track is the highest one that is active")]
    public int A_Track_index = 0;
    [Tooltip("indicates which weapon in the B Track is the highest one that is active")]
    public int B_Track_index = 0;
    [Tooltip("indicates which weapon in the E Track is the highest one that is active")]
    public int E_Track_index =0;
    //  new List<string>(){"test", "tes2"};


//attempts at setting syntax for tracks and gun structs
#region cut
    // public enum TrackName{A_track, B_track, E_track}
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


    // private DoublyLinkedList<string> Atrack_list = new DoublyLinkedList<string>(new List<string> { "DBpistol", "Shotgun", "Chain" });
    // private DoublyLinkedListNode<string> Etrack_head;
    // private DoublyLinkedListNode<string> Btrack_head;
    // private DoublyLinkedListNode<string> Atrack_head;

#endregion cut

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
        
        #region cut
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
       #endregion cut

        Debug.Log("size of atrack" + A_track.Count);

    }


    public void Upgrade(TrackName track)
    {

        Debug.Log("Player is upgrading" + track.ToString());

        switch(track){
            case TrackName.A_track:
                UpgradeHelper(A_track, A_Track_index);
                break;
            case TrackName.B_track:
                UpgradeHelper(B_track, B_Track_index);
                break;
            case TrackName.E_track:
                UpgradeHelper(B_track, B_Track_index);
                break;
        }
    //old version of how we upgraded the player's gun
    #region cut
            // case TrackName
        // if(track == TrackName.A_track)
        // {
        //     if(A_Track_index+1>=A_track.Count){
        //         Debug.LogWarning("UpgradeManager.Upgrade() was called with a track that's already been maxxed out. You should handle this case at some point. probably by catching it in the UI");
        //     }
        //     A_Track_index++;

        //     A_track[A_Track_index].gunPrefab.SetActive(true);
        //     var gun_controller_a = A_track[A_Track_index].gunPrefab.GetComponent<FiringController>();
        //     firer.Guns.Add(gun_controller_a);
        //     // A_track.RemoveAt(0); //I don't believe we want to remove this - instead keep a ref in case we need to deactivate, and instead increment the index we're looking at
        //     Debug.Log("upgrade A track from playercont");
            
        // } else if (button == "BUTTON_BTRACK")
        // {
        //     B_track[0].gameObject.SetActive(true);
        //     var gun_controller_b = B_track[0].GetComponent<FiringController>();
        //     firer.Guns.Add(gun_controller_b);
        //     B_track.RemoveAt(0);
        //     Debug.Log("upgrade B track from playercont");
        // }
        // if (button == "BUTTON_ETRACK")
        // {
        //     E_track[0].SetActive(true);
        //     var gun_controller_e = E_track[0].GetComponent<FiringController>();
        //     firer.Guns.Add(gun_controller_e);
        //     E_track.RemoveAt(0);
        //     Debug.Log("upgrade E track from playercont");
        // }

        // else
        // {
        //     Debug.Log("There is no upgrade");
        // }
#endregion cut
    }

    private void UpgradeHelper(List<GunStruct> track, int index){

        if(index+1>=track.Count){
            Debug.LogWarning("UpgradeManager.Upgrade() was called with a track that's already been maxxed out. You should have handled this case in the UI");
        }
        index++;

        track[index].gunPrefab.SetActive(true);
        var gun_controller = track[index].gunPrefab.GetComponentInChildren<FiringController>();
        firer.Guns.Add(gun_controller);
        // A_track.RemoveAt(0); //I don't believe we want to remove this - instead keep a ref in case we need to deactivate, and instead increment the index we're looking at
        Debug.Log("upgrade A track from playercont");
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


    // Update is called once per frame
    void Update()
    {
        
    }
}
