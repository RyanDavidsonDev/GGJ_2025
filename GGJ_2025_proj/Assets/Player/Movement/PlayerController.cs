using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;
public class PlayerController : MonoBehaviour
{
    [Header("Player Input")]
    private PlayerInputControls playerInputControls; // This is the object that listens for inputs at the hardware level
    private Vector2 movementInput;

    [Header("Object References")]
    [SerializeField] private BaseMovement baseMovement;
    [SerializeField] private CinemachineCamera


    void Update()
    {
        
    }

    public void OnEnable() 
    {
       
        
    }
    public void OnDisable()
    {
        
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        var movement = context.ReadValue<Vector2>();
    }
}
