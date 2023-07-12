using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField] private float currentHealth, maxHealth;
    [SerializeField] private float moveSpeed;
    [SerializeField] private int damage;
    private Transform player;
    [SerializeField] private Vector2 target;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private float dropChanceOfHealItem;
    [SerializeField] private GameObject healPotion;
    [SerializeField] private GameObject throwableObject;
    [SerializeField] private float throwableObjectSpeed, cooldownToShoot;
    [SerializeField] private bool isFireSlime, isBrute, isSlime, isDarkTablet;
    private bool _canShoot = true;
    [SerializeField] private float movingCounter, movingCounterReset;
    [SerializeField] private GameObject xp;
    [SerializeField] private GameObject despawner;
    private bool canGo;
    private bool findNewVector = true;
    private static readonly int Dash = Animator.StringToHash("dash");

    private void Awake()
    {
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
        else if (isDarkTablet)
        {
            _spriteRenderer.flipX = player.position.x > gameObject.transform.position.x;

            StartCoroutine(DetectTargetAndGo());
        }
    }

    public void DecreaseHealthOfEnemy(int playerDamage)
    {
        if (currentHealth != 0)
        {
            currentHealth -= playerDamage;
            StartCoroutine(ChangeColor());
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

            var newDespawner = Instantiate(despawner, transform.position, quaternion.identity);
            Destroy(newDespawner, 0.5f);
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

    private IEnumerator ChangeColor()
    {
        var currentColor = _spriteRenderer.color;
        _spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        _spriteRenderer.color = currentColor;
    }

    private IEnumerator DetectTargetAndGo()
    {
        if (findNewVector)
        {
            target = player.position;
            yield return new WaitForSeconds(2f);
            findNewVector = false;
            canGo = true;
        }

        if (canGo)
        {
            transform.position = Vector2.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
            GetComponent<Animator>().SetBool(Dash, true);
        }

        if (transform.position.Equals(target))
        {
            canGo = false;
            GetComponent<Animator>().SetBool(Dash, false);
            yield return new WaitForSeconds(2f);
            findNewVector = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player.gameObject.GetComponent<PlayerBehaviour>().DecreaseHealthOfPlayer(damage);
        }
    }
}