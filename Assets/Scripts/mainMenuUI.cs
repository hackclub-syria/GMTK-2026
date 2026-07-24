using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class mainMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject settings;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject credits;

    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider SFXslider;

    
    void Start() {
        if (!PlayerPrefs.HasKey("master")) {
            PlayerPrefs.SetFloat("master", 1);
            PlayerPrefs.SetFloat("music", 1);
            PlayerPrefs.SetFloat("sfx", 1); 
        }
        masterSlider.value = PlayerPrefs.GetFloat("master");
        musicSlider.value = PlayerPrefs.GetFloat("music");
        SFXslider.value = PlayerPrefs.GetFloat("sfx"); 
    }

    public void playButton() {
        SceneManager.LoadSceneAsync(1);
    }
    public void settingsButton() {
        settings.SetActive(true);
        mainMenu.SetActive(false);
    }
    public void quiitSettingsButton() {
        settings.SetActive(false);
        mainMenu.SetActive(true); 
    }
    public void creditsButton() {
        credits.SetActive(true);
        mainMenu.SetActive(false);
    }
    public void quitCreditsButton() {
        credits.SetActive(false); 
        mainMenu.SetActive(true);
    }

    public void quitGameButton() {
        Application.Quit();
    }

    public void changeMaster() {
        AudioListener.volume = masterSlider.value;
        PlayerPrefs.SetFloat("master", masterSlider.value);
    }
    public void changeMusic() {
        PlayerPrefs.SetFloat("music", musicSlider.value);
    }
    public void changeSFX() {
        PlayerPrefs.SetFloat("sfx", SFXslider.value);
    }
}
