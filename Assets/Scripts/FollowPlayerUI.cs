using UnityEngine;

public class FollowPlayerUI : MonoBehaviour
{
    public Transform target;

    public Vector3 offset = new Vector3(0f, 1.5f, 0f);

    [Range(0.01f, 0.5f)]
    public float smoothTime = 0.1f;
    private Vector3 velocity = Vector3.zero;

    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void LateUpdate()
    {
        if (target != null && cam != null)
        {
            Vector3 targetWorldPosition = target.position + offset;
            Vector3 screenPosition = cam.WorldToScreenPoint(targetWorldPosition);
            transform.position = Vector3.SmoothDamp(transform.position, screenPosition, ref velocity, smoothTime);
            transform.rotation = Quaternion.identity;
        }
    }
}
