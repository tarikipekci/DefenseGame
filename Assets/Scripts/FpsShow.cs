using TMPro;
using UnityEngine;

public class FpsShow : MonoBehaviour
{
    public TextMeshProUGUI fpsText;
    private float pollingTime = 1f;
    private float time;
    private int frameCount;
    private bool fpsShowing;

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    private void Update()
    {
        time += Time.deltaTime;
        frameCount++;
        if (time >= pollingTime)
        {
            var frameRate = Mathf.RoundToInt(frameCount / time);
            fpsText.text = frameRate.ToString() + " FPS";
            time -= pollingTime;
            frameCount = 0;
        }
    }

    public void DisplayFps()
    {
        if (fpsShowing == false)
        {
            fpsText.gameObject.SetActive(true);
            fpsShowing = true;
        }
        else
        {
            fpsText.gameObject.SetActive(false);
            fpsShowing = false;
        }
    }
}