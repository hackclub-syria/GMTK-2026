using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialManager : MonoBehaviour
{
    public GameObject[] tutorialGroups;
    public int currentIndex = 0;
    [SerializeField] private GameObject secretTunnel, forceOrb, levelSwitch;
    private void Start()
    {
        if (tutorialGroups == null || tutorialGroups.Length == 0) return;

        for (int i = 0; i < tutorialGroups.Length; i++)
        {
            tutorialGroups[i].SetActive(i == 0);
        }
    }

    private void Update()
    {
        if (Keyboard.current.enterKey.wasPressedThisFrame || Keyboard.current.numpadEnterKey.wasPressedThisFrame || Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (tutorialGroups == null || tutorialGroups.Length == 0) return;

            if (currentIndex < tutorialGroups.Length - 1)
            {
                tutorialGroups[currentIndex].SetActive(false);
                currentIndex++;
                tutorialGroups[currentIndex].SetActive(true);
            }
            else
            {
                tutorialGroups[currentIndex].SetActive(false);
            }
            if (currentIndex >= 6)
            {
                forceOrb.SetActive(true);
                levelSwitch.SetActive(true);
                secretTunnel.SetActive(false);
            }
        }
    }
}
