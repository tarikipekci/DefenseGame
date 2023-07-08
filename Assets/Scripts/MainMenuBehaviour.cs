using UnityEngine;

public class MainMenuBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject classPanel;
    
    public void StartTheGame()
    {
       classPanel.SetActive(true);
    }
    
    public void ExitTheGame()
    {
        Application.Quit();
    }
}