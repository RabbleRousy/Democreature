using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("BeanTesting");
        SceneManager.UnloadSceneAsync("MainMenu");
    }
    
    public void QuitGame() => Application.Quit();
}
