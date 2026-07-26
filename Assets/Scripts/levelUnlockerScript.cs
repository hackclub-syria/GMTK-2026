using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class levelUnlockerScript : MonoBehaviour {
    private GameObject transitionManager;

    private void Awake() {
        transitionManager = GameObject.FindGameObjectWithTag("sceneTransition");
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("duck")) {
            string sceneName = SceneManager.GetActiveScene().name;
            int thisLevel = sceneName[sceneName.Length - 1] - '0';
            //if (thisLevel == 0) thisLevel = 10;
            if (thisLevel == 6) thisLevel = -1;
            if (thisLevel >= PlayerPrefs.GetInt("unlockedlevel")) {
                PlayerPrefs.SetInt("unlockedlevel", thisLevel + 1);
                PlayerPrefs.Save();
            }
            transitionManager.transform.GetChild(0).gameObject.SetActive(true);
            //transitionManager.SetActive(true);
            sceneTransitionScript.instance.openLevel(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
