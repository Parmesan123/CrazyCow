using Entities;
using UnityEngine;

namespace Skills
{
    public class DestroyAllBoxSkill : ISkill
    {
        private readonly float _radius;

        public SkillData Data { get; }

        public DestroyAllBoxSkill(SkillData data)
        {
            Data = data;

            _radius = Data.Radius;
        }

        public void Perform(Transform player)
        {
            Collider[] colliders = Physics.OverlapSphere(player.position, _radius);

            foreach (Collider collider in colliders)
                if (collider.TryGetComponent(out Box boxToDestroy))
                    boxToDestroy.Destroy();
        }
    }    
}