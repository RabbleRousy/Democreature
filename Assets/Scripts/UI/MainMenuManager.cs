using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject audioPrefab;

    private void Start()
    {
        if (GameObject.Find("Audio(Clone)") != null) return;
        
        var audio = Instantiate(audioPrefab);
        DontDestroyOnLoad(audio);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("BeanTesting");
        SceneManager.UnloadSceneAsync("MainMenu");
    }
    
    public void QuitGame() => Application.Quit();
}
