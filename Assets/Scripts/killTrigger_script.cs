using UnityEngine;

public class killTrigger_script : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.CompareTag("duck"))
        Die.instance.DieBih();
    }
}
