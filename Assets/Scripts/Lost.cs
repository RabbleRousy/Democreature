using UnityEngine;

public class Lost : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(GameObject.Find("Audio"));
    }
}
