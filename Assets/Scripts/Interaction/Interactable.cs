using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{

    Outline outline;
    public string message;
    public UnityEvent onInteraction;
    public bool shouldUseOutline;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        if (shouldUseOutline)
        {
            outline = GetComponent<Outline>();
        }
        DisableOutline();

    }
    public void Interact()
    {
        onInteraction.Invoke();
    }

    public void DisableOutline()
    {
        if (shouldUseOutline)
            outline.enabled = false;
    }
    public void EnableOutline()
    {
        if (shouldUseOutline)
            outline.enabled = true;
    }




}
