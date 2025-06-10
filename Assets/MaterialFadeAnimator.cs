using UnityEngine;

public class MaterialFadeAnimator : MonoBehaviour
{
    [Header("References")]
    public GameObject targetObject;             // The object with original materials
    public Material overlayMaterial;            // The material to fade out
    public string animationTriggerName = "Start"; // Animator trigger to call

    [Header("Timing")]
    public float startDelay = 1f;               // Delay before starting the fade
    public float fadeDuration = 2f;             // Duration of the fade effect

    private Material[] originalMaterials;
    private Renderer rend;
    private Animator animator;
    private float fadeTimer = 0f;
    private bool isFading = false;

    private void Start()
    {
        if (targetObject == null || overlayMaterial == null)
        {
            Debug.LogError("Please assign the Target Object and Overlay Material.");
            return;
        }

        rend = targetObject.GetComponent<Renderer>();
        animator = targetObject.GetComponent<Animator>();

        // Store original materials
        originalMaterials = rend.materials;

        // Replace materials with overlay material
        Material[] tempMats = new Material[originalMaterials.Length];
        for (int i = 0; i < tempMats.Length; i++)
            tempMats[i] = new Material(overlayMaterial); // Instance to avoid modifying shared

        rend.materials = tempMats;

        // Start fading after delay
        Invoke(nameof(BeginFade), startDelay);
    }

    void BeginFade()
    {
        isFading = true;
        fadeTimer = 0f;
    }

    void Update()
    {
        if (!isFading || rend == null) return;

        fadeTimer += Time.deltaTime;
        float t = Mathf.Clamp01(fadeTimer / fadeDuration);

        // Apply transparency to all overlay materials
        foreach (Material mat in rend.materials)
        {
            if (mat.HasProperty("_Color"))
            {
                Color col = mat.color;
                col.a = 1f - t;
                mat.color = col;
            }
        }

        // Check if fade is complete
        if (t >= 1f)
        {
            // Restore original materials
            rend.materials = originalMaterials;

            // Trigger animation
            if (animator != null && !string.IsNullOrEmpty(animationTriggerName))
            {
                animator.SetTrigger(animationTriggerName);
            }

            isFading = false;
        }
    }
}

