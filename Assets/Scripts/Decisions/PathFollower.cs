using UnityEngine;

public class PathFollower : MonoBehaviour
{
    [SerializeField]
    GameObject[] waypoints;

    float time = 0.0f;
    int curr = 0;
    int next = 1;

    void Update()
    {
        float dt = Time.deltaTime;
        time += dt;
        if (time > 1.0)
        {
            time = 0.0f;
            curr++;
            next++;
            curr = curr % waypoints.Length;
            next = next % waypoints.Length;
        }

        Vector3 position = Vector3.Lerp(waypoints[curr].transform.position, waypoints[next].transform.position, time);
        transform.position = position;
    }
}
