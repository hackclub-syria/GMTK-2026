using UnityEngine;
using System.Collections;

public class SimpleStandUp : MonoBehaviour
{
    [Header("Stand Up Settings")]
    public float standUpTime = 0.3f;
    public float settleWaitTime = 1f;
    public float sleepVelocity = 0.1f;
    public float wakeUpVelocity = 2.0f;

    [Header("Animation & Sprite Settings")]
    public Animator animator;           // <-- ADDED to handle Idle animation
    public SpriteRenderer spriteRenderer;
    public Sprite normalSprite;
    public Sprite shotSprite;

    private Rigidbody2D rb;
    private CapsuleCollider2D capsule;
    private bool isStanding = false;
    private float settledTimer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        capsule = GetComponent<CapsuleCollider2D>();

        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
        if (animator == null) animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (isStanding) return;

        float currentSpeedSq = rb.linearVelocity.sqrMagnitude;

        // 1. WAKING UP (Getting knocked around by physics)
        // Only triggers once per hit because WakeUpDuck() sets freezeRotation to false
        if (rb.freezeRotation && currentSpeedSq > (wakeUpVelocity * wakeUpVelocity))
        {
            WakeUpDuck();
        }

        // 2. SETTLING DOWN (Coming to a rest)
        if (currentSpeedSq < (sleepVelocity * sleepVelocity) && Mathf.Abs(rb.angularVelocity) < sleepVelocity)
        {
            settledTimer += Time.fixedDeltaTime;
            if (settledTimer > settleWaitTime)
            {
                if (Mathf.Abs(Mathf.DeltaAngle(rb.rotation, 0f)) > 0.5f)
                {
                    // Duck is resting on its side, play the standup routine
                    StartCoroutine(StandUpRoutine());
                }
                else if (!rb.freezeRotation)
                {
                    // Edge case: Duck landed perfectly upright naturally without rotating!
                    SnapToIdle();
                }
            }
        }
        else
        {
            settledTimer = 0f;
        }
    }

    private IEnumerator StandUpRoutine()
    {
        isStanding = true;

        rb.isKinematic = true;
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;

        float startAngle = rb.rotation;
        float startX = rb.position.x;
        float floorY = capsule.bounds.min.y;
        float elapsed = 0f;

        while (elapsed < standUpTime)
        {
            elapsed += Time.fixedDeltaTime;
            float t = Mathf.Clamp01(elapsed / standUpTime);
            float smoothT = t * t * (3f - 2f * t);

            float currentAngle = Mathf.LerpAngle(startAngle, 0f, smoothT);
            float requiredY = GetTargetY(currentAngle, floorY);

            rb.MoveRotation(currentAngle);
            rb.MovePosition(new Vector2(startX, requiredY));

            yield return new WaitForFixedUpdate();
        }

        rb.rotation = 0f;
        rb.position = new Vector2(startX, GetTargetY(0f, floorY));
        rb.isKinematic = false;

        SnapToIdle(); // Duck is stabilized, hook to idle!

        isStanding = false;
        settledTimer = 0f;
    }

    // --- NEW HELPER METHODS ---

    private void SnapToIdle()
    {
        rb.freezeRotation = true;
        rb.rotation = 0f; // Ensure perfect zero

        if (animator != null)
        {
            animator.SetBool("IsIdle", true);
        }
        // Fallback safety if Animator isn't used
        else if (spriteRenderer != null && normalSprite != null)
        {
            spriteRenderer.sprite = normalSprite;
        }
    }

    private void WakeUpDuck()
    {
        rb.freezeRotation = false;
        settledTimer = 0f;

        if (animator != null)
        {
            animator.SetBool("IsIdle", false);
        }

        // Even with Animator, we can optionally map a static shotSprite 
        // to a 1-frame animation clip in your animator window
        if (spriteRenderer != null && shotSprite != null)
        {
            spriteRenderer.sprite = shotSprite;
        }
    }

    // Called instantly from cursor detonate so animation reacts on frame 1
    public void UnfreezeForThrow()
    {
        WakeUpDuck();
    }

    private float GetTargetY(float angle, float floorY)
    {
        // ... (Keep your EXACT original GetTargetY logic down here!)
        Vector2 scale = transform.lossyScale;
        Vector2 localCore;
        float radius;

        if (capsule.direction == CapsuleDirection2D.Vertical)
        {
            localCore = new Vector2(0f, (capsule.size.y / 2f) - (capsule.size.x / 2f));
            radius = (capsule.size.x / 2f) * Mathf.Abs(scale.x);
        }
        else
        {
            localCore = new Vector2((capsule.size.x / 2f) - (capsule.size.y / 2f), 0f);
            radius = (capsule.size.y / 2f) * Mathf.Abs(scale.y);
        }

        localCore = new Vector2(localCore.x * scale.x, localCore.y * scale.y);

        Vector3 rotatedCore = Quaternion.Euler(0, 0, angle) * (Vector3)localCore;
        float clearance = radius + Mathf.Abs(rotatedCore.y);

        Vector2 scaledOffset = new Vector2(capsule.offset.x * scale.x, capsule.offset.y * scale.y);
        Vector3 rotatedOffset = Quaternion.Euler(0, 0, angle) * (Vector3)scaledOffset;

        return floorY + clearance - rotatedOffset.y;
    }
}
