using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Player Input")]
    private PlayerInputControls controls; // This is the object that listens for inputs at the hardware level
    private Vector2 movementInput;

    [Header("Object References")]
    [SerializeField] private BaseMovement baseMovement;
    [SerializeField] private PlayerFirer firer;


    private GameManager gameManager = GameManager._instance;
    private void Awake()
    {
        controls = new PlayerInputControls();
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
        //controls.UI.Disable();


        switch (mapName)
        {
            case "Player":
                // We need to enable our "Player" action map so Unity will listen for our player input.
                controls.Player.Enable();

                // Since we are switching into gameplay, we will no longer need control of our mouse cursor
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                break;

            //case "UI":
            //    // We need to enable our "UI" action map so Unity will listen for our UI input.
            //    controls.UI.Enable();

            //    // Since we are switching into a UI, we will also need control of our mouse cursor
            //    Cursor.visible = true;
            //    Cursor.lockState = CursorLockMode.None;
            //    break;

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
        controls.Player.Fire.performed += StopFireAction;
    }
    private void UnsubscribeInputActions()
    {
        // It is important to unbind and actions that we bind
        // when our object is destroyed, or this can cause issues
        controls.Player.Move.started -= MoveAction;
        controls.Player.Move.performed -= MoveAction;
        controls.Player.Move.canceled -= MoveAction;
    }
    private void MoveAction(InputAction.CallbackContext context)
    {
        // Read in the Vector2 of our player input.
        movementInput = context.ReadValue<Vector2>();

        Debug.Log("The player is trying to move: " + movementInput);

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
        gameManager.ChangeXP(amount);
    }

    private void OnTriggerEnter(Collider other)
    {
        XP_Orb orb = other.GetComponent<XP_Orb>();
        if (orb != null)
        {
            CollectExperience(orb.GetXP());
        }
    }
}
