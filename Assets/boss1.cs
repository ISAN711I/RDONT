using UnityEngine;

public class boss1 : MonoBehaviour
{
    float attacktimer = 2;
    int currentrand = 1;
    public GameObject basicshot;

    bool flamethrower = false;

    private Rigidbody2D rb;

    public float flamethrower_speed;

    private GameObject player;
    private Transform target;
    public GameObject shotpos;
    public float numShots;

    float phase_mult = 1;
    public float phase_2_mult;

    public float chargeSpeed = 10;

    private Enemy_health me;

    public Sprite[] allsprites;

    bool is_enraged = false;

    int flamethrower_previous = 0;

    public float spawn_sound_volume;
    public AudioClip spawn_sound;
    public AudioClip death_sound;
    public AudioClip gunshot;
    bool dead = false;

    boss_death_anim_effects my_effects;
    void Start()
    {
        my_effects = GameObject.FindWithTag("GameController").GetComponent<boss_death_anim_effects>();

        AudioSource.PlayClipAtPoint(spawn_sound, transform.position, spawn_sound_volume);
        setSprite(1);
        GameObject targetObj = GameObject.FindGameObjectWithTag("Player");
        me = GetComponent<Enemy_health>();
            target = targetObj.transform;
            rb = GetComponent<Rigidbody2D>();
        shotpos.transform.localPosition = new Vector3(.08f, -0.16f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        if(me.health > 0 ){
        if(me.health < 40)
        {
            phase_mult = phase_2_mult;
            is_enraged = true;
            shotpos.transform.localPosition = new Vector3(.16f, -0.36f, 0f);
        }
        Vector2 direction = (Vector2)(target.position - transform.position);
        direction.Normalize();

        attacktimer -= Time.deltaTime;
        if(attacktimer <= 0)
        {
            if (is_enraged == false)
            {
                setSprite(1);
            } else
            {
                setSprite(2);
            }
            flamethrower = false;
           switch(Random.Range(1,4))
            {
                case 3:
                    if (flamethrower_previous == 0)
                    {
                        attacktimer = 6 / Mathf.Sqrt(phase_mult);
                        flamethrowerattack();
                    } else
                    {
                        attacktimer = 1;
                        charge();
                        flamethrower_previous -=1;
                    }
                    break;
                case 2:
                    attacktimer = 1;
                    charge();
                    flamethrower_previous -=1;
                    break;
                case 1:

                    flamethrower_previous = 3;
                    attacktimer = .4f / phase_mult;
                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.Euler(0, 0, angle);
                    Invoke("shotgunAttack", .2f);

                    break;
            }
        } else if(flamethrower == true)
        {
            if (is_enraged == false)
            {
                setSprite(0);
            }
            else
            {
                setSprite(3);
            }
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            float currentAngle = rb.rotation; // In degrees

            float angleDiff = Mathf.DeltaAngle(currentAngle, angle);

            float angularVelocity = Mathf.Sign(angleDiff) *phase_mult*flamethrower_speed * Time.fixedDeltaTime;

            rb.angularVelocity += angularVelocity;
            Instantiate(basicshot, shotpos.transform.position, Quaternion.Euler(0, 0, transform.eulerAngles.z + 40*Random.value));
        } else
        {
           
            rb.angularVelocity = 0;
        }
        } else {
            if(dead == false) {
            AudioSource.PlayClipAtPoint(death_sound, transform.position, 50);
                        dead = true;
            }

            rb.rotation = 0;
            rb.linearVelocity = new Vector2(0,0);
             setSprite(4);
             Invoke("Die",8.96f);
        }
    }

    void shotgunAttack()
    {

        if (is_enraged == false)
        {
            setSprite(0);
        }
        else
        {
            setSprite(3);
        }
        for (int i = 0; i <= numShots; i++ )
        {
            Instantiate(basicshot, shotpos.transform.position, Quaternion.Euler(0, 0, transform.eulerAngles.z + 30*phase_mult * (Random.value-.5f)));

        }

    }
    void charge()
    {
        Vector2 direction = (Vector2)(target.position - transform.position);
        direction.Normalize();
        rb.AddForce(direction*chargeSpeed,ForceMode2D.Impulse);
        attacktimer = .6f / phase_mult;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
        Invoke("shotgunAttack", .15f);

    }
    void flamethrowerattack()
    {
        flamethrower = true;


    }


    void setSprite(int num)
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = allsprites[num];
    }
    void Die() {
        AudioSource.PlayClipAtPoint(gunshot, transform.position, 50);
        my_effects.makethingshappen();
        Destroy(gameObject);
    }
}
