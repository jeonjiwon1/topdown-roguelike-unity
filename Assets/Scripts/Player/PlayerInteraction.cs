using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("상호작용 키")]
    [SerializeField] private KeyCode interactKey = KeyCode.Space;

    private InteractableBase currentInteractable;

    private void Update()
    {
        if (currentInteractable == null)
        {
            return;
        }

        if (Input.GetKeyDown(interactKey))
        {
            currentInteractable.Interact(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        InteractableBase interactable = other.GetComponent<InteractableBase>();

        if (interactable != null)
        {
            currentInteractable = interactable;
            Debug.Log("상호작용 가능 : " + other.name + " / " + interactable.GetInteractPrompt());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        InteractableBase interactable = other.GetComponent<InteractableBase>();

        if (interactable != null && currentInteractable == interactable)
        {
            currentInteractable = null;
            Debug.Log("상호작용 종료 : " + other.name);
        }
    }
}