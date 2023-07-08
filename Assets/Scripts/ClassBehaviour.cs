using UnityEngine;
using UnityEngine.SceneManagement;

public class ClassBehaviour : MonoBehaviour
{
    public static bool isArcher, isMage, isKnight;
    public void SelectArcher()
    {
        isArcher = true;
        StartTheGame();
    }

    public void SelectMage()
    {
        isMage = true;
        StartTheGame();
    }

    public void SelectKnight()
    {
        isKnight = true;
        StartTheGame();
    }

    private void StartTheGame()
    {
        SceneManager.LoadScene("Scenes/Level");
    }
}