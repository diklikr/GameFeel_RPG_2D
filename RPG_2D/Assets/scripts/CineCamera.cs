using Unity.Cinemachine;
using UnityEngine;

public class CineCamera : MonoBehaviour
{
    public CinemachineVirtualCameraBase vcam;

    public Transform player;

    void Start()
    {
        // find player by tag if not assigned
        if (player == null)
        {
            var go = GameObject.FindGameObjectWithTag("Player");
            if (go != null) player = go.transform;
        }

        // try explicit vcam first (if assigned in inspector)
        if (vcam == null)
        {
            // try to get the Brain's active virtual camera from Camera.main
            if (Camera.main != null)
            {
                var brain = Camera.main.GetComponent<CinemachineBrain>();
                if (brain != null && brain.ActiveVirtualCamera != null)
                {
                    // ActiveVirtualCamera is an ICinemachineCamera; try to cast to CinemachineVirtualCameraBase
                    vcam = brain.ActiveVirtualCamera as CinemachineVirtualCameraBase;
                }
            }
        }

        // fallback: find any CinemachineVirtualCameraBase in the scene
        if (vcam == null)
            vcam = Object.FindFirstObjectByType<CinemachineVirtualCameraBase>();

        // assign Follow if possible
        if (vcam != null && player != null)
        {
            vcam.Follow = player;
            Debug.Log($"CinemachineFollowSetter: assigned Follow = {player.name} on vcam = {vcam.name}");
        }
        else
        {
            if (vcam == null) Debug.LogWarning("CinemachineFollowSetter: no virtual camera found (Brain active vcam or scene vcam).");
            if (player == null) Debug.LogWarning("CinemachineFollowSetter: no player Transform found (tag 'Player' fallback failed).");
        }
    }
}