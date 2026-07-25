using UnityEngine;

public class PressurePlate_script : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Sprite activatedSprite;
    [SerializeField] private Sprite deactivatedSprite;

    private int trigger = 0;

    void OnTriggerEnter2D(Collider2D col)
    {
        trigger++;
        if (trigger >= 1)
        {
            ActivatePlate();
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        trigger--;
        if (trigger <= 0)
        {
            trigger = 0;
            DeactivatePlate();
        }
    }

    void ActivatePlate()
    {
        sr.sprite = activatedSprite;
        gameObject.tag = "activated";
    }

    void DeactivatePlate()
    {
        sr.sprite = deactivatedSprite;
        gameObject.tag = "deactivated";
    }
}