using UnityEngine;

public class smart_bullets_splitter : MonoBehaviour
{
    public float speed;
    public float lifeTime;
    public float distance;
    public float damage;
    public LayerMask whatIsSolid;
    public string Freak = "Freaky";

    public int splits = 0;

    public GameObject splitter;
    public bool multisplit;
    public GameObject destroyEffect;


    public InventoryObject inventory;
    private splitter_functions mysplits;

    private void Start()
    {
        mysplits = GetComponent<splitter_functions>();
        Invoke(nameof(DestroyProjectile), lifeTime);

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
            
            if (mine.mysplitter.prefab != null && mine.Name != "Instagram Reels")
            {
                splitter = mine.mysplitter.prefab;
                splits += amt * mine.mysplitter.num_splits;
                multisplit = !mine.mysplitter.issingle;
            }
        }
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(Freak);
        GameObject closest = null;
        float shortestDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                closest = enemy;
            }
        }

        if (closest != null)
        {
            Vector2 direction = (closest.transform.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }

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
            DestroyProjectile();
        }

        // Move projectile forward (in local right direction)
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    void DestroyProjectile()
    {
      mysplits.destroy_splitter();
    }
}
