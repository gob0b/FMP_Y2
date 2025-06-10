using UnityEngine;

public class PlayAnimationAfterDelay : MonoBehaviour
{
    public Animator animator;            // Reference to the Animator component
    public string animationTriggerName;  // Name of the trigger to play the animation
    public float delay = 2f;             // Delay before the animation plays

    private void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        StartCoroutine(PlayAnimationWithDelay());
    }

    private System.Collections.IEnumerator PlayAnimationWithDelay()
    {
        yield return new WaitForSeconds(delay);
        animator.SetTrigger(animationTriggerName);
    }
}
