using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class cursor_script : MonoBehaviour
{
    [SerializeField] private Transform T_mask;
    [SerializeField] private float tempo = 120;
    [SerializeField] private int[] pattern = new int[] { 4, 2, 4, 2 };
    [SerializeField] private float lerpSpeed = 15f;

    private Camera Cam;
    private float interval;
    private float timer = 0f;

    private int ind_pattern = 0;
    private int currentBeat;
    private Vector3 target_pos;

    void Start()
    {
        Cam = Camera.main;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;

        interval = 60f / tempo;

        if (pattern == null || pattern.Length == 0)
        {
            pattern = new int[] { 4 };
        }

        currentBeat = pattern[0];

        if (T_mask != null)
        {
            target_pos = T_mask.localPosition;
        }
    }

    void Update()
    {
        Replace_cursor();
        Handle_countdown();
        T_mask.localPosition = Vector3.Lerp(T_mask.localPosition, target_pos, Time.deltaTime * lerpSpeed);
    }

    void Handle_countdown()
    {
        timer += Time.deltaTime;

        if (timer >= interval)
        {
            timer -= interval;
            currentBeat--;
            int length = pattern[ind_pattern];

            if (currentBeat == length - 1)
            {
                target_pos = new Vector3(target_pos.x, 0.1f, target_pos.z);
            }

            target_pos = new Vector3(
                target_pos.x,
                target_pos.y - (T_mask.localScale.y / length),
                target_pos.z
            );

            if (currentBeat > 0)
            {
                Debug.Log(currentBeat);
            }
            else
            {
                Debug.Log("Boom");
                ind_pattern = (ind_pattern + 1) % pattern.Length;
                currentBeat = pattern[ind_pattern];
            }
        }
    }


    void Replace_cursor()
    {
        if (Mouse.current == null) return;
        Vector2 pos_screen = Mouse.current.position.ReadValue();
        Vector3 pos_world = Cam.ScreenToWorldPoint(pos_screen);
        pos_world.z = 0f;
        transform.localPosition = pos_world;
    }
}