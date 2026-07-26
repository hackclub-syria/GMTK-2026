using UnityEngine;

public class EmoRunAwayFromParents : MonoBehaviour
{
    void Start()
    {
        transform.SetParent(null, true);
    }
}
