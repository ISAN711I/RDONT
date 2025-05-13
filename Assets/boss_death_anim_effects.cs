using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class boss_death_anim_effects : MonoBehaviour
{
    public Image image;
    public AudioClip[] coolsounds;

    private bool thingshappening = false;
    private float soundTimer = 0f;

    public float startInterval = 1.0f;
    public float endInterval = 0.05f;
    public float startVolume = 0.1f;
    public float endVolume = 1.0f;
    public float rampDuration = 5f;

    private float effectTime = 0f;
    private float colorChangeTimer = 0f;
    public float colorChangeInterval = 0.05f; // How fast colors change when alpha = 1

    private PlayerHealth playe;

    void Start()
    {
        playe = GameObject.FindWithTag("Player").GetComponent<PlayerHealth>();
        if (image == null) image = GetComponent<Image>();

        Color color = image.color;
        color.a = 0f;
        image.color = color;
    }

    void Update()
    {
        if (thingshappening)
        {
            effectTime += Time.deltaTime;
            float t = Mathf.Clamp01(effectTime / rampDuration);

            // Lerp frequency and volume
            float currentInterval = Mathf.Lerp(startInterval, endInterval, t);
            float currentVolume = Mathf.Lerp(startVolume, endVolume, t);

            // Play sounds
            soundTimer -= Time.deltaTime;
            if (soundTimer <= 0f)
            {
                AudioClip clip = coolsounds[Random.Range(0, coolsounds.Length)];
                AudioSource.PlayClipAtPoint(clip, Vector3.zero, currentVolume);
                soundTimer = currentInterval;
                playe.TakeDamage(0);
            }

            // Update color alpha or flash random colors if alpha == 1
            Color color = image.color;
            if (color.a < 1f)
            {
                color.a += 0.2f * Time.deltaTime;
                color.a = Mathf.Clamp01(color.a);
                image.color = color;
            }
            else
            {
                colorChangeTimer -= Time.deltaTime;
                if (colorChangeTimer <= 0f)
                {
                    image.color = new Color(
                        Random.value,
                        Random.value,
                        Random.value,
                        1f // Keep alpha at 1
                    );
                    colorChangeTimer = colorChangeInterval;
                }
            }
        }
    }

    public void makethingshappen()
    {
        thingshappening = true;
        effectTime = 0f;
        Invoke("change_scene", 10f);
    }

    public void change_scene()
    {
        SceneManager.LoadScene(3);

    }
}
