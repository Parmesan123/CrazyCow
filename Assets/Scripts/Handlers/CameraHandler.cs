using Cinemachine;
using Entities;
using UnityEngine;
using Zenject;

public class CameraHandler : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _basicCamera;
    
    [Inject]
    private void Construct(PlayerMovement playerMovement)
    {
        _basicCamera.Follow = playerMovement.transform;
        _basicCamera.LookAt = playerMovement.transform;
    }
}