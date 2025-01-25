using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : BaseMovement
{
    [Header("Player - Ground Movement")]
    [SerializeField] private float accelerationRate = 60f;  // The speed at which this character accelerates in m/s
    [SerializeField] private float decelerationRate = 30f;  // The speed at which this character decelerates in m/s
    [SerializeField] private float maxWalkSpeed = 4f;


    [Header("Player - Rotation")]
    [SerializeField] private float groundRotationRate = 10f; // The rate at which the player rotates (when grounded)


    [Header("Player - Ground Check")]
    [SerializeField] private float groundCheckDistance = 0.1f;  // The distance below the player to check for ground
    [SerializeField] LayerMask environmentLayerMask;            // Which layers are considered to be the environment
    private bool wasGroundedLastFrame = false;                  // Denotes whether you were on the ground last frame or not
    private bool isGrounded = false;


    private void FixedUpdate()
    {
        // Check if we are on the ground.
        CheckIsGrounded();

        // Move our character each frame.
        MoveCharacter();

        // Every physics update, make sure that
        // we are not exceeding our current
        // maximum allowed velocity.
        LimitVelocity();

    }

    private void Update()
    {
        

        // Rotate our character to face towards our input.
        RotateCharacter();

        
        
    }

    protected override void RotateCharacter()
    {
        // If we're trying to move
        if (movementDirection != Vector3.zero)
        {
            // Rotate our player mesh by aligning their forward vector with the movement direction
            if (isGrounded)
                characterModel.forward = Vector3.Slerp(characterModel.forward, movementDirection.normalized, groundRotationRate * Time.deltaTime);
        }
    }
    protected override void MoveCharacter()
    {
        // We only need to apply forces to move
        // if we are trying to move. Thus, if we
        // aren't inputting anything, don't apply
        // any forces (they'd be 0 anyways).
        if (movementDirection != Vector3.zero)
        {
            // If we are on the ground we want to move according to our movespeed.
            if (isGrounded)
            {
                // Apply our movement Force.
                rigidbody.AddForce(movementDirection * accelerationRate, ForceMode.Acceleration);
            }
            // Otherwise, if we are in the air we want to
            // move according to our movespeed modified by
            // our airControlMultiplier.
           
        }
        // If we're not trying to move but we're on the ground
        else if (isGrounded)
        {
            // And if we're still moving, let's decelerate
            Vector3 currentVelocity = GetHorizontalRBVelocity();
            if (currentVelocity.magnitude > 0.5f)
            {
                // Use an opposing acceleration force to slow down gradually.
                Vector3 counteractDirection = currentVelocity.normalized * -1f;
                rigidbody.AddForce(counteractDirection * decelerationRate, ForceMode.Acceleration);
            }
        }
    }
    private Vector3 GetHorizontalRBVelocity()
    {
        return new Vector3(rigidbody.velocity.x, 0f, rigidbody.velocity.z);
    }

    private void CheckIsGrounded()
    {
        // Record the grounded status from the previous check
        wasGroundedLastFrame = isGrounded;

        // Calculate the center of the spheres on the bottom and top of the capsule for the Capsule Cast
        RaycastHit hit;
        Vector3 p1 = transform.position + (Vector3.up * capsuleCollider.radius);
        Vector3 p2 = transform.position + (Vector3.up * (capsuleCollider.bounds.size.y - capsuleCollider.radius));

        isGrounded = Physics.CapsuleCast(p1,                        // Center of first circle 
                                         p2,                        // Center of second circle 
                                         capsuleCollider.radius,    // Radius of capsule 
                                         Vector3.down,              // Direction of cast 
                                         out hit,                   // RaycastHit that receives information about hit
                                         groundCheckDistance,       // Length of cast
                                         environmentLayerMask);     // LayerMask to specify hittable layers
        // second ground check for when overlapping ground
        if (!isGrounded)
        {
            // we're on the ground if there's at least one collider around the bottom of our capsule
            Collider[] colliders = Physics.OverlapSphere(p1, capsuleCollider.radius + groundCheckDistance, environmentLayerMask);
            isGrounded = (colliders.Length > 0);
        }

        // Tell the animator we're on the ground
        //animator.SetBool("IsGrounded", isGrounded);

    }
    private void LimitVelocity()
    {
        // Limit Horizontal Velocity
        // If our current velocity is greater than our maximum allowed velocity...
        Vector3 currentVelocity = GetHorizontalRBVelocity();
        // Note: Square root is an expensive operation! Comparing the squared distances is cheaper.
        if (currentVelocity.sqrMagnitude > (currentMaxSpeed * currentMaxSpeed))
        {
            // Use an impulse force to counteract our velocity to slow down to max allowed velocity.
            Vector3 counteractDirection = currentVelocity.normalized * -1f;
            float counteractAmount = currentVelocity.magnitude - currentMaxSpeed;
            rigidbody.AddForce(counteractDirection * counteractAmount, ForceMode.VelocityChange);
        }

        // Limit Vertical Velocity
        // If our current speed is greater than our max speed
       
    }



}
