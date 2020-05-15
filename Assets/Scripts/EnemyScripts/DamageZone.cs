using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    [SerializeField] Collider2D damageZone;
    [SerializeField] LayerMask  damageMask;

    ContactFilter2D contactFilter;

    // Start is called before the first frame update
    void Start()
    {
        contactFilter = new ContactFilter2D();
        contactFilter.SetLayerMask(damageMask);
        contactFilter.useTriggers = true;
        
    }

    // Update is called once per frame
    void Update()
    {
        Collider2D[] results = new Collider2D[18];

        int nCollisions = Physics2D.OverlapCollider(damageZone, contactFilter, results);

        if (nCollisions > 0)
        {
            for (int i = 0; i < nCollisions; i++)
            {
                Collider2D otherCollider = results[i];

                HealthPoints playerHp = otherCollider.GetComponent<HealthPoints>();

                if (playerHp != null)
                {
                    playerHp.DealDamage(20);
                }
            }

            Debug.Log("Collision");
        }
        
    }
}
