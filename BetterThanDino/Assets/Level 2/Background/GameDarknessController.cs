using UnityEngine;
using UnityEngine.UI; // Required for UI components

public class GameDarknessController : MonoBehaviour
{
    public Image darknessOverlay; // Assign the UI Image here in the Inspector
    public float interval = 7f; // Time between darkness events
    public float darknessDuration = 3f; // How long the darkness lasts
    public float fadeDuration = 1f; // How long it takes to fade in/out

    private Coroutine darknessRoutine;

    void Start()
    {
        Color transparentBlack = new Color(0, 0, 0, 0); // Fully transparent
        darknessOverlay.color = transparentBlack; // Ensure the overlay starts fully transparent
        StartCoroutine(DarknessCycle());
    }

    System.Collections.IEnumerator DarknessCycle()
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);
            yield return FadeDarkness(1f); // Fade in to full darkness
            yield return new WaitForSeconds(darknessDuration);
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
            darknessOverlay.color = new Color(0, 0, 0, newAlpha); // Update the overlay color
            yield return null;
        }

        // Ensure the final alpha value is set exactly
        darknessOverlay.color = new Color(0, 0, 0, targetAlpha);
    }
}