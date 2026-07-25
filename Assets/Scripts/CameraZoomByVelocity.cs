using UnityEngine;
using Unity.Cinemachine;

public class CameraZoomByVelocity : MonoBehaviour
{
    public CinemachineCamera vCam;
    public Rigidbody2D targetRb;

    public float minZoom = 5f;
    public float maxZoom = 8f;
    public float zoomSmoothTime = 0.5f;
    public float minVelocity = 0.5f;
    public float maxVelocity = 10f;
    private float zoomVelocity;

    private void FixedUpdate()
    {
        if (vCam == null || targetRb == null) return;

        float currentSpeed = targetRb.linearVelocity.magnitude;

        float speedFactor = Mathf.InverseLerp(minVelocity, maxVelocity, currentSpeed);

        float targetOrthographicSize = Mathf.Lerp(minZoom, maxZoom, speedFactor);

        float currentSize = vCam.Lens.OrthographicSize;

        float newSize = Mathf.SmoothDamp(
            currentSize,
            targetOrthographicSize,
            ref zoomVelocity,
            zoomSmoothTime
        );

        vCam.Lens.OrthographicSize = newSize;
    }
}
