using UnityEngine;

public static class Extensions
{
    public static Vector3 CalculatePointOnCircle(this Transform center, float radius)
    {
        float xCurrentMin = center.position.x - radius;
        float xCurrentMax = center.position.x + radius;
        
        float xResult = Random.Range(xCurrentMin, xCurrentMax);
        float zComponent = Mathf.Sqrt(Mathf.Abs(Mathf.Pow(radius, 2) - Mathf.Pow(xResult - center.position.x, 2)));
        float zResult = zComponent + center.position.z;

        return new Vector3(xResult, 0, zResult);
    }
}