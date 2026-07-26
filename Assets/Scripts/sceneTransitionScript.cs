using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneTransitionScript : MonoBehaviour {
    public static sceneTransitionScript instance;
    [SerializeField] private Animator transitionAnim; 

    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(transform.parent.gameObject);
            //DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public void openLevel(string levelName) { 
        StartCoroutine(loadLevel(levelName)); 
    }
    IEnumerator loadLevel(string levelName) {
        transitionAnim.SetTrigger("End");
        yield return new WaitForSeconds(1); 
        SceneManager.LoadScene(levelName);
        transitionAnim.SetTrigger("Start");
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
    }
    public void openLevel(int levelID) { 
        StartCoroutine (loadLevel(levelID));
    }
    IEnumerator loadLevel(int levelID) {
        transitionAnim.SetTrigger("End");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(levelID);
        transitionAnim.SetTrigger("Start");
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
    }
}
