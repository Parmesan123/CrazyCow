using UnityEngine;

namespace Services
{
    public static class Extensions
    {
        public static Vector3 GetRandomPointOnCircle(this Transform center, float radius)
        {
            Vector2 randomPoint = Random.insideUnitCircle * radius;

            return new Vector3(randomPoint.x + center.transform.position.x, 0,
                randomPoint.y + center.transform.position.z);
        }
    }
}