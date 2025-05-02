using UnityEngine;

public class jumping_behavior : MonoBehaviour
{
    public float z_speed = 5f;         // Jump strength
    public float g = 9.81f;            // Gravity (positive Z = down)
    public float jumptime = 2f;        // Time between jumps
    public float lateral_speed = 3f;   // Lateral movement speed

    public float minScale = 0.8f;      // Scale when far (high in air)
    public float maxScale = 1.2f;      // Scale when close (on ground)

    private Transform player;
    private Rigidbody rb;

    private float z_0;                 // Ground Z level
    private float jumptimer;

    private bool isJumping = false;
    private float groundedTimer = 0f;
    private bool hasShot = false;

    public Transform tounge_pos;
    public GameObject projectile;

    public float rotationSpeed;

    [Header("Attack Sounds")]
    public AudioClip[] attackSounds; // Array of attack sound clips
    public float attackSoundVolume = 1f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }

        z_0 = transform.position.z;
        jumptimer = jumptime;
    }

    void FixedUpdate()
    {
        jumptimer -= Time.deltaTime;

        if (isJumping)
        {
            rb.AddForce(new Vector3(0, 0, g), ForceMode.Acceleration);

            if (transform.position.z >= z_0 - 0.01f && rb.linearVelocity.z >= 0f)
            {
                isJumping = false;
                rb.linearVelocity = Vector3.zero;

                Vector3 pos = transform.position;
                pos.z = z_0;
                transform.position = pos;

                groundedTimer = 0f;
                hasShot = false;
            }
        }
        else
        {
            groundedTimer += Time.deltaTime;

            if (!hasShot && groundedTimer >= 1f)
            {
                Shoot();
                hasShot = true;
            }

            Vector2 direction = player.position - transform.position;
            float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }

        if (!isJumping && jumptimer <= 0f)
        {
            Jump();
            jumptimer = jumptime;
        }

        UpdateScale();
    }

    void Jump()
    {
        if (player == null) return;

        Vector3 direction = new Vector3(
            player.position.x - transform.position.x,
            player.position.y - transform.position.y,
            0
        ).normalized;

        rb.linearVelocity = direction * lateral_speed;
        rb.linearVelocity += new Vector3(0, 0, -z_speed);
        isJumping = true;
    }

    void UpdateScale()
    {
        float zDist = z_0 - transform.position.z;
        float t = Mathf.Clamp01(zDist / 5f);
        float scale = Mathf.Lerp(minScale, maxScale, t);
        transform.localScale = new Vector3(scale, scale, scale);
    }

    void Shoot()
    {
        Instantiate(projectile, tounge_pos.position, Quaternion.identity);

        // Play random attack sound
        if (attackSounds.Length > 0)
        {
            AudioClip clip = attackSounds[Random.Range(0, attackSounds.Length)];
            AudioSource.PlayClipAtPoint(clip, transform.position, attackSoundVolume);
        }
    }
}
