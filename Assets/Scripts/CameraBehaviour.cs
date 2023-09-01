using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
   public Transform target;
   public Vector3 offset;
   public float damping;

   private Vector3 _velocity = Vector3.zero;

   private void FixedUpdate()
   {
      var movePosition = target.position + offset;
      transform.position = Vector3.SmoothDamp(transform.position, movePosition,  ref _velocity, damping);
      
      
   }
}