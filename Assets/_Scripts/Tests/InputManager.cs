using UnityEngine;

public class InputManager : MonoBehaviour
{
    private static InputManager _instance;

    public static InputManager Instance
    {
        get
        {
            return _instance;
        }
    }

    private PlayerInputActions playerInputActions;

    private void Awake()
    {
        if(_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }// remover o singleton e colocar o scriptInput dentro do jogador

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
