using UnityEngine;
using UnityEngine.UI; // Required for UI components

public class GameDarknessController : MonoBehaviour
{
    public Image darknessOverlay; // Main overlay for overall darkness
    public float interval = 7f; // Time between darkness events
    public float darknessDuration = 3f; // How long the darkness lasts
    public float fadeDuration = 1f; // How long it takes to fade in/out

    public AudioClip fadeInSound; // Sound effect for fading in (darkness starts)
    public AudioClip fadeOutSound; // Sound effect for fading out (darkness ends)
    private AudioSource audioSource;

    private Coroutine darknessRoutine;

    void Start()
    {
        // Ensure the overlay starts fully transparent
        darknessOverlay.color = new Color(0, 0, 0, 0);

        // Set up the AudioSource component
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;

        // Start the darkness cycle
        StartCoroutine(DarknessCycle());
    }

    System.Collections.IEnumerator DarknessCycle()
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);
            
            // Play fade-in sound
            PlaySound(fadeInSound);
            
            yield return FadeDarkness(1f); // Fade in to full darkness
            yield return new WaitForSeconds(darknessDuration);
            
            // Play fade-out sound
            PlaySound(fadeOutSound);
            
            yield return FadeDarkness(0f); // Fade out to transparency
        }
    }

    System.Collections.IEnumerator FadeDarkness(float targetAlpha)
    {
        float startAlpha = darknessOverlay.color.a;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsed / fadeDuration);

            // Update the overlay alpha
            darknessOverlay.color = new Color(0, 0, 0, newAlpha);

            yield return null;
        }

        // Ensure final alpha value is set
        darknessOverlay.color = new Color(0, 0, 0, targetAlpha);
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
