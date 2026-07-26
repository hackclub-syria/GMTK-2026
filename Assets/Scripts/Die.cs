using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class Die : MonoBehaviour
{
    public GameObject deathVfxPrefab;
    public GameObject restartMenu;
    public GameObject pauseMenu;
    private SpriteRenderer duckSprite;
    private Collider2D duckCollider;

    void Start()
    {
        Invoke("DieBih", 2f);
        duckSprite = GetComponent<SpriteRenderer>();
        duckCollider = GetComponent<Collider2D>();
    }
    public void DieBih()
    {
        StartCoroutine(JuicyDeathRoutine());
    }

    private IEnumerator JuicyDeathRoutine()
    {
        restartMenu.SetActive(true);
        pauseMenu.SetActive(false);
        if (duckCollider != null) duckCollider.enabled = false;
        if (duckSprite != null) duckSprite.enabled = false;
        Instantiate(deathVfxPrefab, transform.position, Quaternion.identity);
        
        // impact frame ahh
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(0.1f);
        Time.timeScale = 1f;
        
        Destroy(gameObject);
    }
}
