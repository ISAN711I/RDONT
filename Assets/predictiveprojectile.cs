using UnityEngine;

public class predictiveprojectile : MonoBehaviour
{
    public float initial_velocity = 30f;

    private Transform player;
    private Vector2 playerv0;
    private Vector3 velocity;
    public float damage;
    public float times;
    void Start()
    {
        Invoke("kill",times);
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            Rigidbody2D playerrb = playerObj.GetComponent<Rigidbody2D>();

            if (playerrb != null)
            {
                playerv0 = playerrb.linearVelocity;

                Vector2 delta = player.position - transform.position;
                float distmag = delta.magnitude;

                // Estimate time to target
                float timeToTarget = distmag / initial_velocity;

                // Predict future position
                Vector2 predictedPos = (Vector2)player.position + playerv0 * timeToTarget;

                // Calculate velocity toward predicted position
                Vector2 direction = (predictedPos - (Vector2)transform.position).normalized;
                velocity = new Vector3(direction.x, direction.y, 0f) * initial_velocity;
            }
            else
            {
                Debug.LogError("Player object has no Rigidbody2D.");
            }
        }
        else
        {
            Debug.LogError("Player object not found with tag 'Player'.");
        }
    }

    void Update()
    {
        transform.position += velocity * Time.deltaTime;

    }
    void kill() {
        Destroy(gameObject);
    }
    private void OnCollisionEnter2D(Collision2D collision)
{
if (collision.gameObject.CompareTag("Player"))
{
PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
if (playerHealth != null)
{
playerHealth.TakeDamage(damage);
 Destroy(gameObject);
}
}
}
}
