using UnityEngine;

public class Ac_unit_movement : MonoBehaviour
{
    public float followDistance;
    public float moveSpeed;
    public float error;
    private Transform player;
    int rotate_direction;

    public float shot_delay;
    private float shot_delay_timer;

    public float recoil_force;
    public float recoil_time;
    private float recoil_timer;

    public GameObject projectile;
    public GameObject particles;

    [Header("Attack Sounds")]
    public AudioClip[] attackSounds; // Array of attack sound clips
    public float attackSoundVolume = 1f; // Volume control

    void Start()
    {
        // Find the player object and get its transform
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        player = playerObj.transform;

        // Add random variation to follow distance, shot delay, and move speed
        followDistance += 2 * (Random.value - 0.5f);
        rotate_direction = Random.value < 0.5f ? -1 : 1;
        shot_delay += 2 * (Random.value - 0.5f);
        moveSpeed += 2 * (Random.value - 0.5f);
    }

    void Update()
    {
        // Get the distance between the enemy and the player
        float distance = Vector3.Distance(transform.position, player.position);

        // Calculate direction to the player
        Vector3 direction = (player.position - transform.position).normalized;

        // Calculate the angle to face the player on the Z-axis
        float angle2 = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        float rotationAdjustment = 90f;  // Adjust based on your model's orientation

        // Rotate the enemy on the Z-axis to face the player
        Quaternion rotation2 = Quaternion.Euler(0f, 0f, angle2 + rotationAdjustment);
        transform.rotation = rotation2;

        if (recoil_timer <= 0)
        {
            if (Mathf.Abs(distance - followDistance) > error)
            {
                // Move toward or away from player
                if (distance - followDistance < 0)
                {
                    transform.position -= direction * moveSpeed * Time.deltaTime;
                }
                else
                {
                    transform.position += direction * moveSpeed * Time.deltaTime;
                }
            }
            else
            {
                // Rotate around player
                Vector3 rotated = new Vector3(direction.y, -direction.x, 0);
                float rotate_speed = 2 * shot_delay_timer / shot_delay;
                transform.position += rotate_speed * rotate_direction * rotated * moveSpeed * Time.deltaTime;

                // Shoot projectile
                if(shot_delay_timer <= .1) {
                    Instantiate(particles, transform.position, Quaternion.identity);
                } 
                if (shot_delay_timer <= 0)
                {
                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    Quaternion rotation = Quaternion.Euler(0f, 0f, angle);
                    Instantiate(projectile, transform.position, rotation);


                    // Play random attack sound
                    if (attackSounds.Length > 0)
                    {
                        AudioClip clip = attackSounds[Random.Range(0, attackSounds.Length)];
                        AudioSource.PlayClipAtPoint(clip, transform.position, attackSoundVolume);
                    }

                    rotate_direction = Random.value < 0.5f ? -1 : 1;
                    shot_delay_timer = shot_delay;
                    recoil_timer = recoil_time;
                }
                else
                {
                    shot_delay_timer -= Time.deltaTime;
                }
            }
        }
        else
        {
            // Apply recoil
            transform.position -= direction * recoil_timer * recoil_timer * recoil_force * Time.deltaTime;
            recoil_timer -= Time.deltaTime;
        }
    }
}
