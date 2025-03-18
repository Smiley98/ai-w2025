using UnityEngine;

public static class Projection
{
    public static Vector2 ProjectPointLine(Vector2 A, Vector2 B, Vector2 P)
    {
        Vector2 AB = B - A;
        float t = Vector2.Dot(P - A, AB) / Vector2.Dot(AB, AB);
        return A + AB * Mathf.Clamp(t, 0.0f, 1.0f);
    }

    public static float ScalarProjectPointLine(Vector2 A, Vector2 B, Vector2 P)
    {
        Vector2 AB = B - A;
        float t = Vector2.Dot(P - A, AB) / Vector2.Dot(AB, AB);
        return t;
    }
}
