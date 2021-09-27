using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputSys : MonoBehaviour
{
    //[SerializeField]
    public PlayerInputActions playerInputActions;
    private InputAction movement;
    private Rigidbody rigidbody;


    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }
    void Awake()
    {
        playerInputActions = new PlayerInputActions();
    }


    private void Jump()
    {
        Debug.Log("pulou");
    }

    private void OnEnable()
    {
        movement = playerInputActions.Player.Movement;
        movement.Enable();

        playerInputActions.Player.Jump.performed += DoJump;
        playerInputActions.Player.Jump.Enable();
    }

    private void DoJump(InputAction.CallbackContext obj)
    {
        Debug.Log("jump");
        rigidbody.AddForce(new Vector3(0, 2, 0), ForceMode.Impulse);
    }

    private void OnDisable()
    {
        movement.Disable();
        playerInputActions.Player.Jump.Disable();
    }

    private void FixedUpdate()
    {
        Debug.Log("mov values " + movement.ReadValue<Vector2>());
    }
}
