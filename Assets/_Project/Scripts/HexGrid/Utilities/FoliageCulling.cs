using UnityEngine;
using System.Collections.Generic; // Required for Lists

public class FoliageCulling : MonoBehaviour
{
    public float checkRadius = 2.0f;
    public string targetTag = "Foliage";

    // This list stores everything we hide
    private List<GameObject> hiddenFoliage = new List<GameObject>();

    void Start()
    {
        ClearArea();
    }

    void ClearArea()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, checkRadius);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag(targetTag))
            {
                GameObject foliage = hitCollider.gameObject;
                
                // Add to list and deactivate
                hiddenFoliage.Add(foliage);
                foliage.SetActive(false);
            }
        }
    }

    // Runs automatically when the object is Destroyed
    private void OnDestroy()
    {
        ReactivateFoliage();
    }

    // Optional: Runs if the object is just toggled off in the hierarchy
    private void OnDisable()
    {
        // Only run if the application is actually playing to avoid editor errors
        if (gameObject.scene.isLoaded) 
        {
            ReactivateFoliage();
        }
    }

    void ReactivateFoliage()
    {
        foreach (GameObject foliage in hiddenFoliage)
        {
            // Check if the foliage still exists (it might have been deleted elsewhere)
            if (foliage != null)
            {
                foliage.SetActive(true);
            }
        }
        
        // Clear the list so we don't try to reactivate twice
        hiddenFoliage.Clear();
    }
}