using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField] private float currentHealth, maxHealth;
    [SerializeField] private float moveSpeed;
    [SerializeField] private int damage;
    [SerializeField] private Transform player;
    [SerializeField] private Vector2 target;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private float dropChanceOfHealItem;
    [SerializeField] private GameObject healPotion;
    [SerializeField] private GameObject throwableObject;
    [SerializeField] private float throwableObjectSpeed, cooldownToShoot;
    [SerializeField] private bool isFireSlime, isBrute, isSlime;
    private bool _canShoot = true;
    [SerializeField] private float movingCounter, movingCounterReset;
    public static EnemyBehaviour instance;
    [SerializeField] private GameObject xp;

    private void Awake()
    {
        instance = this;
        player = PlayerBehaviour.instance.transform;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;
        movingCounterReset = movingCounter;
    }

    private void Update()
    {
        if (isBrute || isSlime)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
            _spriteRenderer.flipX = player.position.x < gameObject.transform.position.x;
        }
        else if (isFireSlime)
        {
            if (movingCounter > 0)
            {
                movingCounter -= Time.deltaTime;
                transform.position = Vector2.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
            }
            else
            {
                movingCounter = movingCounterReset;
                var newVector = new Vector2(Random.Range(-17, 17), Random.Range(9, -9));
                target = newVector;
            }

            _spriteRenderer.flipX = player.position.x < gameObject.transform.position.x;
            if (_canShoot)
            {
                _canShoot = false;
                StartCoroutine(AllowToFire());
            }
        }
    }

    public void DecreaseHealthOfEnemy(int playerDamage)
    {
        if (currentHealth != 0)
        {
            currentHealth -= playerDamage;
        }

        if (currentHealth <= 0)
        {
            DropHealItem();
            Instantiate(xp, transform.position, quaternion.identity);
            var contains = LevelManager.instance.enemies.Contains(this);
            if (contains)
            {
                LevelManager.instance.enemies.Remove(this);
            }
            Destroy(gameObject);
        }
    }

    private void DropHealItem()
    {
        var randomValue = Random.Range(1, 100);
        if (dropChanceOfHealItem > randomValue)
        {
            Instantiate(healPotion, transform.position, quaternion.identity);
        }
    }

    private void Fire()
    {
        Vector2 direction = player.position - transform.position;
        var tmpThrowableObject = Instantiate(throwableObject, transform.position, transform.rotation);
        tmpThrowableObject.transform.right = direction;
        tmpThrowableObject.GetComponent<Rigidbody2D>().velocity = direction.normalized * throwableObjectSpeed;
    }

    private IEnumerator AllowToFire()
    {
        yield return new WaitForSeconds(cooldownToShoot);
        _canShoot = true;
        Fire();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player.gameObject.GetComponent<PlayerBehaviour>().DecreaseHealthOfPlayer(damage);
        }
    }
}