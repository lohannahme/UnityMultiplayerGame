using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class CinemachinePOVExtension : CinemachineExtension
{
    [SerializeField] private InputManager inputManager;
    private Vector3 startingRotation;

    private float _clampAngle = 80f;
    [SerializeField]
    private float _speed = 10f;

    protected override void Awake()
    {
        
        base.Awake();
    }

    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (vcam.Follow)
        {
            if (stage == CinemachineCore.Stage.Aim)
            {
                if (startingRotation == null) startingRotation = transform.localRotation.eulerAngles;


                //Vector2 deltaInput = inputManager.GetMouseDelta();
                Vector2 deltaInput = Mouse.current.delta.ReadValue();
                startingRotation.x += deltaInput.x * _speed * Time.deltaTime;
                startingRotation.y += deltaInput.y * _speed * Time.deltaTime;

                startingRotation.y = Mathf.Clamp(startingRotation.y, -_clampAngle, _clampAngle);
                state.RawOrientation = Quaternion.Euler(-startingRotation.y, startingRotation.x, 0f);

            }
        }
    }

}
