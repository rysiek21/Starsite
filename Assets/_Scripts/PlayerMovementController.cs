using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerMovementController : NetworkBehaviour
{
    Modifiers modifiers;

    private CharacterController characterController;
    public float standardSpeed = 6.0f;
    public float standardAirSpeed = 3.0f;
    public float sneakDivider = 2; 
    public float jumpHeight = 6.0f;
    public float gravity = 9.81f;
    private Vector3 moveDirection = Vector3.zero;

    void Start()
    {
        if (!isLocalPlayer) return;
        modifiers = GetComponent<Modifiers>();
        characterController = GetComponent<CharacterController>();
    }

    void FixedUpdate()
    {
        if (!isLocalPlayer) return;
        
        if(characterController.isGrounded)
        {
            float speed = standardSpeed;
            if (Input.GetAxis("Sneak") != 0)
            {
                speed /= sneakDivider;
            }

            moveDirection = transform.forward * Input.GetAxis("Vertical") * (speed + modifiers.movementSpeedModifier) + transform.right * Input.GetAxis("Horizontal") * (speed + modifiers.movementSpeedModifier);
            if (Input.GetButton("Jump"))
            {
                moveDirection.y = jumpHeight;
            }
        }
        else
        {
            float speed = standardAirSpeed;
            if (Input.GetAxis("Sneak") != 0)
            {
                speed /= sneakDivider;
            }
            moveDirection = transform.forward * Input.GetAxis("Vertical") * (speed + modifiers.movementSpeedModifier) + transform.up * moveDirection.y + transform.right * Input.GetAxis("Horizontal") * (speed + modifiers.movementSpeedModifier);
        }
        
        moveDirection.y -= gravity * Time.deltaTime;
        characterController.Move(moveDirection * Time.deltaTime);
    }
}
