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
    void OnGUI()
    {
        // 1. Create a custom style based on Unity's default Box
        GUIStyle style = new GUIStyle(GUI.skin.box);
        style.fontSize = 40;                       // Make the text BIG
        style.fontStyle = FontStyle.Bold;          // Make it BOLD
        style.normal.textColor = Color.green;      // Give it a strong color (e.g., green or yellow)
        style.alignment = TextAnchor.MiddleCenter; // Center the text inside the box

        // 2. Draw the box. Rect(X, Y, Width, Height)
        GUI.Box(new Rect(20, 20, 300, 80), "Float: " + rb.linearVelocity.magnitude.ToString("F2"), style);
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