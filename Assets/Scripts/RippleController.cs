using System.Collections.Generic;
using UnityEngine;

public class RippleController : MonoBehaviour
{
    public float rippleSpeed = 1.5f;
    public float rippleLifespan = 2.0f;
    public float maxRippleStrength = 0.05f;

    private class Ripple
    {
        public Vector2 center;
        public float age;
    }

    private List<Ripple> activeRipples = new List<Ripple>();
    private const int MAX_RIPPLES = 10;
    private Vector4[] shaderDataArray = new Vector4[MAX_RIPPLES];

    private readonly int dataArrayID = Shader.PropertyToID("_RippleData");
    private readonly int aspectRatioID = Shader.PropertyToID("_RippleAspectRatio");

    private void Start() => ClearAllRipples();
    private void OnDisable() => ClearAllRipples();

    void Update()
    {
        UpdateRipples();
    }
    public void TriggerRipple(Vector2 screenPosition)
    {
        if (activeRipples.Count >= MAX_RIPPLES)
        {
            activeRipples.RemoveAt(0);
        }

        Vector2 viewportPos = Camera.main.ScreenToViewportPoint(screenPosition);
        activeRipples.Add(new Ripple { center = viewportPos, age = 0f });
    }

    private void UpdateRipples()
    {
        float aspectRatio = (float)Screen.width / (float)Screen.height;
        Shader.SetGlobalFloat(aspectRatioID, aspectRatio);

        for (int i = 0; i < MAX_RIPPLES; i++) shaderDataArray[i] = Vector4.zero;

        for (int i = activeRipples.Count - 1; i >= 0; i--)
        {
            Ripple r = activeRipples[i];
            r.age += Time.deltaTime;

            if (r.age >= rippleLifespan)
            {
                activeRipples.RemoveAt(i);
                continue;
            }

            float lifePercent = r.age / rippleLifespan;
            float currentProgress = r.age * rippleSpeed;
            float currentStrength = Mathf.Lerp(maxRippleStrength, 0f, lifePercent);

            shaderDataArray[i] = new Vector4(r.center.x, r.center.y, currentProgress, currentStrength);
        }

        Shader.SetGlobalVectorArray(dataArrayID, shaderDataArray);
    }

    private void ClearAllRipples()
    {
        activeRipples.Clear();
        for (int i = 0; i < MAX_RIPPLES; i++) shaderDataArray[i] = Vector4.zero;
        if (Application.isPlaying) Shader.SetGlobalVectorArray(dataArrayID, shaderDataArray);
    }
}
