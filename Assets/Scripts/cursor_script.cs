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
    private Camera Cam;
    private float interval;
    private float timer = 0f;

    private int ind_pattern = 0;
    private int currentBeat;

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
    }

    void Update()
    {
        Replace_cursor();
        Handle_countdown();
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
                T_mask.localPosition = new Vector3(T_mask.localPosition.x, 0.1f, T_mask.localPosition.z);
            }
            T_mask.localPosition = new Vector3(
                T_mask.localPosition.x,
                T_mask.localPosition.y - (T_mask.localScale.y / length),
                T_mask.localPosition.z
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