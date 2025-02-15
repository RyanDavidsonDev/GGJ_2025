using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using static GameManager;


public class PlayerController : MonoBehaviour
{
    [Header("Player Input")]
    private PlayerInputControls controls; // This is the object that listens for inputs at the hardware level
    private Vector2 movementInput;

    [Header("Object References")]
    [SerializeField] private BaseMovement baseMovement;
    [SerializeField] private PlayerFirer firer;

    // [SerializeField] private List<GameObject> A_track;
    // [SerializeField] private List<GameObject> B_track;
    // [SerializeField] private List<GameObject> E_track;

    [SerializeField] private Stack<GameObject> Upgrades;

    [SerializeField] public UpgradeManager upgradeManager;

    private bool isPaused = false;

   // private GameManager gameManager = GameManager._instance;

    private void Awake()
    {
        controls = new PlayerInputControls();
        
    }

    private void Start()
    {
        GameManager.Instance.setPC(this);
        GameManager.Instance.startPlay.AddListener(ReceiveResume);//call receivePause when gm broadcasts startPlay
    }



    public void OnEnable()
    {
        SubscribeInputActions();

        SwitchActionMap("Player");
    }
    public void OnDisable()
    {
        UnsubscribeInputActions();

        SwitchActionMap();
    }
    private void SwitchActionMap(string mapName = "")
    {
        controls.Player.Disable();
        controls.UI.Disable();


        switch (mapName)
        {
            case "Player":
                // We need to enable our "Player" action map so Unity will listen for our player input.
                controls.Player.Enable();
                controls.UI.Disable();

                // Since we are switching into gameplay, we will no longer need control of our mouse cursor
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                break;

            case "UI":
                //We need to enable our "UI" action map so Unity will listen for our UI input.
                controls.UI.Enable();
                controls.Player.Disable();

                //Since we are switching into a UI, we will also need control of our mouse cursor
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                break;

            default:
                // Show the mouse cursor
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                break;
        }
    }

    private void SubscribeInputActions()
    {
        controls.Player.Move.started += MoveAction;
        controls.Player.Move.performed += MoveAction;
        controls.Player.Move.canceled += MoveAction;

        controls.Player.Fire.started += StartFireAction;
        controls.Player.Fire.canceled += StopFireAction;

        controls.Player.TogglePause.started += PauseAction;



        controls.UI.TogglePause.started += PauseAction;
    }
    private void UnsubscribeInputActions()
    {
        // It is important to unbind and actions that we bind
        // when our object is destroyed, or this can cause issues
        controls.Player.Move.started -= MoveAction;
        controls.Player.Move.performed -= MoveAction;
        controls.Player.Move.canceled -= MoveAction;

        controls.Player.Fire.started -= StartFireAction;
        controls.Player.Fire.canceled -= StopFireAction;

        controls.Player.TogglePause.started -= PauseAction;


        controls.UI.TogglePause.started -= PauseAction;
    }
    private void MoveAction(InputAction.CallbackContext context)
    {
        // Read in the Vector2 of our player input.
        movementInput = context.ReadValue<Vector2>();

        //Debug.Log("The player is trying to move: " + movementInput);

        baseMovement.SetMovementInput(movementInput);

    }

    private void StartFireAction(InputAction.CallbackContext context)
    {
        firer.BroadcastStartFire();
    }
    private void StopFireAction(InputAction.CallbackContext context)
    {
        firer.BroadcastStopFire();
    }

    private void CollectExperience(int amount)
    {
        GameManager.Instance.ChangeBubbles(amount);
        //Debug.Log($"amount: {amount}");
    }


    //private List<string> Etrack_list = new List<string> { "MGL", "RPG", "MLRS" };
    //private List<string> Btrack_list = new List<string> { "", "" };
    //private List<string> Atrack_list = new List<string> { "", "" };
    
    public void Upgrade(string button)
    {

    }

    private void PauseAction(InputAction.CallbackContext context)
    {
        Debug.Log("AAAAAAAAA");
        if (GameManager.Instance.CurrentGameState == GameState.Playing)
        {
            Debug.Log("pause in player cont");
            SwitchActionMap("UI");
            GameManager.Instance.SetGamePause();
        }
        else if (GameManager.Instance.CurrentGameState == GameState.Paused)
        {
            Debug.Log("unpause in player cont");
            GameManager.Instance.SetPlaying();
        } else
        {
            Debug.LogWarning("playercontroller.pauseaction() game is neither playing nor paused. I'm not sure how you got here");
            //probably should have GM keep track of most recent game state
        }
        
    }

    private void ReceiveResume()
    {
        SwitchActionMap("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        XP_Orb orb = other.GetComponent<XP_Orb>();
        if (orb != null)
        {
            CollectExperience(orb.GetXP());
            Destroy(orb.gameObject);
        }
    }
}
