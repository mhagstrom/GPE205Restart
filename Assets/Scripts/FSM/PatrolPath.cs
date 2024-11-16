using System;
using System.Linq;
using AAA;
using UnityEngine;

public class PatrolPath : MonoBehaviour
{
    // holds an array of waypoints for the AI tanks to follow
    private Transform[] _waypoints;

    private void Start()
    {
        //get all the children of the patrol path
        _waypoints = GetComponentsInChildren<Transform>().ToArray();
    }

    //find the enarest waypoint on the path so when the tank returns to the path it can find the closest point
    public Transform GetNearestWaypoint(Vector3 position)
    {
        Transform nearestWaypoint = null;
        float nearestDistance = Mathf.Infinity;
        
        foreach (Transform waypoint in _waypoints)
        {
            float distance = Vector3.Distance(position, waypoint.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestWaypoint = waypoint;
            }
        }

        return nearestWaypoint;
    }
    
    //find the next waypoint after the tank has reached its current one
    public Transform GetNextWaypoint(Transform currentWaypoint)
    {
        int currentIndex = -1;
        for (int i = 0; i < _waypoints.Length; i++)
        {
            if (_waypoints[i] == currentWaypoint)
            {
                currentIndex = i;
                break;
            }
        }

        if (currentIndex == -1)
        {
            Debug.LogError("Current waypoint not found in patrol path");
            return null;
        }

        int nextIndex = (currentIndex + 1) % _waypoints.Length;
        return _waypoints[nextIndex];
    }
    
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        //style the text
        GUIStyle style = new GUIStyle();
        style.fontSize = 20;
        style.fontStyle = FontStyle.Bold;
        style.normal.textColor = Color.white;
        
        for (int i = 0; i < _waypoints.Length; i++)
        {
            //draw numbers on the waypoints
            UnityEditor.Handles.Label(_waypoints[i].position, i.ToString(), style);
            
            //draw spheres at each waypoint
            Gizmos.color = new Color(.8f, 0f, 1f, .5f);
            Gizmos.DrawSphere(_waypoints[i].position, .5f);
            
            //connect the lines
            Gizmos.color = new Color(1f, .5f, .9f, 1f);
            if (i < _waypoints.Length - 1)
            {
                Gizmos.DrawLine(_waypoints[i].position, _waypoints[i + 1].position);
            }
            else
            {
                Gizmos.DrawLine(_waypoints[i].position, _waypoints[0].position);
            }
        }
    }
#endif
}
