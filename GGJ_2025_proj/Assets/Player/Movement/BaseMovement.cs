using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseMovement : MonoBehaviour
{
    #region Variables

    [Header("Character Ground Movement")]

    public float currentMaxSpeed = 4f;
    [Header("Character - Character Input")]
    [Tooltip("The 2D movement input from the controller.")]
    protected Vector2 movementInput;
    [Tooltip("The 3D direction in which this character should move.")]
    protected Vector3 movementDirection;

    [Header("Character - Component/Object References")]
    [SerializeField] protected Animator animator;
    [SerializeField] protected CapsuleCollider capsuleCollider;
    [SerializeField] protected Rigidbody rigidbody;
    [SerializeField] protected AudioSource audioSource;
    [SerializeField] protected Transform characterModel;

    #endregion

    #region Unity Functions



    #endregion

    #region Custom Functions

    #region Input

    // Virtual functions can have base implementations
    public virtual void SetMovementInput(Vector2 moveInput)
    {
        // Set the value of this Movement script's movement input
        movementInput = moveInput;
    }

    #endregion

    #region Movement

    // All child classes MUST (mandatory) implement this function, thus abstract
    protected abstract void MoveCharacter();

    // All child classes CAN (optional) implement this function, thus virtual
    protected virtual void RotateCharacter()
    {
        // Do nothing
    }

}
#endregion
#endregion