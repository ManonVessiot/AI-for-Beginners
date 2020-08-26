using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowWayPoint : MonoBehaviour
{
    public GameObject[] wayPoints;
    int currentWayPoint = 0;

    public float distanceAccuricy = 10.0f;
    public float speed = 10.0f;
    public float rotSpeed = 10.0f;

    public float lookAhead = 10.0f;

    GameObject tracker;

    private void Start()
    {
        tracker = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        DestroyImmediate(tracker.GetComponent<Collider>());
        tracker.GetComponent<MeshRenderer>().enabled = false;
        tracker.transform.position = transform.position;
        tracker.transform.rotation = transform.rotation;
    }

    void ProgressTracker()
    {
        if (Vector3.Distance(tracker.transform.position, transform.position) < lookAhead)
        {
            if (Vector3.Distance(tracker.transform.position, wayPoints[currentWayPoint].transform.position) < distanceAccuricy)
            {
                currentWayPoint++;
                if (currentWayPoint >= wayPoints.Length)
                {
                    currentWayPoint = 0;
                }
            }
            tracker.transform.LookAt(wayPoints[currentWayPoint].transform);
            tracker.transform.Translate(0, 0, 2 * speed * Time.deltaTime);
        }
        
    }

    private void FixedUpdate()
    {
        ProgressTracker();

        Quaternion lookAtWayPoint = Quaternion.LookRotation(tracker.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookAtWayPoint, rotSpeed * Time.deltaTime);

        transform.Translate(0, 0, speed * Time.deltaTime);
    }
}
