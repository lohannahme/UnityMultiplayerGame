using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerMoveController : NetworkBehaviour
{
    [SerializeField] private float _movementSpeed = 5f;
    [SerializeField] private CharacterController _controller;
    [SerializeField] private Transform _cinemachineTransform;

    private Vector2 previousInput;

    private Controls controls;

    private Controls Controls
    {
        get
        {
            if (controls != null) { return controls; }
            return controls = new Controls();
        }
    }

    public override void OnStartAuthority()
    {
        enabled = true;

        Controls.PlayerM.Move.performed += ctx => SetMovement(ctx.ReadValue<Vector2>());
        Controls.PlayerM.Move.canceled += ctx => ResetMovement();
    }

    [ClientCallback]
    private void OnEnable() => Controls.Enable();

    [ClientCallback]
    private void OnDisable() => Controls.Disable();

    [ClientCallback]
    private void Update() => Move();

    [Client]
    private void SetMovement(Vector2 movement) => previousInput = movement;

    [Client]
    private void ResetMovement() => previousInput = Vector2.zero;

    [Client]
    private void Move()
    {

        Vector3 move = new Vector3(previousInput.x, 0, previousInput.y);
        move = _cinemachineTransform.forward * move.z + _cinemachineTransform.right * move.x;
        move.y = 0f;
        _controller.Move(move * Time.deltaTime * _movementSpeed);

    }

}
