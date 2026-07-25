using UnityEngine;
using UnityEngine.UI;

public class SFXScript : MonoBehaviour
{
    public static SFXScript instance;
    [SerializeField] private AudioSource SFXObject;
    [SerializeField] private AudioClip buttonSound;
    [SerializeField] private AudioClip interactSound;
    [SerializeField] private Slider Masterslider;
    [SerializeField] private Slider MusicSlider;
    [SerializeField] private Slider SFXSlider;

    void Start() {
        if (instance == null)
            instance = this;
    }

    public void PlaySFX(AudioClip clip, Transform spawnTrans, float volume = 1, float pitch = 1) {
        AudioSource audioSource = Instantiate(SFXObject, spawnTrans.position, Quaternion.identity);
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.pitch = pitch;
        audioSource.Play();
        float clipLength = audioSource.clip.length;
        Destroy(audioSource.gameObject, clipLength);
    }
    public void buttonSFX() {
        PlaySFX(buttonSound, transform, 1f, Random.Range(0.9f, 1.1f));
    }
    public void interactSFX() {
        PlaySFX(interactSound, transform, 0.8f, Random.Range(0.8f, 1.2f));
    }
    public void slideSFX(int sliderNumber) {
        switch (sliderNumber) {
            case 1:
                float pitch;
                pitch = 1f + Masterslider.value;
                PlaySFX(buttonSound, transform, 1, pitch);
                break;
            case 2:
                pitch = 1f + MusicSlider.value;
                PlaySFX(buttonSound, transform, 1, pitch);
                break;
            case 3:
                pitch = 1f + SFXSlider.value;
                PlaySFX(buttonSound, transform, 1, pitch);
                break;
        }
    }
}
