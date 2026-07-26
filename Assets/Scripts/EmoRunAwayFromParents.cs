using UnityEngine;

public class EmoRunAwayFromParents : MonoBehaviour
{
    public bool isBitch = false;
    void Start()
    {
        transform.SetParent(null, true);
        transform.localScale = Vector3.one; // the canvas fucks the scale in the first frame
        if (isBitch)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }
}
