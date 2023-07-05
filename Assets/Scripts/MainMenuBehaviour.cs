using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuBehaviour : MonoBehaviour
{
    public void StartTheGame()
    {
        SceneManager.LoadScene("Level");
    }
    
    public void ExitTheGame()
    {
        Application.Quit();
    }
}