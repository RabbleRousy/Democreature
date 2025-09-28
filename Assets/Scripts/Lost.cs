using UnityEngine;
using UnityEngine.SceneManagement;

public class Lost : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(GameObject.Find("Audio(Clone)"));
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("MainMenu");
        SceneManager.UnloadSceneAsync("Lost");
    }
}
