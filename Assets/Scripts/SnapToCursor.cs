using UnityEngine;

public class SnapToCursor : MonoBehaviour
{
    void Update()
    {
        Vector2 mouse = Input.mousePosition;
        mouse = Camera.main.ScreenToWorldPoint(mouse);
        transform.position = mouse;
    }
}
