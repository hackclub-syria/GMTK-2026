using UnityEngine;
using Unity.Cinemachine;

public class CameraShake : CinemachineExtension
{
    [Header("Shake Settings")]
    [SerializeField] private float maxOffset = 0.3f;
    [SerializeField] private float maxRoll = 4f;
    [SerializeField] private float traumaDecayPerSecond = 1.2f;
    [SerializeField] private float frequency = 25f;

    private float trauma;
    private float seed;

    protected override void Awake()
    {
        base.Awake();
        seed = Random.value * 100f;
    }

    public void AddTrauma(float amount)
    {
        trauma = Mathf.Clamp01(trauma + amount);
    }

    public void AddTrauma(Vector3 _) => AddTrauma(0.4f);

    protected override void PostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam,
        CinemachineCore.Stage stage,
        ref CameraState state,
        float deltaTime)
    {
        if (stage == CinemachineCore.Stage.Finalize)
        {
            trauma = Mathf.Max(0f, trauma - traumaDecayPerSecond * Time.unscaledDeltaTime);

            if (trauma <= 0f) return;

            float shake = trauma * trauma;
            float t = Time.unscaledTime * frequency;

            float offsetX = maxOffset * shake * (Mathf.PerlinNoise(seed, t) * 2f - 1f);
            float offsetY = maxOffset * shake * (Mathf.PerlinNoise(seed + 50f, t) * 2f - 1f);
            float roll = maxRoll * shake * (Mathf.PerlinNoise(seed + 100f, t) * 2f - 1f);

            state.PositionCorrection += new Vector3(offsetX, offsetY, 0f);

            state.OrientationCorrection *= Quaternion.Euler(0f, 0f, roll);
        }
    }
}