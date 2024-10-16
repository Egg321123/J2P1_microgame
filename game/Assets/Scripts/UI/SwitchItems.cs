using UnityEngine;

// handy utility to set self to inactive and a target gameobject to active! :3
public class SwitchItems : MonoBehaviour
{
    [SerializeField] GameObject gO2;
    public void Switch()
    {
        gO2.SetActive(true);
        gameObject.SetActive(false);
    }
}
