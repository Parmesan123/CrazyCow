using Entities;
using UnityEngine;

public class DestroyFirstBoxSkill : ISkill
{
    private float _defaultRadius;

    public SkillData Data { get; }

    public DestroyFirstBoxSkill(SkillData data)
    {
        Data = data;

        _defaultRadius = Data.Radius;
    }

    public void Perform(Transform player)
    {
        for (int i = 0; i < 400; i++)
        {
            Collider[] colliders = Physics.OverlapSphere(player.position, _defaultRadius);

            foreach (Collider collider in colliders)
            {
                if (!collider.TryGetComponent(out Box boxToDestroy)) 
                    continue;
                
                boxToDestroy.Destroy();
                return;
            }

            _defaultRadius += 1;
        }
    }
}