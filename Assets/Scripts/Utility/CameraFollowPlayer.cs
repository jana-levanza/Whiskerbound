using Unity.Cinemachine;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    private CinemachineCamera vcam;

    private void Awake()
    {
        vcam = GetComponent<CinemachineCamera>();
    }

    private void Update()
    {
        if (vcam != null && vcam.Follow == null)
            FindAndFollowTarget();
    }

    private void FindAndFollowTarget()
    {
        GameObject cutsceneActor = GameObject.FindGameObjectWithTag("Actors");
        if (cutsceneActor != null)
        {
            SetCameraTarget(cutsceneActor.transform);
            return;
        }

        SwitchToRealPlayer();
    }

    public void SwitchToRealPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        SetCameraTarget(player.transform);
    }

    private void SetCameraTarget(Transform target)
    {
        vcam.Follow = target;
        Vector3 instantPos = target.position;
        instantPos.z = transform.position.z;
        transform.position = instantPos;
        vcam.PreviousStateIsValid = false;
    }
}