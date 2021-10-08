using UnityEngine;
using Mirror;
using Cinemachine;
using UnityEngine.InputSystem;

public class NetworkCameraController : NetworkBehaviour
{
    [Header("Camera")]
    [SerializeField] private Vector2 _maxFollowOffset = new Vector2(-1f, 6f);
    [SerializeField] private Vector2 _cameraVelocity = new Vector2(4f, .25f);
    [SerializeField] private Transform _playerTransform = null;
    [SerializeField] private CinemachineVirtualCamera _virtualCamera = null;
    [SerializeField] private CinemachinePOVExtension _povExtension;

    private Controls controls;

    private Controls Controls
    {
        get
        {
            if(controls != null) { return controls; }
            return controls = new Controls();
        }
    }


    public override void OnStartAuthority()
    {
       
        _virtualCamera.gameObject.SetActive(true);

        _povExtension.enabled = true;

        enabled = true;

    }

    [ClientCallback]
    private void OnEnable() => Controls.Enable();

    [ClientCallback]
    private void OnDisable() => Controls.Disable();
   

   



}
