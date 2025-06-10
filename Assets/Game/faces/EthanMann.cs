using UnityEngine;

public class EthanMann : MonoBehaviour
{
    [Tooltip("The GameObject to show and then hide.")]
    public GameObject targetObject;

    [Tooltip("Time in seconds before the GameObject disappears.")]
    public float delayBeforeDisappear = 5f;

    private void Start()
    {
        if (targetObject != null)
        {
            targetObject.SetActive(true);
            Invoke(nameof(HideObject), delayBeforeDisappear);
        }
        else
        {
            Debug.LogWarning("EthanMann: No targetObject assigned.");
        }
    }

    void HideObject()
    {
        if (targetObject != null)
        {
            targetObject.SetActive(false);
        }
    }
}
