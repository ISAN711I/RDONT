using UnityEngine;

public class pew : MonoBehaviour
{
    public float speed;
    public float lifeTime;
    public float distance;
    public int damage;
    public LayerMask whatIsSolid;
    public string Freak = "Freaky";

    public GameObject destroyEffect;

    [Header("Optional Audio")]
    public AudioClip shootSound;

    private void Start()
    {
        // Play shoot sound at the projectile's position
        if (shootSound != null)
        {
            AudioSource.PlayClipAtPoint(shootSound, transform.position);
        }

        Invoke(nameof(DestroyProjectile), lifeTime);
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
        if (destroyEffect != null)
        {
            Instantiate(destroyEffect, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
