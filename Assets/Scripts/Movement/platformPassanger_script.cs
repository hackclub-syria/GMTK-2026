using UnityEngine;

public class platformPassanger_script : MonoBehaviour
{

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("duck"))
        {
            foreach (ContactPoint2D contact in col.contacts)
            {
                if (contact.normal.y < -0.5f)
                {
                    col.transform.SetParent(transform);
                    break;
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("duck"))
        {
            if (col.transform.parent == transform)
            {
                col.transform.SetParent(null);
            }
        }
    }
}