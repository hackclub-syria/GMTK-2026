using UnityEngine;
using System.Collections;

public class SimpleStandUp : MonoBehaviour
{
    public float standUpTime = 0.3f;
    public float settleWaitTime = 1f;
    public float sleepVelocity = 0.1f;
    public float wakeUpVelocity = 2.0f;

    private Rigidbody2D rb;
    private CapsuleCollider2D capsule;
    private bool isStanding = false;
    private float settledTimer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        capsule = GetComponent<CapsuleCollider2D>();
    }

    void FixedUpdate()
    {
        if (isStanding) return;

        float currentSpeedSq = rb.linearVelocity.sqrMagnitude;

        if (rb.freezeRotation && currentSpeedSq > (wakeUpVelocity * wakeUpVelocity))
        {
            rb.freezeRotation = false;
            settledTimer = 0f;
        }

        if (currentSpeedSq < (sleepVelocity * sleepVelocity) && Mathf.Abs(rb.angularVelocity) < sleepVelocity)
        {
            settledTimer += Time.fixedDeltaTime;
            if (settledTimer > settleWaitTime && Mathf.Abs(Mathf.DeltaAngle(rb.rotation, 0f)) > 0.5f)
            {
                StartCoroutine(StandUpRoutine());
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
        rb.freezeRotation = true;

        isStanding = false;
        settledTimer = 0f;
    }

    private float GetTargetY(float angle, float floorY)
    {
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
/* call this when detonate + duck is affected   public void UnfreezeForThrow()
    {
        if (rb != null) rb.freezeRotation = false;
        settledTimer = 0f;
    }*/
}
