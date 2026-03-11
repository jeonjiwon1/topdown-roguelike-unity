using UnityEngine;

public abstract class InteractableBase : MonoBehaviour
{
    [Header("상호작용 설정")]
    [SerializeField] protected string interactPrompt = "Press Space";

    public string GetInteractPrompt()
    {
        return interactPrompt;
    }

    public abstract void Interact(GameObject interactor);
}