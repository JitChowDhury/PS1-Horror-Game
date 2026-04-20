using UnityEngine;

public class AnimatorToggle : MonoBehaviour
{
    public Animator animator;
    public string boolParameter = "Open";

    private bool state = false;

    public void Toggle()
    {
        state = !state;
        animator.SetBool(boolParameter, state);
    }
}