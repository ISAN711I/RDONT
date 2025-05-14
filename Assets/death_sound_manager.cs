using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using System.Collections;
public class death_sound_manager : MonoBehaviour
{
    public float fadeDuration = 5f;
    public AudioClip explode;
    public float explode_const;
    public AudioClip music;
    public float music_const;
    public AudioClip ringing;
    public float ringing_const;

    private float fadeTimer;

    public AudioSource audioSource_exp;
    public AudioSource audioSource_ring;
    public AudioSource audioSource_music;

    public RawImage targetImage;
    public VideoPlayer videoPlayer;  // <-- Reference to VideoPlayer

    float music_volume(float t)
    {
        return music_const * Mathf.Atan(t);
    }
    float ringing_volume(float t)
    {
        return ringing_const * 1 / (1 + Mathf.Pow(t, 10));
    }

    void Start()
    {
        videoPlayer.Stop();              // Stop playback just in case
        videoPlayer.frame = 0;           // Set to first frame (your blank one)
        videoPlayer.time = 0;            // Redundant, but ensures zero timestamp
        videoPlayer.Play();              // Start playback
        videoPlayer.Pause();
        Color originalColor = targetImage.color;

        audioSource_exp.clip = explode;
        audioSource_exp.volume = explode_const;
        audioSource_exp.Play();

        audioSource_ring.clip = ringing;
        audioSource_ring.volume = ringing_const;
        audioSource_ring.time = 39f;
        audioSource_ring.Play();

        audioSource_music.clip = music;
        audioSource_music.volume = 0f;
        audioSource_music.Play();

        fadeTimer = 0;

        StartCoroutine(FadeOutRawImage());
        StartCoroutine(PlayVideoAfterDelay(2f)); // <-- Start delayed video
    }

    void Update()
    {
        fadeTimer += Time.deltaTime;
        audioSource_ring.volume = ringing_const * ringing_volume(fadeTimer);
        audioSource_music.volume = music_const * music_volume(fadeTimer);
    }

    private IEnumerator FadeOutRawImage()
    {
        if (targetImage == null) yield break;

        Color originalColor = targetImage.color;
        float elapsed = 0f;

        yield return new WaitForSeconds(9f); // Wait 5 seconds before starting fade

        while (elapsed < fadeDuration)
        {
            float alpha = Mathf.Lerp(originalColor.a, 0f, elapsed / fadeDuration);
            targetImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            elapsed += Time.deltaTime;
            yield return null;
        }

        targetImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
        targetImage.enabled = false;
    }

    private IEnumerator PlayVideoAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (videoPlayer != null)
        {
            videoPlayer.Play();
        }
    }
}
