using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Vector3 offset;
    private float smoothTime = 0.25f;
    private Vector3 velocity = Vector3.zero;

    [SerializeField] private Transform target;
    private Transform originaltarget;

    private void Start()
    {
        originaltarget = target;
        InputManager.OnCameraResetClicked += resetCamera;

    }

    private void Update()
    {
        if (target == null)
            return;

        Vector3 targetPosition = target.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }

    public void setTarget(Transform newtarget)
    {
        target = newtarget;
    }

    public void resetCamera()
    {
        target = originaltarget;
    }
}