using UnityEngine;

public class FireBallSkillBehaviour : MonoBehaviour
{
    [SerializeField] private float FireballSpeed;
    [SerializeField] private float circleRadius;

    private Vector2 currentLocation;
    private float currentAngle;

    [SerializeField] private GameObject[] Fireballs;

    private void Update()
    {
        for (var i = 0; i < Fireballs.Length; i++)
        {
            currentLocation = transform.position;
            currentAngle += FireballSpeed * Time.deltaTime;
            var offset = new Vector2(Mathf.Sin(currentAngle + 2 * i), Mathf.Cos(currentAngle + 2 * i)) * circleRadius;
            Fireballs[i].transform.position = currentLocation + offset;
        }
    }
}