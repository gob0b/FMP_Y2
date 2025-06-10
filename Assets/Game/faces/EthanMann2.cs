using UnityEngine;

public class EthanMann2 : MonoBehaviour
{
    [Tooltip("The GameObject to hide and then show.")]
    public GameObject targetObject;

    [Tooltip("Time in seconds to keep the GameObject hidden.")]
    public float delayBeforeAppear = 5f;

    private void Start()
    {
        if (targetObject != null)
        {
            targetObject.SetActive(false); // Hide at start
            Invoke(nameof(ShowObject), delayBeforeAppear); // Schedule showing
        }
        else
        {
            Debug.LogWarning("EthanMann2: No targetObject assigned.");
        }
    }

    void ShowObject()
    {
        if (targetObject != null)
        {
            targetObject.SetActive(true); // Show after delay
        }
    }
}
