using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField] private float currentHealth, maxHealth;
    [SerializeField] private float moveSpeed, moveSpeedReset, debuffSpeed;
    [SerializeField] private int damage;
    private Transform player;
    [SerializeField] private Vector2 target;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private float dropChanceOfHealItem, dropChanceOfAnvilItem, dropChanceOfShieldItem;
    [SerializeField] private GameObject healPotion, anvilDrop, shieldItem;
    [SerializeField] private GameObject throwableObject;
    [SerializeField] private float throwableObjectSpeed, cooldownToShoot;
    [SerializeField] private bool isFireSlime, isBrute, isSlime, isDarkTablet, isImp;
    private bool _canShoot = true;
    [SerializeField] private float movingCounter, movingCounterReset;
    [SerializeField] private GameObject xp;
    [SerializeField] private GameObject despawner;
    [SerializeField] private float debuffLevel;
    private bool itemDropped;
    private bool findNewTarget = true;
    [SerializeField] private float coolDownForSlowDebuff, coolDownForSlowDebuffReset;
    [SerializeField] private float enemyRestDuration, enemyRestDurationCounter;

    [SerializeField] private float debuffDuration, debuffDurationReset;
    private bool hit;
    private static readonly int Dash = Animator.StringToHash("dash");

    private void Awake()
    {
        player = PlayerBehaviour.instance.transform;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;
        movingCounterReset = movingCounter;
        debuffSpeed = moveSpeed - debuffLevel;
        moveSpeedReset = moveSpeed;
        debuffDurationReset = debuffDuration;
        coolDownForSlowDebuffReset = coolDownForSlowDebuff;
        enemyRestDurationCounter = enemyRestDuration;
    }

    private void FixedUpdate()
    {
        Rest();

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
                var newVector = new Vector2(Random.Range(-30, 30), Random.Range(15, -15));
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

            DetectTargetAndGo();
        }

        else if (isImp)
        {
            _spriteRenderer.flipX = player.position.x > gameObject.transform.position.x;
            transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
        }
    }

    public void DecreaseHealthOfEnemy(float playerDamage)
    {
        if (currentHealth != 0)
        {
            currentHealth -= playerDamage;
            StartCoroutine(ChangeColor());
        }

        if (currentHealth <= 0)
        {
            DropHealItem();
            DropAnvilDropItem();
            DropShieldItem();
            Instantiate(xp, transform.position, quaternion.identity);
            var contains = LevelManager.instance.enemies.Contains(this);
            if (contains)
            {
                LevelManager.instance.enemies.Remove(this);
                LevelManager.instance.ReturnHowManyEnemiesLeft();
            }

            var newDespawner = Instantiate(despawner, transform.position, quaternion.identity);
            Destroy(newDespawner, 0.5f);
            if (isSlime)
            {
                currentHealth = maxHealth;
                _spriteRenderer.color = Color.white;
                gameObject.SetActive(false);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    private void DropHealItem()
    {
        var randomValue = Random.Range(1, 100);
        if (dropChanceOfHealItem > randomValue && itemDropped == false)
        {
            Instantiate(healPotion, transform.position, quaternion.identity);
            itemDropped = true;
        }
    }

    private void Rest()
    {
        if (hit)
        {
            if (enemyRestDuration > 0)
            {
                moveSpeed = 0;
                enemyRestDuration -= Time.deltaTime;
            }
            else
            {
                moveSpeed = moveSpeedReset;
                enemyRestDuration = enemyRestDurationCounter;
                hit = false;
            }
        }
    }

    private void DropAnvilDropItem()
    {
        var randomValue = Random.Range(1, 100);
        if (dropChanceOfAnvilItem > randomValue && itemDropped == false)
        {
            Instantiate(anvilDrop, transform.position, quaternion.identity);
            itemDropped = true;
        }
    }

    private void DropShieldItem()
    {
        var randomValue = Random.Range(1, 100);
        if (dropChanceOfShieldItem > randomValue && itemDropped == false)
        {
            Instantiate(shieldItem, transform.position, quaternion.identity);
            itemDropped = true;
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

    private void DetectTargetAndGo()
    {
        if (findNewTarget)
        {
            target = player.position;
            findNewTarget = false;
        }

        if (debuffDuration > 0)
        {
            debuffDuration -= Time.deltaTime;
            moveSpeed = moveSpeedReset;
            GetComponent<Animator>().SetBool(Dash, true);
            transform.position = Vector2.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
        }

        if (debuffDuration <= 0)
        {
            if (coolDownForSlowDebuff > 0)
            {
                findNewTarget = true;
                GetComponent<Animator>().SetBool(Dash, false);
                moveSpeed = debuffSpeed;
                transform.position = Vector2.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
                coolDownForSlowDebuff -= Time.deltaTime;
            }

            if (coolDownForSlowDebuff <= 0)
            {
                coolDownForSlowDebuff = coolDownForSlowDebuffReset;
                debuffDuration = debuffDurationReset;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            hit = true;
            player.gameObject.GetComponent<PlayerBehaviour>().DecreaseHealthOfPlayer(damage);
        }
    }
}