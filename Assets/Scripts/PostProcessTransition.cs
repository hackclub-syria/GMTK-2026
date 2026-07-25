using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class PostProcessTransition : MonoBehaviour
{
    public Volume vintageVolume;
    public float transitionDuration = 1.0f;

    private Coroutine transitionCoroutine;

    public void TurnOnVintageEffect()
    {
        if (transitionCoroutine != null) StopCoroutine(transitionCoroutine);
        transitionCoroutine = StartCoroutine(LerpVolumeWeight(1f));
    }

    public void TurnOffVintageEffect()
    {
        if (transitionCoroutine != null) StopCoroutine(transitionCoroutine);
        transitionCoroutine = StartCoroutine(LerpVolumeWeight(0f));
    }

    private IEnumerator LerpVolumeWeight(float targetWeight)
    {
        float startWeight = vintageVolume.weight;
        float timeElapsed = 0f;

        while (timeElapsed < transitionDuration)
        {
            vintageVolume.weight = Mathf.SmoothStep(startWeight, targetWeight, timeElapsed / transitionDuration);
            timeElapsed += Time.unscaledDeltaTime;

            yield return null;
        }

        vintageVolume.weight = targetWeight;
    }

}
