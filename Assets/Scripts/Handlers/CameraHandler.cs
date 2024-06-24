using Cinemachine;
using Entities;
using UnityEngine;
using Zenject;

namespace Handlers
{
    public class CameraHandler : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera _basicCamera;
    
        [Inject]
        private void Construct(PlayerBehavior player)
        {
            _basicCamera.Follow = player.transform;
            _basicCamera.LookAt = player.transform;
        }
    }
}