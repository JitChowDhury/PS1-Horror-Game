using System;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float playerReach = 3f;
    public LayerMask interactLayer;
    Interactable currentInteractble;

    void Update()
    {
        CheckInteraction();
        if (Input.GetKeyDown(KeyCode.E) && currentInteractble != null)
        {
            currentInteractble.Interact();
        }
    }
    void CheckInteraction()
    {
        RaycastHit hit;
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

        if (Physics.Raycast(ray, out hit, playerReach, interactLayer))
        {
            if (hit.collider.CompareTag("Interactable"))
            {
                Interactable newInteractable = hit.collider.GetComponent<Interactable>();

                if (currentInteractble && newInteractable != currentInteractble)
                {
                    currentInteractble.DisableOutline();
                }


                if (newInteractable.enabled)
                {
                    SetNewCurrentInteractable(newInteractable);
                }
                else
                {
                    DisableCurrentInteractable();
                }
            }
            else
            {
                DisableCurrentInteractable();
            }


        }
        else
        {
            DisableCurrentInteractable();
        }
    }

    private void SetNewCurrentInteractable(Interactable newInteractable)
    {
        currentInteractble = newInteractable;
        currentInteractble.EnableOutline();
        HudController.instance.EnableInteractionText(currentInteractble.message);

    }

    private void DisableCurrentInteractable()
    {
        HudController.instance.DisableInteractionText();
        if (currentInteractble)
        {
            currentInteractble.DisableOutline();
            currentInteractble = null;
        }
    }
}
