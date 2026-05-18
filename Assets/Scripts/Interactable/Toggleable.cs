using UnityEngine;

public class Toggleable : MonoBehaviour
{
    [SerializeField] Behaviour target;

    public void SetActive(bool state)
    {
        target.enabled = state;
    }
}
