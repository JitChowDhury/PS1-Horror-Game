using TMPro;
using UnityEngine;

public class HudController : MonoBehaviour
{
    public static HudController instance;

    private void Awake()
    {
        instance = this;
    }

    [SerializeField] TMP_Text interactionText;

    public void EnableInteractionText(string Text)
    {
        interactionText.text = Text;
        interactionText.gameObject.SetActive(true);
    }

    public void DisableInteractionText()
    {
        interactionText.gameObject.SetActive(false);
    }
}
