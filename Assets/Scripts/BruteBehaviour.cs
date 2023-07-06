using UnityEngine;

public class BruteBehaviour : MonoBehaviour
{
    [SerializeField] private Transform player;
    private Animator _animator;
    private static readonly int Attack = Animator.StringToHash("attack");
    private static readonly int Walking = Animator.StringToHash("walking");
    private static readonly int AttackRight = Animator.StringToHash("attackRight");

    private void Start()
    {
        _animator = GetComponent<Animator>();
        player = PlayerBehaviour.instance.transform;
    }

    private void Update()
    {
        if (Vector2.Distance(player.position, transform.position) <= 5f)
        {
            if (player.position.x < transform.position.x)
            {
                _animator.SetTrigger(Attack);
                _animator.SetBool(Walking, false);
            }
            else
            {
                _animator.SetTrigger(AttackRight);
                _animator.SetBool(Walking, false);
            }
        }
        else
        {
            _animator.SetBool(Walking, true);
        }
    }
}