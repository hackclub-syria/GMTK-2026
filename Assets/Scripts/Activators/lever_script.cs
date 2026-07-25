using UnityEngine;

public class lever_script : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Sprite activatedSprite;
    [SerializeField] private Sprite deactivatedSprite;
    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("duck"))
        ActivateLever();
    }

    void ActivateLever()
    {
        sr.sprite = activatedSprite;
        gameObject.tag = "activated";
    }

    void DeactivateLever()
    {
        sr.sprite = deactivatedSprite;
        gameObject.tag = "deactivated";
    }
}
