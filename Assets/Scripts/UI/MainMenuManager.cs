using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject audio;

    private void Start()
    {
        DontDestroyOnLoad(audio);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("BeanTesting");
        SceneManager.UnloadSceneAsync("MainMenu");
    }
    
    public void QuitGame() => Application.Quit();
}
