using System.Collections;
using UnityEngine;

public class Die : MonoBehaviour
{
    public static Die instance { get; private set; }
    [Header("Settings")]
    public GameObject deathVfxPrefab;

    private SpriteRenderer duckSprite;
    private Collider2D duckCollider;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    private void Start()
    {
        duckSprite = GetComponent<SpriteRenderer>();
        duckCollider = GetComponent<Collider2D>();
    }

    public void DieBih()
    {
        StartCoroutine(JuicyDeathRoutine());
    }

    private IEnumerator JuicyDeathRoutine()
    {
        if (duckCollider != null) duckCollider.enabled = false;
        if (duckSprite != null) duckSprite.enabled = false;
        if (deathVfxPrefab != null)
        {
            Instantiate(deathVfxPrefab, transform.position, Quaternion.identity);
        }
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(0.1f);
        Time.timeScale = 1f;

        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        Time.timeScale = 1f;
    }
}