using UnityEngine;

public class HeartUI : MonoBehaviour
{
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void Break()
    {
        anim.SetTrigger("Break");
    }

    public void Restore()
    {
        gameObject.SetActive(true);
        anim.SetTrigger("Restore");
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void SetFullInstant()
    {
        gameObject.SetActive(true);
    }

    public void SetEmptyInstant()
    {
        gameObject.SetActive(false);
    }
}