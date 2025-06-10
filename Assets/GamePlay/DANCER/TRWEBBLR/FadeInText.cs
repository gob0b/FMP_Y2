using UnityEngine;
using TMPro;

public class FadeInText : MonoBehaviour
{
    public TextMeshProUGUI tmpText;
    public float delayBeforeFade = 2f;
    public float fadeDuration = 2f;

    private float timer = 0f;
    private bool isFading = false;

    void Start()
    {
        if (tmpText == null)
            tmpText = GetComponent<TextMeshProUGUI>();

        Color startColor = tmpText.color;
        startColor.a = 0f;
        tmpText.color = startColor;

        timer = 0f;
        isFading = false;
    }

    void Update()
    {
        if (!isFading)
        {
            timer += Time.deltaTime;
            if (timer >= delayBeforeFade)
            {
                isFading = true;
                timer = 0f;
            }
        }
        else if (tmpText.color.a < 1f)
        {
            float alpha = Mathf.Clamp01(timer / fadeDuration);
            Color newColor = tmpText.color;
            newColor.a = alpha;
            tmpText.color = newColor;

            timer += Time.deltaTime;
        }
    }
}
