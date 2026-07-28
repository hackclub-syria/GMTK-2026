using UnityEngine;

public class spring_script : MonoBehaviour
{
    [Header("Spring Settings")]
    [SerializeField] private float force = 15f;

    [Header("Animation")]
    [SerializeField] private Animator animator;

    [SerializeField] private string triggerName = "isActive";

    private void Awake()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Rigidbody2D rb = collision.rigidbody;
        if (rb == null) return;

        Vector2 dir = transform.up;

        float speedSpringAxis = Vector2.Dot(rb.linearVelocity, dir);
        if (speedSpringAxis < 0)
            rb.linearVelocity -= dir * speedSpringAxis;

        rb.AddForce(dir * force, ForceMode2D.Impulse);

        if (animator != null)
            animator.SetTrigger(triggerName);
    }
}
