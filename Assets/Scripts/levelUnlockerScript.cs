using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class levelUnlockerScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("duck")) {
            string sceneName = SceneManager.GetActiveScene().name;
            int thisLevel = sceneName[sceneName.Length - 1] - '0';
            if (thisLevel == 0) thisLevel = 10;
            if (thisLevel >= PlayerPrefs.GetInt("unlockedlevel")) {
                PlayerPrefs.SetInt("unlockedlevel", thisLevel + 1);
                PlayerPrefs.Save();
            }
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); 
        }
    }
}
