using UnityEngine;

public class InputManager : MonoBehaviour
{
    private static InputManager _instance;

   

    private PlayerInputActions playerInputActions;

    private void Awake()
    {
       

        playerInputActions = new PlayerInputActions();

        Cursor.visible = false;
    }

    private void OnEnable()
    {
        playerInputActions.Enable();
    }

    private void OnDisable()
    {
        playerInputActions.Disable();
    }

    public Vector2 GetPlayerMovement()
    {
        return playerInputActions.Player.Movement.ReadValue<Vector2>();
    }

    public Vector2 GetMouseDelta()
    {
        return playerInputActions.Player.Look.ReadValue<Vector2>();
    }

    public bool IsJumping()
    {
        return playerInputActions.Player.Jump.triggered; //atualizar para c# events.
    }

    public bool PassCucumber()
    {
        return playerInputActions.Player.CucumberPass.triggered;
    }
}
