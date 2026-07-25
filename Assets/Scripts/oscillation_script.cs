using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class oscillation_script : MonoBehaviour
{
    [SerializeField] private float speed = 3.0f;
    [SerializeField] private float stopTime = 0.05f;

    private Transform T_mover;
    private List<Vector3> waypoints = new List<Vector3>();
    private int ind_current = 1; 

    private int dir = 1;
    private Vector3 speed_current; 
    private void Start()
    {   
        if (transform.childCount < 3) return;
        T_mover = transform.GetChild(0);

        for (int i = 1; i < transform.childCount; i++) waypoints.Add(transform.GetChild(i).position);
        T_mover.position = waypoints[0];
    }

    private void Update()
    {
        if (waypoints.Count < 2) return;
        Vector3 pos_target = waypoints[ind_current];
        T_mover.position = Vector3.SmoothDamp(T_mover.position, pos_target, ref speed_current, 1/speed);
        if (Vector3.Distance(T_mover.position, pos_target) < stopTime) NextWaypoint();
    }

    private void NextWaypoint()
    {
        ind_current += dir;
        if (ind_current >= waypoints.Count)
        {
            dir = -1;
            ind_current = waypoints.Count - 2;
        }
        else if (ind_current < 0)
        {
            dir = 1;
            ind_current = 1;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.limeGreen;

        for (int i = 1; i < transform.childCount - 1; i++)
        {
            Gizmos.DrawLine(transform.GetChild(i).position, transform.GetChild(i + 1).position);
            Gizmos.DrawSphere(transform.GetChild(i).position, 0.2f);
        }
        Gizmos.DrawSphere(transform.GetChild(transform.childCount - 1).position, 0.2f);
    }
}
