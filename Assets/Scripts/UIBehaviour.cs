using UnityEngine;

public class UIBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private Animator pauseMenuAnimator;
    public bool isMenuOpened;
    private static readonly int Pressed = Animator.StringToHash("pressed");
    public void SetPauseMenu()
    {
        switch (isMenuOpened)
        {
            case true:
                ClosePauseMenu();
                break;
            case false:
                OpenPauseMenu();
                break;
        }
    }

    private void OpenPauseMenu()
    {
        isMenuOpened = true;
        pauseMenu.SetActive(true);
        pauseMenuAnimator.SetTrigger(Pressed);
        Time.timeScale = 0f;
    }

    private void ClosePauseMenu()
    {
        isMenuOpened = false;
        pauseMenu.SetActive(false);
        pauseMenuAnimator.SetTrigger(Pressed);
        Time.timeScale = 1f;
    }
}