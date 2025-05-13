using UnityEngine;

public class gum_splitter_behavior : MonoBehaviour
{
    public float speed;
    public float lifeTime;
    public float distance;
    public float damage;
    public LayerMask whatIsSolid;
    public string Freak = "Freaky";

    private splitter_functions mysplits;

    float angle_mult = 1;
    public InventoryObject inventory;

    private void Start()
    {
        mysplits = gameObject.GetComponent<splitter_functions>();

        Invoke(nameof(DestroyProjectile), lifeTime);

        for (int i = 0; i < inventory.container.Count; i++)
        {
            int amt = inventory.container[i].amount;
            ItemData mine = inventory.container[i].item;
            damage += amt * mine.damage;
            damage *= 1 + amt * mine.damage_mult;

            speed += amt * mine.shot_speed;
            if (mine.true_shot_speed_mult != 0)
            {
                speed *= Mathf.Pow(mine.true_shot_speed_mult, amt);
            }

            if (mine.true_angle_mult != 0)
            {
                angle_mult *= Mathf.Pow(mine.true_angle_mult, amt);
            }
        }
        damage *= .2f;
        speed *= 1.4f;

        transform.rotation = Quaternion.Euler(0f, 0f, transform.eulerAngles.z + (Random.Range(-180, 180)) * angle_mult);
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

        if (hitSomething)
        {
            DestroyProjectile();
        }

        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    void DestroyProjectile()
    {
        // Always trigger the splitter before destruction
        if (mysplits != null)
        {
            mysplits.destroy_splitter();
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
