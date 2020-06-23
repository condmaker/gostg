using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    [SerializeField] Collider2D damageZone;
    [SerializeField] LayerMask  damageMask;
    [SerializeField] LayerMask  enemyMask;

    ContactFilter2D contactFilter;

    void OnEnable()
    {
        contactFilter = new ContactFilter2D();
        contactFilter.SetLayerMask(damageMask);
        contactFilter.useTriggers = true;
        
    }

    void Update()
    {
        Collider2D[] results = new Collider2D[18];

        int nCollisions = Physics2D.OverlapCollider(damageZone, contactFilter, results);

        if (nCollisions > 0)
        {
            for (int i = 0; i < nCollisions; i++)
            {
                Collider2D otherCollider = results[i];
                Collider2D collider = Physics2D.OverlapBox(otherCollider.bounds.center, otherCollider.bounds.size, 0, enemyMask);
                Vector2    direction;

                HealthPoints playerHp = otherCollider.GetComponent<HealthPoints>();

                if (playerHp != null)
                {
                    direction = otherCollider.bounds.center - collider.bounds.center;
                    direction.Normalize();
                    direction.y = 1f;

                    playerHp.DealDamage(20, direction);
                }
            }
        }
        
    }
}
