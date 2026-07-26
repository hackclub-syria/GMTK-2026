using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIscript : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject settings;

    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider SFXslider;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private GameObject duck;
    [SerializeField] private GameObject cur;

    private bool isPaused = false;

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

        pauseMenu.SetActive(false);
        settings.SetActive(false);
    }

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        duck.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        isPaused = true;
        Debug.Log(Time.timeScale);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        cur.SetActive(false);
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        settings.SetActive(false);
        Time.timeScale = 1f;
        duck.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;

        isPaused = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        cur.SetActive(true);


    }

    public void ResumeButton()
    {
        Resume();
    }

    public void settingsButton()
    {
        pauseMenu.SetActive(false);
        settings.SetActive(true);
    }

    public void quiitSettingsButton()
    {
        settings.SetActive(false);
        pauseMenu.SetActive(true);
    }

    public void returnToMainMenuButton()
    {
        Time.timeScale = 1f;
        duck.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        SceneManager.LoadSceneAsync(0);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void changeMaster()
    {
        PlayerPrefs.SetFloat("master", masterSlider.value);
        audioMixer.SetFloat("masterVolume", Mathf.Log10(Mathf.Max(masterSlider.value, 0.0001f)) * 20f);
    }

    public void changeMusic()
    {
        PlayerPrefs.SetFloat("music", musicSlider.value);
        audioMixer.SetFloat("musicVolume", Mathf.Log10(Mathf.Max(musicSlider.value, 0.0001f)) * 20f);
    }

    public void changeSFX()
    {
        PlayerPrefs.SetFloat("sfx", SFXslider.value);
        audioMixer.SetFloat("SFXvolume", Mathf.Log10(Mathf.Max(SFXslider.value, 0.0001f)) * 20f);
    }
}