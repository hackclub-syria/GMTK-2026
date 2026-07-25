using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class cursor_script : MonoBehaviour
{
    [Header("Cursor & Rhythm")]
    [SerializeField] private Transform T_mask;
    [SerializeField] private float tempo = 120;
    [SerializeField] private int[] pattern = new int[] { 4, 2, 4, 2 };

    [Header("Countdown Animation")]
    [SerializeField, Range(1f, 30f)] private float lerpSpeed = 15f;

    [Header("Time Dilation")]
    [SerializeField, Range(0.02f, 0.5f)] private float slowScale = 0.1f;
    [SerializeField] private float enterDuration = 0.15f;
    [SerializeField] private float exitDuration = 0.06f;

    [Header("Explosion (Runs on Unscaled Time)")]
    [SerializeField] private float explosionRadius = 2.5f;
    [SerializeField] private float explosionForce = 12f;

    [SerializeField] private AnimationCurve falloff = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
    [SerializeField] private LayerMask affectedLayers;

    [SerializeField] private float maxLaunchSpeed = 60f;

    [Header("Feedback Hooks")]
    public UnityEvent<Vector3> onDetonate;

    private Camera Cam;
    private float interval;
    private float timer = 0f;

    private int ind_pattern = 0;
    private int currentBeat;

    private float baseFixedDeltaTime;

    private float targetMaskLocalY;

    private readonly Collider2D[] hitBuffer = new Collider2D[32];

    void Start()
    {
        Cam = Camera.main;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;

        interval = 60f / tempo;

        if (pattern == null || pattern.Length == 0)
        {
            pattern = new int[] { 4 };
        }

        currentBeat = pattern[0];
        targetMaskLocalY = T_mask.localPosition.y;

        baseFixedDeltaTime = Time.fixedDeltaTime;
    }

    void Update()
    {
        Handle_TimeDilation();
        Replace_cursor();
        Handle_countdown();

        float newY = Mathf.Lerp(T_mask.localPosition.y, targetMaskLocalY, Time.unscaledDeltaTime * lerpSpeed);
        T_mask.localPosition = new Vector3(T_mask.localPosition.x, newY, T_mask.localPosition.z);
    }

    void Handle_TimeDilation()
    {
        bool held = Keyboard.current != null && Keyboard.current.spaceKey.isPressed;

        float target = held ? slowScale : 1f;
        float duration = held ? enterDuration : exitDuration;

        float rate = 1f / Mathf.Max(duration, 0.0001f);

        Time.timeScale = Mathf.MoveTowards(Time.timeScale, target, rate * Time.unscaledDeltaTime);
        Time.fixedDeltaTime = baseFixedDeltaTime * Mathf.Max(Time.timeScale, 0.02f);
    }

    void Handle_countdown()
    {
        timer += Time.unscaledDeltaTime;

        if (timer >= interval)
        {
            timer -= interval;
            currentBeat--;
            int length = pattern[ind_pattern];

            if (currentBeat == length - 1)
            {
                targetMaskLocalY = 0.1f;
            }

            targetMaskLocalY -= (T_mask.localScale.y / length);

            if (currentBeat > 0)
            {
                Debug.Log(currentBeat);
            }
            else
            {
                Detonate();

                ind_pattern = (ind_pattern + 1) % pattern.Length;
                currentBeat = pattern[ind_pattern];
            }
        }
    }

    private void Detonate()
    {
        Vector3 worldPos = transform.localPosition;

        int count = Physics2D.OverlapCircleNonAlloc(worldPos, explosionRadius, hitBuffer, affectedLayers);

        for (int i = 0; i < count; i++)
        {
            Rigidbody2D rb = hitBuffer[i].attachedRigidbody;
            if (rb == null) continue;

            Vector2 hitPoint = hitBuffer[i].ClosestPoint(worldPos);
            Vector2 offset = hitPoint - (Vector2)worldPos;
            float dist = offset.magnitude;
            Vector2 dir = dist > 0.01f ? offset / dist : Random.insideUnitCircle.normalized;

            float normalizedDist = Mathf.Clamp01(dist / explosionRadius);
            float strength = falloff.Evaluate(normalizedDist) * explosionForce;

            rb.AddForceAtPosition(dir * strength, hitPoint, ForceMode2D.Impulse);

            rb.linearVelocity = Vector2.ClampMagnitude(rb.linearVelocity, maxLaunchSpeed);
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        }

        onDetonate?.Invoke(worldPos);

    }

    void Replace_cursor()
    {
        if (Mouse.current == null) return;
        Vector2 pos_screen = Mouse.current.position.ReadValue();
        Vector3 pos_world = Cam.ScreenToWorldPoint(pos_screen);
        pos_world.z = 0f;
        transform.localPosition = pos_world;
    }
}