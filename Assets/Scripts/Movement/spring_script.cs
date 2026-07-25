using System.Collections;
using UnityEngine;

public class spring_script : MonoBehaviour
{
    [Header("Spring Settings")]
    [SerializeField] private float force = 15f;

    [Header("Sprites")]
    [SerializeField] private Sprite deactivatedSprite;
    [SerializeField] private Sprite activatedSprite;
    [SerializeField] private float delay = 0.3f;

    private SpriteRenderer sr;
    private Coroutine co;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        if (deactivatedSprite != null && sr != null)
        {
            sr.sprite = deactivatedSprite;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector2 dir = transform.up;
            float speed_spring_axis = Vector2.Dot(rb.linearVelocity, dir);

            if (speed_spring_axis < 0)
            {
                rb.linearVelocity -= dir * speed_spring_axis;
            }

            rb.AddForce(dir * force, ForceMode2D.Impulse);
            TriggerSpringVisual();
        }
    }

    private void TriggerSpringVisual()
    {
        if (sr == null || activatedSprite == null) return;
        sr.sprite = activatedSprite;
        if (co != null)
        {
            StopCoroutine(co);
        }
        co = StartCoroutine(ResetSpriteAfterDelay());
    }

    private IEnumerator ResetSpriteAfterDelay()
    {
        yield return new WaitForSeconds(delay);
        if (deactivatedSprite != null)
        {
            sr.sprite = deactivatedSprite;
        }
    }
}