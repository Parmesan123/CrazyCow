using UnityEngine;

public static class Extensions
{
    public static Vector3 GetRandomPointOnCircle(this Transform center, float radius)
    {
        float xCurrentMin = center.position.x - radius;
        float xCurrentMax = center.position.x + radius;
        
        float xResult = Random.Range(xCurrentMin, xCurrentMax);
        float zComponent = Mathf.Sqrt(Mathf.Abs(Mathf.Pow(radius, 2) - Mathf.Pow(xResult - center.position.x, 2)));
        
        int randomSide = Random.Range(1, 3);
        zComponent *= Mathf.Pow(-1, randomSide);
        
        float zResult = center.position.z + zComponent;

        return new Vector3(xResult, 0, zResult);
    }
}