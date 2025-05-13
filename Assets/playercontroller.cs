using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class playercontroller : MonoBehaviour
{
    public InventoryObject inventory;
    public PlayerHealth me;

    [Header("Projectile & Movement")]
    public GameObject projectile;
    public int num_shots = 1;
    public float angle = 0;
    public float initial_speed = 5f;
    private float speed;
    public float attackspeed = 10/3;
    public float startTimeBtwShots;
    public float offset;

    [Header("Sprites")]
    public Sprite spriteUp;
    public Sprite spriteDown;
    public Sprite spriteLeft;
    public Sprite spriteRight;

    [Header("Dash Visuals")]
    public Sprite dashSprite;
    public ParticleSystem dashEffectPrefab; 

    [Header("Dash")]
    public float dash_power = 10f;
    public float dash_damage;
    public float dash_duration = 0.2f;
    private float dash_timer;
    private bool isDashing;
    private Vector2 dashDirection;

    [Header("Stamina")]
    public float max_stamina = 5f;
    private float stamina;
    public float sprint_mult = 1.5f;
    public float dash_kill_regen;

    [Header("UI")]
    public HealthBar statusBar;

    private float timeBtwShots;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        stamina = max_stamina;

        if (statusBar != null)
        {
            statusBar.SetMaxStamina(max_stamina);
            statusBar.SetStamina(stamina);
        }

        for(int i = 0; i < inventory.container.Count;i++)
        {
            int amt = inventory.container[i].amount;
            ItemData mine = inventory.container[i].item;
            initial_speed += amt*mine.speed;
            initial_speed *= 1+  amt * mine.speed_mult;

            attackspeed += amt * mine.attack_speed;
            attackspeed *= 1  + amt * mine.attack_speed_mult;

            num_shots += amt * mine.num_shots;
            angle += amt * mine.shot_angle;
            if (mine.true_angle_mult != 0)
            {
                angle *= Mathf.Pow(mine.true_angle_mult, amt);
            }
        }




        startTimeBtwShots = 1 / (attackspeed);
    }

    void Update()
    {
        // Regenerate stamina
        if (stamina < max_stamina)
        {
            stamina += Time.deltaTime;
            if (stamina > max_stamina) stamina = max_stamina;
            if (statusBar != null) statusBar.SetStamina(stamina);
        }

        // Sprinting
        if (Input.GetKey(KeyCode.LeftShift) && stamina > 0f)
        {
            speed = initial_speed * sprint_mult;
            stamina -= Time.deltaTime;
            if (stamina < 0f) stamina = 0f;
            if (statusBar != null) statusBar.SetStamina(stamina);
        }
        else
        {
            speed = initial_speed;
        }

        // Aiming
        Vector3 diff = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        diff.z = 0f;
        float rot = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;

        // Movement input
        if (!isDashing)
        {
            moveInput.x = Input.GetAxisRaw("Horizontal");
            moveInput.y = Input.GetAxisRaw("Vertical");
            moveInput.Normalize();
        }

        if (!isDashing) UpdateSpriteDirection(moveInput);

        // Shooting
        if (timeBtwShots <= 0)
        {
            if (Input.GetMouseButton(0))
            {
                for (int i = 0; i < num_shots; i++)
                {
                    Instantiate(projectile, transform.position, Quaternion.Euler(0f, 0f, rot + angle*(Random.value-.5f)));
                    timeBtwShots = startTimeBtwShots;
                }
            }
        }
        else
        {
            timeBtwShots -= Time.deltaTime;
        }

        // Dash
        if (Input.GetMouseButtonDown(1) && !isDashing && stamina >= max_stamina)
        {
            stamina = 0f;
            if (statusBar != null) statusBar.SetStamina(stamina);

            dashDirection = diff.normalized;
            rb.linearVelocity = Vector2.zero;
            rb.AddForce(dashDirection * dash_power, ForceMode2D.Impulse);

            isDashing = true;
            dash_timer = dash_duration;
            me.canbehit = false;

            // Switch to dash sprite and rotate player
            spriteRenderer.sprite = dashSprite;
            float angle = Mathf.Atan2(dashDirection.y, dashDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);

            // ✅ Play dash particle effect
            if (dashEffectPrefab != null)
            {
                ParticleSystem dashFX = Instantiate(dashEffectPrefab, transform.position, Quaternion.identity);
                dashFX.transform.SetParent(transform); // follow the player
                Destroy(dashFX.gameObject, 1f); // auto-destroy after 1 second
            }
        }

        // Dash timer
        if (isDashing)
        {
            dash_timer -= Time.deltaTime;
            if (dash_timer <= 0f)
            {
                isDashing = false;
                me.canbehit = true;

                // Reset rotation and revert to directional sprite
                transform.rotation = Quaternion.identity;
                UpdateSpriteDirection(moveInput);
            }
        }
    }

    void FixedUpdate()
    {
        if (!isDashing)
        {
            rb.linearVelocity = moveInput * speed;
        }
    }

    void UpdateSpriteDirection(Vector2 direction)
    {
        if (direction == Vector2.zero) return;

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            spriteRenderer.sprite = direction.x > 0 ? spriteRight : spriteLeft;
        }
        else
        {
            spriteRenderer.sprite = direction.y > 0 ? spriteUp : spriteDown;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDashing)
        {
            Enemy_health enemy = collision.gameObject.GetComponent<Enemy_health>();
            if (enemy != null)
            {
                enemy.TakeDamage(dash_damage);
                stamina += dash_kill_regen;
                if (stamina > max_stamina) stamina = max_stamina;
                if (statusBar != null) statusBar.SetStamina(stamina);
            }
        }
    }
}
