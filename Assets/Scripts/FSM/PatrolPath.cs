using System.Collections.Generic;
using UnityEngine;

public class PatrolPath : MonoBehaviour
{
    public List<Transform> Waypoints => GameManager.Instance.Waypoints;

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;
        int index = 0;
        foreach(var w in Waypoints)
        {
            if(w == null) continue;
            UnityEditor.Handles.Label(w.transform.position, index.ToString());
            index++;
        }
    }
#endif

    //find the enarest waypoint on the path so when the tank returns to the path it can find the closest point
    public Transform GetNearestWaypoint(Vector3 position)
    {
        Transform nearestWaypoint = null;
        float nearestDistance = Mathf.Infinity;
        
        foreach (Transform waypoint in Waypoints)
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
        for (int i = 0; i < Waypoints.Count; i++)
        {
            if (Waypoints[i] == currentWaypoint)
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

        int nextIndex = (currentIndex + 1) % Waypoints.Count;
        return Waypoints[nextIndex];
    }
    
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying) return;

        //style the text
        GUIStyle style = new GUIStyle();
        style.fontSize = 20;
        style.fontStyle = FontStyle.Bold;
        style.normal.textColor = Color.white;
        
        for (int i = 0; i < Waypoints.Count; i++)
        {
            //draw numbers on the waypoints
            UnityEditor.Handles.Label(Waypoints[i].position, i.ToString(), style);
            
            //draw spheres at each waypoint
            Gizmos.color = new Color(.8f, 0f, 1f, .5f);
            Gizmos.DrawSphere(Waypoints[i].position, .5f);
            
            //connect the lines
            Gizmos.color = new Color(1f, .5f, .9f, 1f);
            if (i < Waypoints.Count - 1)
            {
                Gizmos.DrawLine(Waypoints[i].position, Waypoints[i + 1].position);
            }
            else
            {
                Gizmos.DrawLine(Waypoints[i].position, Waypoints[0].position);
            }
        }
    }
#endif
}
