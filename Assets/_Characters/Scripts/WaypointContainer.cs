using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointContainer : MonoBehaviour
{



    private void OnDrawGizmos()
    {
        Vector3 firstPosition = transform.GetChild(0).position;
        Vector3 previousPosition = firstPosition;
        foreach (Transform waypoint in transform)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawSphere(waypoint.transform.position, 0.1f);
            Gizmos.color = Color.green;
            Gizmos.DrawLine(previousPosition, waypoint.transform.position);
            previousPosition = waypoint.transform.position;

        }
        Gizmos.color = Color.black;
        Gizmos.DrawLine(previousPosition, firstPosition); 
    }
}
