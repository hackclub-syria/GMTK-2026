using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using System.Collections;

public class mainMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject settings;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject credits;
    [SerializeField] private GameObject levels;
    private GameObject transitionManager;

    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider SFXslider;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Button[] buttons;

    [SerializeField] private GameObject comicPanel;
    [SerializeField] private Image panelImage;
    [SerializeField] private Sprite[] images;
    [SerializeField] private bool alwaysPlayComics;

    void Awake()
    {
        if (!PlayerPrefs.HasKey("master"))
        {
            PlayerPrefs.SetFloat("master", 1);
            PlayerPrefs.SetFloat("music", 1);
            PlayerPrefs.SetFloat("sfx", 1);
            PlayerPrefs.Save();
        }
        masterSlider.value = PlayerPrefs.GetFloat("master");
        audioMixer.SetFloat("masterVolume", Mathf.Log10(masterSlider.value) * 20f);
        musicSlider.value = PlayerPrefs.GetFloat("music");
        audioMixer.SetFloat("musicVolume", Mathf.Log10(musicSlider.value) * 20f);
        SFXslider.value = PlayerPrefs.GetFloat("sfx");
        audioMixer.SetFloat("SFXvolume", Mathf.Log10(SFXslider.value) * 20f);

        int unlockedlevel = PlayerPrefs.GetInt("unlockedlevel", 1);
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = false;
        }
        for (int i = 0; i < unlockedlevel; i++)
        {
            if (i > 6) break;
            buttons[i].interactable = true;
        }
        buttons[8].interactable = buttons[9].interactable = true;

        transitionManager = GameObject.FindGameObjectWithTag("sceneTransition");
    }

    public void playButton()
    {
        bool isFirstPlay = PlayerPrefs.GetInt("firstPressPlay", 0) == 0;

        if (isFirstPlay || alwaysPlayComics)
        {
            if (isFirstPlay)
            {
                PlayerPrefs.SetInt("firstPressPlay", 1);
                PlayerPrefs.Save();
            }
            StartCoroutine(playSlideShow());
        }
        else
        {
            mainMenu.SetActive(false);
            levels.SetActive(true);
        }
    }

    IEnumerator playSlideShow()
    {
        mainMenu.SetActive(false);
        comicPanel.SetActive(true);

        yield return new WaitForSeconds(0.2f);

        for (int i = 0; i < images.Length; ++i)
        {
            panelImage.sprite = images[i];

            bool inputPressed = false;

            while (!inputPressed)
            {
                if (Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame)
                {
                    inputPressed = true;
                }

                if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
                {
                    inputPressed = true;
                }

                yield return null;
            }
        }

        comicPanel.SetActive(false);
        transitionManager.transform.GetChild(0).gameObject.SetActive(true);
        sceneTransitionScript.instance.openLevel("level0");
    }

    public void quitLevelsMenu()
    {
        levels.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void settingsButton()
    {
        settings.SetActive(true);
        mainMenu.SetActive(false);
    }

    public void quiitSettingsButton()
    {
        settings.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void creditsButton()
    {
        credits.SetActive(true);
        mainMenu.SetActive(false);
    }

    public void quitCreditsButton()
    {
        credits.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void quitGameButton()
    {
        Application.Quit();
    }

    public void changeMaster()
    {
        PlayerPrefs.SetFloat("master", masterSlider.value);
        PlayerPrefs.Save();
        audioMixer.SetFloat("masterVolume", Mathf.Log10(masterSlider.value) * 20f);
    }

    public void changeMusic()
    {
        PlayerPrefs.SetFloat("music", musicSlider.value);
        PlayerPrefs.Save();
        audioMixer.SetFloat("musicVolume", Mathf.Log10(musicSlider.value) * 20f);
    }

    public void changeSFX()
    {
        PlayerPrefs.SetFloat("sfx", SFXslider.value);
        PlayerPrefs.Save();
        audioMixer.SetFloat("SFXvolume", Mathf.Log10(SFXslider.value) * 20f);
    }

    public void openLevel(int levelID)
    {
        string s = "level" + levelID;
        transitionManager.transform.GetChild(0).gameObject.SetActive(true);
        sceneTransitionScript.instance.openLevel(s);
    }
}
