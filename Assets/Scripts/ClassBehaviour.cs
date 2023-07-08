using UnityEngine;
using UnityEngine.SceneManagement;

public class ClassBehaviour : MonoBehaviour
{
    public static bool isArcher, isMage, isKnight;
    public void SelectArcher()
    {
        isArcher = true;
        isMage = false;
        isKnight = false;
        StartTheGame();
    }

    public void SelectMage()
    {
        isMage = true;
        isArcher = false;
        isKnight = false;
        StartTheGame();
    }

    public void SelectKnight()
    {
        isKnight = true;
        isArcher = false;
        isMage = false;
        StartTheGame();
    }

    private void StartTheGame()
    {
        SceneManager.LoadScene("Scenes/Level");
    }
}