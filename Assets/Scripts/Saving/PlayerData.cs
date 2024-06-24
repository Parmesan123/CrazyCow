using System;

namespace Saving
{
    [Serializable]
    public class PlayerData
    {
        public float MovementSpeed;
        public float DestroyBonusTime;
        public float DestroyRange;

        public PlayerData(float movementSpeed, float destroyBonusTime, float destroyRange)
        {
            MovementSpeed = movementSpeed;
            DestroyBonusTime = destroyBonusTime;
            DestroyRange = destroyRange;
        }
    }
}