using UnityEngine;
using System.Collections.Generic;

public class MultiObjectClipper : MonoBehaviour
{
    [Header("Clipping Objects")]
    public Transform objectA; // Can be parent with multiple children
    public Transform objectB; // The clipping object

    [Header("Materials")]
    public Material clippingMaterial;

    [Header("Settings")]
    public bool preserveOriginalColors = true;
    public bool includeInactiveChildren = false;

    private List<RendererData> objectARenderers = new List<RendererData>();
    private List<Material> clippingMaterials = new List<Material>();

    [System.Serializable]
    private class RendererData
    {
        public Renderer renderer;
        public Material[] originalMaterials;
        public Color[] originalColors;
    }

    // Shader property IDs
    private static readonly int ClipCenter = Shader.PropertyToID("_ClipCenter");
    private static readonly int ClipSize = Shader.PropertyToID("_ClipSize");
    private static readonly int ColorProperty = Shader.PropertyToID("_Color");

    void Start()
    {
        InitializeClipping();
    }

    void InitializeClipping()
    {
        if (objectA == null || objectB == null || clippingMaterial == null)
        {
            Debug.LogError("All objects and clipping material must be assigned!");
            return;
        }

        CollectRenderers();
        ApplyClippingMaterials();
    }

    void CollectRenderers()
    {
        objectARenderers.Clear();

        // Get all renderers from objectA and its children
        Renderer[] renderers = objectA.GetComponentsInChildren<Renderer>(includeInactiveChildren);

        foreach (Renderer renderer in renderers)
        {
            RendererData data = new RendererData();
            data.renderer = renderer;
            data.originalMaterials = renderer.materials;

            // Store original colors if preserving them
            if (preserveOriginalColors)
            {
                data.originalColors = new Color[data.originalMaterials.Length];
                for (int i = 0; i < data.originalMaterials.Length; i++)
                {
                    if (data.originalMaterials[i].HasProperty(ColorProperty))
                    {
                        data.originalColors[i] = data.originalMaterials[i].GetColor(ColorProperty);
                    }
                    else
                    {
                        data.originalColors[i] = Color.white;
                    }
                }
            }

            objectARenderers.Add(data);
        }

        Debug.Log($"Found {objectARenderers.Count} renderers to clip");
    }

    void ApplyClippingMaterials()
    {
        clippingMaterials.Clear();

        foreach (RendererData data in objectARenderers)
        {
            Material[] newMaterials = new Material[data.originalMaterials.Length];

            for (int i = 0; i < data.originalMaterials.Length; i++)
            {
                // Create a new instance of the clipping material
                Material newMaterial = new Material(clippingMaterial);

                // Preserve original color if enabled
                if (preserveOriginalColors && data.originalColors != null)
                {
                    if (newMaterial.HasProperty(ColorProperty))
                    {
                        newMaterial.SetColor(ColorProperty, data.originalColors[i]);
                    }
                }

                // Copy main texture if it exists
                if (data.originalMaterials[i].mainTexture != null)
                {
                    newMaterial.mainTexture = data.originalMaterials[i].mainTexture;
                }

                newMaterials[i] = newMaterial;
                clippingMaterials.Add(newMaterial);
            }

            data.renderer.materials = newMaterials;
        }
    }

    void Update()
    {
        UpdateClipping();
    }

    void UpdateClipping()
    {
        if (objectB == null || clippingMaterials.Count == 0) return;

        // Get clipping bounds from objectB
        Renderer clipRenderer = objectB.GetComponent<Renderer>();
        if (clipRenderer == null) return;

        Vector3 clipCenter = objectB.position;
        Vector3 clipSize = clipRenderer.bounds.size;

        // Update all clipping materials
        foreach (Material material in clippingMaterials)
        {
            if (material != null)
            {
                material.SetVector(ClipCenter, clipCenter);
                material.SetVector(ClipSize, clipSize);
            }
        }
    }

    // Method to restore original materials
    public void RestoreOriginalMaterials()
    {
        foreach (RendererData data in objectARenderers)
        {
            if (data.renderer != null)
            {
                data.renderer.materials = data.originalMaterials;
            }
        }

        // Clean up created materials
        foreach (Material material in clippingMaterials)
        {
            if (material != null)
            {
                DestroyImmediate(material);
            }
        }

        clippingMaterials.Clear();
    }

    // Method to refresh clipping (useful when child objects change)
    public void RefreshClipping()
    {
        RestoreOriginalMaterials();
        CollectRenderers();
        ApplyClippingMaterials();
    }

    void OnDestroy()
    {
        RestoreOriginalMaterials();
    }

    void OnDrawGizmos()
    {
        if (objectA != null && objectB != null)
        {
            // Draw bounds of both objects
            Gizmos.color = Color.green;
            if (objectA.GetComponent<Renderer>() != null)
            {
                Gizmos.DrawWireCube(objectA.GetComponent<Renderer>().bounds.center,
                                  objectA.GetComponent<Renderer>().bounds.size);
            }

            Gizmos.color = Color.red;
            if (objectB.GetComponent<Renderer>() != null)
            {
                Gizmos.DrawWireCube(objectB.GetComponent<Renderer>().bounds.center,
                                  objectB.GetComponent<Renderer>().bounds.size);
            }
        }
    }
}