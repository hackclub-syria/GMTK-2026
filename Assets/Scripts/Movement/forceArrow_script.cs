using UnityEngine;
using System.Collections;
public class forceArrow_script : MonoBehaviour
{
    [Header("Launch Settings")]
    [SerializeField] private float launchForce = 20f;
    [SerializeField] private float delay = 0.12f;

    [Header("Pull Settings")]
    [SerializeField] private float pullForce = 30f;
    [SerializeField] private float brakingForce = 15f;

    private void OnTriggerEnter2D(Collider2D collider)
    {
       if(collider.CompareTag("duck"))
       { Rigidbody2D rb = collider.attachedRigidbody;

            if (rb != null)
            {
                StopAllCoroutines();
                StartCoroutine(PullPush(rb));
            }
       }
    }

    private IEnumerator PullPush(Rigidbody2D rb)
    {   
        float elapsed = 0f;
        WaitForFixedUpdate wait_fixed = new WaitForFixedUpdate();

        while (elapsed < delay)
        {
            if (rb == null) yield break;
            Vector2 center_dir = ((Vector2)transform.position - rb.position).normalized;
            rb.AddForce(center_dir * pullForce, ForceMode2D.Force);
            rb.AddForce(-rb.linearVelocity * brakingForce, ForceMode2D.Force);
            elapsed += Time.fixedDeltaTime;
            yield return wait_fixed;
        }

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            Vector2 launch_dir = transform.up;
            rb.AddForce(launch_dir * launchForce, ForceMode2D.Impulse);
        }
    }
}