using Unity.VisualScripting;
using UnityEngine;

public class limeLevelChage_script : MonoBehaviour
{
    [SerializeField] private int targetSceneBuildIndex = 0;

    private GameObject transitionManager;

    private void Awake()
    {
        transitionManager = GameObject.FindGameObjectWithTag("sceneTransition");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
      //  if (!collision.CompareTag("duck")) return;

        //transitionManager.transform.GetChild(0).gameObject.SetActive(true);
        transitionManager.SetActive(true);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        sceneTransitionScript.instance.openLevel(targetSceneBuildIndex);
    }
}
