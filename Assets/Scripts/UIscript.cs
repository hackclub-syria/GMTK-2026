using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UIscript : MonoBehaviour {
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject settings;

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

    public void ResumeButton() {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }
    public void settingsButton() {
        pauseMenu.SetActive(false);
        settings.SetActive(true); 
    }
    public void quiitSettingsButton()
    {
        settings.SetActive(false);
        pauseMenu.SetActive(true);
    }
    public void returnToMainMenuButton() {
        SceneManager.LoadSceneAsync(0); 
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
