using UnityEngine;

public class VelocityVisualizer : MonoBehaviour
{
    [SerializeField] private float lengthMultiplier = 0.1f;
    [SerializeField] private float minSpeedToShow = 0.5f;
    [SerializeField] private float tailWidth = 0.05f;
    [SerializeField] private float headWidth = 0.2f;
    [SerializeField] private float headRatio = 0.35f;

    private Rigidbody2D rb;
    private LineRenderer lr;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        lr = GetComponent<LineRenderer>();

        lr.positionCount = 2;

        AnimationCurve arrowCurve = new AnimationCurve();
        float neckStart = 1f - headRatio;

        arrowCurve.AddKey(new Keyframe(0f, tailWidth));
        arrowCurve.AddKey(new Keyframe(neckStart, tailWidth));

        arrowCurve.AddKey(new Keyframe(neckStart + 0.001f, headWidth));
        arrowCurve.AddKey(new Keyframe(1f, 0f));

        lr.widthCurve = arrowCurve;

        lr.numCapVertices = 0;
        lr.numCornerVertices = 0;
    }

    void Update()
    {
        Vector2 velocity = rb.linearVelocity;
        float speed = velocity.magnitude;

        if (speed < minSpeedToShow)
        {
            lr.enabled = false;
            return;
        }

        lr.enabled = true;
        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, transform.position + (Vector3)(velocity * lengthMultiplier));
    }
}