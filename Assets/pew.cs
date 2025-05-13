using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public struct splitter
{
    public GameObject prefab;
    public bool issingle;
    public bool triggeroncontact;
    public int num_splits;
}

public class pew : MonoBehaviour
{
    public float speed;
    public float lifeTime;
    public float distance;
    public float damage;
    public LayerMask whatIsSolid;
    public string Freak = "Freaky";

    public int splits = 0;

    public GameObject splitter;

    public GameObject destroyEffect;

    [Header("Optional Audio")]
    public AudioClip shootSound;
    public InventoryObject inventory;

    public bool multisplit = false;

    public Splitter_list OSL;

    private void Start()
    {
        // Play shoot sound at the projectile's position
        if (shootSound != null)
        {
            AudioSource.PlayClipAtPoint(shootSound, transform.position);
        }

        Invoke(nameof(fake_destroy), lifeTime);

        for (int i = 0; i < inventory.container.Count; i++)
        {
            int amt = inventory.container[i].amount;
            ItemData mine = inventory.container[i].item;

            damage += amt * mine.damage;
            damage *= 1 + amt * mine.damage_mult;

            if (mine.true_damage_mult != 0)
            {
                damage *= Mathf.Pow(mine.true_damage_mult, amt);
            }

            speed += amt * mine.shot_speed;
            if (mine.true_shot_speed_mult != 0)
            {
                speed *= Mathf.Pow(mine.true_shot_speed_mult, amt);
            }

            if (mine.mysplitter.prefab != null)
            {
                
                if (!mine.mysplitter.issingle)
                {
                    multisplit = true;
                }
            }
        }
        splits = OSL.splitter_list[0].mysplitter.num_splits * OSL.splitter_list[0].amount ;
    }

    private void Update()
    {
        bool hitSomething = false;

        // Raycast for 2D colliders
        RaycastHit2D hit2D = Physics2D.Raycast(transform.position, Vector2.right, distance, whatIsSolid);
        if (hit2D.collider != null)
        {
            if (hit2D.collider.CompareTag(Freak))
            {
                var enemy = hit2D.collider.GetComponent<Enemy_health>();
                if (enemy != null) enemy.TakeDamage(damage);
            }

            hitSomething = true;
        }

        // Raycast for 3D colliders
        RaycastHit hit3D;
        if (Physics.Raycast(transform.position, transform.right, out hit3D, distance, whatIsSolid))
        {
            if (hit3D.collider.CompareTag(Freak))
            {
                var enemy = hit3D.collider.GetComponent<Enemy_health>();
                if (enemy != null) enemy.TakeDamage(damage);
            }

            hitSomething = true;
        }

        // Destroy projectile if either raycast hit something
        if (hitSomething)
        {
            DestroyProjectile(true);
        }

        // Move projectile forward (in local right direction)
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    void fake_destroy()
    {
        DestroyProjectile(false);
    }

    void DestroyProjectile(bool isfromimpact)
    {
        configured_splitters curr = OSL.splitter_list[0];
        if (curr.amount != 0)
        {
            if (isfromimpact)
            {
                if (curr.mysplitter.triggeroncontact)
                {
                    triggersplitter();
                }
                else
                {
                    if (destroyEffect != null)
                    {
                        Instantiate(destroyEffect, transform.position, Quaternion.identity);
                    }
                    Destroy(gameObject);
                }
            }
            else
            {
                triggersplitter();
            }
        }
        else
        {
            if (destroyEffect != null)
            {
                Instantiate(destroyEffect, transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
        }
    }

    void triggersplitter()
    {
        if (multisplit)
        {
            for (int i = 0; i < splits; i++)
            {
                GameObject init = Instantiate(OSL.splitter_list[0].mysplitter.prefab, transform.position, transform.rotation);
                init.GetComponent<splitter_functions>().curr_split = 1;
            }
        }
        else if (splits != 0)
        {
            GameObject init = Instantiate(OSL.splitter_list[0].mysplitter.prefab, transform.position, transform.rotation);
            init.GetComponent<splitter_functions>().curr_split = 1;
        }

        if (destroyEffect != null)
        {
            Instantiate(destroyEffect, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
