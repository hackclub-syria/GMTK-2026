using System.Collections;
using System.Transactions;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class quit_script : MonoBehaviour
{

    [SerializeField] private GameObject container;
    [SerializeField] private Image bar;
    private GameObject transitionManager;

    private Coroutine holding;
    private bool isHolding = false;
    [SerializeField] private float duration = 1.5f;

    private void Start()
    {
        container.SetActive(false);
        bar.fillAmount = 0f;
        transitionManager = GameObject.FindGameObjectWithTag("sceneTransition");
    }

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)  StartHolding();
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasReleasedThisFrame) CancelHolding();
    }

    private void StartHolding()
    {
        if (isHolding) return;

        isHolding = true;
        container.SetActive(true);
        bar.fillAmount = 0f;

        holding = StartCoroutine(HoldProgressRoutine());
    }

    private void CancelHolding()
    {
        if (!isHolding) return;
        isHolding = false;

        if (holding != null)
        {
            StopCoroutine(holding);
            holding = null;
        }

        bar.fillAmount = 0f;
        container.SetActive(false);
    }

    private IEnumerator HoldProgressRoutine()
    {
        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.unscaledDeltaTime;
            if (bar != null)
            {
                bar.fillAmount = Mathf.Clamp01(timer / duration);
            }
            yield return null;
        }

        container.SetActive(false);
        mainMenu_go();
    }


    private void OnDisable()
    {
        CancelHolding();
    }
    public void mainMenu_go()
    {
        Time.timeScale = 1f;
        //SceneManager.LoadSceneAsync(0);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        transitionManager.transform.GetChild(0).gameObject.SetActive(true);
        //transitionManager.SetActive(true); 
        sceneTransitionScript.instance.openLevel(0);
    }
}