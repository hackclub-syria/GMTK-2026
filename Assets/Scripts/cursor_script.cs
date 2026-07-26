using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.Audio;

public class cursor_script : MonoBehaviour
{
    [Header("Cursor & Rhythm")]
    [SerializeField] private Transform T_mask;
    [SerializeField] private float tempo = 120;
    [SerializeField] private int[] pattern = new int[] { 4, 2, 4, 2 };

    [Header("Time Energy Bar")]
    [SerializeField] private Image energyBarImage;
    [SerializeField] private float maxEnergy = 100f;
    [SerializeField] private float depletionRate = 40f;
    private float currentEnergy;
    private bool isGrounded = false;
    private bool canDilate = true;

    [Header("Countdown Animation")]
    [SerializeField, Range(1f, 30f)] private float lerpSpeed = 15f;

    [Header("Time Dilation")]
    [SerializeField, Range(0.02f, 0.5f)] private float slowScale = 0.1f;
    [SerializeField] private float enterDuration = 0.15f;
    [SerializeField] private float exitDuration = 0.06f;

    [Header("Audio Mixer Effects")]
    [SerializeField] private AudioMixerSnapshot normalSnapshot;
    [SerializeField] private AudioMixerSnapshot slowMotionSnapshot;

    [Header("Explosion (Runs on Unscaled Time)")]
    [SerializeField] private float explosionRadius = 2.5f;
    [SerializeField] private float explosionForce = 12f;
    [SerializeField] private AnimationCurve falloff = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
    [SerializeField] private LayerMask affectedLayers;
    [SerializeField] private float maxLaunchSpeed = 60f;
    public Transform mouseTip;


    [Header("Red Zones")]
    [SerializeField] private LayerMask redZoneLayer;
    private readonly string playerTag = "duck";

    [Header("Feedback Hooks")]
    public UnityEvent<Vector3> onDetonate;
    public PostProcessTransition transitionScript;
    public RippleController rippleController;
    private bool isVintageActive = false;
    public AudioSource musicAudioSource;

    private Camera Cam;

    private double beatInterval;
    private double nextBeatTime;

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

        beatInterval = 60.0 / tempo;

        if (pattern == null || pattern.Length == 0)
        {
            pattern = new int[] { 4 };
        }

        double startDspTime = AudioSettings.dspTime;

        musicAudioSource.PlayScheduled(startDspTime + beatInterval * pattern[0]);

        nextBeatTime = startDspTime + beatInterval;

        currentBeat = pattern[0];
        targetMaskLocalY = T_mask.localPosition.y;

        baseFixedDeltaTime = Time.fixedDeltaTime;

        currentEnergy = maxEnergy;

        if (energyBarImage != null)
        {
            energyBarImage.fillAmount = currentEnergy / maxEnergy;
        }
    }

    void Update()
    {
        Handle_TimeDilation();
        Replace_cursor();
        Handle_countdown();

        float newY = Mathf.Lerp(T_mask.localPosition.y, targetMaskLocalY, Time.unscaledDeltaTime * lerpSpeed);
        T_mask.localPosition = new Vector3(T_mask.localPosition.x, newY, T_mask.localPosition.z);
    }

    public void SetGroundedState(bool state)
    {
        isGrounded = state;
    }

    public void RefillEnergy()
    {
        currentEnergy = maxEnergy;
        canDilate = true;

        if (energyBarImage != null)
        {
            energyBarImage.fillAmount = currentEnergy / maxEnergy;
        }
    }

    void Handle_TimeDilation()
    {
        bool isSpacePressed = Keyboard.current != null && Keyboard.current.spaceKey.isPressed;

        if (!isSpacePressed)
        {
            canDilate = true;
        }

        bool applyDilation = isSpacePressed && canDilate && currentEnergy > 0f;

        if (applyDilation)
        {
            currentEnergy -= depletionRate * Time.unscaledDeltaTime;
            if (currentEnergy <= 0f)
            {
                currentEnergy = 0f;
                canDilate = false;
                applyDilation = false;
            }
        }
        else if (isGrounded)
        {
            currentEnergy = maxEnergy;
        }

        if (energyBarImage != null)
        {
            energyBarImage.fillAmount = currentEnergy / maxEnergy;
        }

        if (applyDilation && !isVintageActive)
        {
            isVintageActive = true;

            if (transitionScript != null) transitionScript.TurnOnVintageEffect();

            if (slowMotionSnapshot != null)
            {
                slowMotionSnapshot.TransitionTo(enterDuration);
            }
        }
        else if (!applyDilation && isVintageActive)
        {
            isVintageActive = false;

            if (transitionScript != null) transitionScript.TurnOffVintageEffect();

            if (normalSnapshot != null)
            {
                normalSnapshot.TransitionTo(exitDuration);
            }
        }

        float target = applyDilation ? slowScale : 1f;
        float duration = applyDilation ? enterDuration : exitDuration;
        float rate = 1f / Mathf.Max(duration, 0.0001f);

        Time.timeScale = Mathf.MoveTowards(Time.timeScale, target, rate * Time.unscaledDeltaTime);
        Time.fixedDeltaTime = baseFixedDeltaTime * Mathf.Max(Time.timeScale, 0.02f);
    }

    void Handle_countdown()
    {
        double currentDspTime = AudioSettings.dspTime;

        while (currentDspTime >= nextBeatTime)
        {
            nextBeatTime += beatInterval;

            currentBeat--;
            int length = pattern[ind_pattern];

            if (currentBeat == length - 1)
            {
                targetMaskLocalY = 0.1f;
            }

            targetMaskLocalY -= (T_mask.localScale.y / length);

            if (currentBeat <= 0)
            {
                Detonate();
                ind_pattern = (ind_pattern + 1) % pattern.Length;
                currentBeat = pattern[ind_pattern];
            }
        }
    }

    private void Detonate()
    {
        Vector3 worldPos = mouseTip.transform.position;

        int count = Physics2D.OverlapCircleNonAlloc(worldPos, explosionRadius, hitBuffer, affectedLayers);

        for (int i = 0; i < count; i++)
        {
            Collider2D col = hitBuffer[i];
            Rigidbody2D rb = col.attachedRigidbody;
            if (rb == null) continue;

            if (col.CompareTag(playerTag))
            {
                if (col.IsTouchingLayers(redZoneLayer))
                {
                    continue;
                }
            }

            Vector2 hitPoint = col.ClosestPoint(worldPos);
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

        if (rippleController != null && Mouse.current != null)
        {
            rippleController.TriggerRipple(worldPos);
        }
    }

    void Replace_cursor()
    {
        if (Mouse.current == null) return;
        Vector2 pos_screen = Mouse.current.position.ReadValue();
        Vector3 pos_world = Cam.ScreenToWorldPoint(pos_screen);
        pos_world.z = 0f;

        transform.position = pos_world;
    }
}