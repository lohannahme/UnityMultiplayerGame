using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;
using Mirror;

public class CinemachinePOVExtension : CinemachineExtension
{
    
    private Vector3 startingRotation;

    private float _clampAngle = 80f;
    [SerializeField]
    private float _speed = 1f;

    private Vector2 deltaInput;

    [SerializeField]
    private Transform _playerRotation;

    [SerializeField]
    private float _playerRotationSpeed = 0.06f;

    private Controls controls;

    private Controls Controls
    {
        get
        {
            if (controls != null) { return controls; }
            return controls = new Controls();
        }
    }

    
    protected override void Awake()
    {
        
        base.Awake();
    }

    [ClientCallback]
    protected override void OnEnable()
    {
        Controls.Enable();
        Controls.PlayerM.Look.performed += ctx => HandleLookInput(ctx);
        Controls.PlayerM.Look.canceled += ctx => HandleLookInput(ctx);

    }
    [ClientCallback]
    private void OnDisable()
    {
        Controls.Disable();
        Controls.PlayerM.Look.performed -= ctx => HandleLookInput(ctx);
        Controls.PlayerM.Look.canceled -= ctx => HandleLookInput(ctx);
    }




    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (vcam.Follow)
        {
            if (stage == CinemachineCore.Stage.Aim)
            {
                if (startingRotation == null) startingRotation = transform.localRotation.eulerAngles;

                startingRotation.x += deltaInput.x * _speed * Time.deltaTime;
                startingRotation.y += deltaInput.y * _speed * Time.deltaTime;

                _playerRotation.Rotate(Vector3.up * deltaInput.x * _playerRotationSpeed);
                

                startingRotation.y = Mathf.Clamp(startingRotation.y, -_clampAngle, _clampAngle);
                state.RawOrientation = Quaternion.Euler(-startingRotation.y, startingRotation.x, 0f);

            }
        }
    }

    private void HandleLookInput(InputAction.CallbackContext ctx)
    {
        deltaInput = ctx.ReadValue<Vector2>();
        
    }

}
