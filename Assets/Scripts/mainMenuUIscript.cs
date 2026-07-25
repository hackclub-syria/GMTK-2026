using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

public class mainMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject settings;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject credits;

    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider SFXslider;
    [SerializeField] private AudioMixer audioMixer;


    void Start()
    {
        if (!PlayerPrefs.HasKey("master"))
        {
            PlayerPrefs.SetFloat("master", 1);
            PlayerPrefs.SetFloat("music", 1);
            PlayerPrefs.SetFloat("sfx", 1);
        }
        masterSlider.value = PlayerPrefs.GetFloat("master");
        audioMixer.SetFloat("masterVolume", Mathf.Log10(masterSlider.value) * 20f);
        musicSlider.value = PlayerPrefs.GetFloat("music");
        audioMixer.SetFloat("musicVolume", Mathf.Log10(musicSlider.value) * 20f);
        SFXslider.value = PlayerPrefs.GetFloat("sfx");
        audioMixer.SetFloat("SFXvolume", Mathf.Log10(SFXslider.value) * 20f);
    }

    public void playButton()
    {
        SceneManager.LoadSceneAsync(1);
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
        audioMixer.SetFloat("masterVolume", Mathf.Log10(masterSlider.value) * 20f);
    }
    public void changeMusic()
    {
        PlayerPrefs.SetFloat("music", musicSlider.value);
        audioMixer.SetFloat("musicVolume", Mathf.Log10(musicSlider.value) * 20f);
    }
    public void changeSFX()
    {
        PlayerPrefs.SetFloat("sfx", SFXslider.value);
        audioMixer.SetFloat("SFXvolume", Mathf.Log10(SFXslider.value) * 20f);
    }
}