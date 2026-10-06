using UnityEngine;
using UnityEngine.InputSystem;

public class InspectController : MonoBehaviour
{
    public void CloseInspect()
    {
        UIManager.Instance.HideInspect();
    }

    public void OnBackgroundClicked()
    {
        CloseInspect();

    }
}