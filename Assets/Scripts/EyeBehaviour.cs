using UnityEngine;
using Random = UnityEngine.Random;

public class EyeBehaviour : MonoBehaviour
{
    [SerializeField] private Transform[] eyeLocations;
    [SerializeField] private GameObject eye;
    [SerializeField] private float animationDuration = 2.2f, durationReset;
    private int count = 5;

    private void Start()
    {
        durationReset = animationDuration;
    }

    private void Update()
    {
        if (animationDuration > 0)
        {
            animationDuration -= Time.deltaTime;
        }

        if (animationDuration <= 0)
        {
            animationDuration = durationReset;
            SpawnEye();
        }
    }

    private void SpawnEye()
    {
        for (var i = 0; i < count; i++)
        {
            var randomElement = Random.Range(0, eyeLocations.Length);
            var newEye = Instantiate(eye, eyeLocations[randomElement].position, Quaternion.identity);
            Destroy(newEye, animationDuration);
        }
    }
}