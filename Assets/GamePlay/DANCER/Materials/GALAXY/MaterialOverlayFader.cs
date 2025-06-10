using UnityEngine;

public class MaterialOverlayFader : MonoBehaviour
{
    [Header("Overlay Settings")]
    public Material overlayMaterial;           // Material to overlay at start (must support transparency)
    public float fadeDuration = 2f;            // Time in seconds to fade out

    private Material[] originalMaterials;       // Original materials of the GameObject
    private Material fadingMaterialInstance;    // Instance of the overlay material
    private Renderer rend;
    private float timer = 0f;
    private bool fading = false;

    void Start()
    {
        rend = GetComponent<Renderer>();

        if (overlayMaterial == null)
        {
            Debug.LogError("Overlay Material not assigned.");
            return;
        }

        // Store original materials
        originalMaterials = rend.sharedMaterials;

        // Create a copy of the overlay material so we can fade it independently
        fadingMaterialInstance = new Material(overlayMaterial);

        // Apply the fading material as the only material
        rend.material = fadingMaterialInstance;

        // Start fading
        fading = true;
    }

    void Update()
    {
        if (!fading) return;

        timer += Time.deltaTime;
        float alpha = Mathf.Clamp01(1f - (timer / fadeDuration));

        // Apply fading to material (assuming it uses _Color with alpha)
        if (fadingMaterialInstance.HasProperty("_Color"))
        {
            Color col = fadingMaterialInstance.color;
            fadingMaterialInstance.color = new Color(col.r, col.g, col.b, alpha);
        }

        if (timer >= fadeDuration)
        {
            // Restore original materials
            rend.materials = originalMaterials;
            fading = false;
        }
    }
}
