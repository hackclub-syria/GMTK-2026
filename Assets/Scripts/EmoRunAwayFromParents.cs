using UnityEngine;

public class EmoRunAwayFromParents : MonoBehaviour
{
    void Start()
    {
        transform.SetParent(null, true);
        transform.localScale = Vector3.one; // the canvas fucks the scale in the first frame
    }
}
