using Joystick_Pack.Scripts.Joysticks;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField] public float moveSpeed;
    private Rigidbody2D _rigidbody;
    [SerializeField] public int currentHealth, maxHealth;
    public static PlayerBehaviour instance;
    [SerializeField] public Slider healthBar, xpBar;
    public Text healthLevelText, levelCounterText;
    private SpriteRenderer _spriteRenderer;
    public Animator _animator;
    private static readonly int Walking = Animator.StringToHash("walking");
    [SerializeField] public float xpAmount;
    [SerializeField] public float level, levelBackup;
    [SerializeField] private FloatingJoystick _joystick;
    private bool shieldEnabled;
    [SerializeField] private GameObject shield;

    private static readonly int Archer = Animator.StringToHash("archer");
    private static readonly int Mage = Animator.StringToHash("mage");
    private static readonly int Knight = Animator.StringToHash("knight");
    [SerializeField] private Image amblem, bar;

    [SerializeField]
    private Sprite arrow, magicBall, archerBar, mageBar, knightBar, archerAmblem, mageAmblem, knightAmblem;


    private void Awake()
    {
        instance = this;
        currentHealth = maxHealth;
    }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();

        if (ClassBehaviour.isArcher)
        {
            _animator.SetTrigger(Archer);
            WeaponBehaviour.instance.arrow.GetComponent<SpriteRenderer>().sprite = arrow;
            amblem.sprite = archerAmblem;
            bar.sprite = archerBar;
            LevelManager.instance.SetBaseStats(7f,3f, 5f,9f);
            LevelManager.instance.UpdateStats();
        }
        else if (ClassBehaviour.isMage)
        {
            _animator.SetTrigger(Mage);
            WeaponBehaviour.instance.arrow.GetComponent<SpriteRenderer>().sprite = magicBall;
            amblem.sprite = mageAmblem;
            bar.sprite = mageBar;
            LevelManager.instance.SetBaseStats(5f,5f, 3f,12f);
            LevelManager.instance.UpdateStats();
        }
        else if (ClassBehaviour.isKnight)
        {
            _animator.SetTrigger(Knight);
            amblem.sprite = knightAmblem;
            bar.sprite = knightBar;
            LevelManager.instance.SetBaseStats(15f,7f, 3f,7f);
            LevelManager.instance.UpdateStats();
        }
    }

    private void Update()
    {
        Walk();
        UpdateHealthUI();
        UpdateXpUI();
    }

    private void Walk()
    {
        _rigidbody.velocity = new Vector2(_joystick.Horizontal * moveSpeed, _joystick.Vertical * moveSpeed);

        switch (_rigidbody.velocity.x)
        {
            case > 0:
                _spriteRenderer.flipX = false;
                _animator.SetBool(Walking, true);
                break;
            case < 0:
                _spriteRenderer.flipX = true;
                _animator.SetBool(Walking, true);
                break;
            default:
            {
                if (_rigidbody.velocity.y != 0)
                {
                    _animator.SetBool(Walking, true);
                }

                else
                {
                    _animator.SetBool(Walking, false);
                }

                break;
            }
        }
    }

    public void DecreaseHealthOfPlayer(int damage)
    {
        if (currentHealth != 0)
        {
            if (shieldEnabled)
            {
                shield.SetActive(false);
                shieldEnabled = false;
            }
            else
            {
                currentHealth -= damage;
            }
        }

        if (currentHealth <= 0)
        {
            UpdateHealthUI();
            gameObject.SetActive(false);
            LevelManager.instance.deathScreen.gameObject.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void IncreaseHealthOfPlayer(int healAmount)
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += healAmount;
        }
    }

    private void UpdateHealthUI()
    {
        healthLevelText.text = currentHealth + "/" + maxHealth;
        healthBar.value = currentHealth;
    }

    private void UpdateXpUI()
    {
        CalculateLevel();
        levelCounterText.text = level.ToString();
        xpBar.value = xpAmount;
    }

    public GameObject GetShieldObject()
    {
        return shield;
    }

    public void SetShieldEnabled(bool currentValue)
    {
        shieldEnabled = currentValue;
    }

    public bool GetShieldEnabled()
    {
        return shieldEnabled;
    }

    private void CalculateLevel()
    {
        if (xpAmount >= 10f)
        {
            level++;
            levelBackup++;
            xpAmount = 0f;
        }
    }
}