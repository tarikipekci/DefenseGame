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
    [SerializeField] private FixedJoystick _joystick;

    private static readonly int Archer = Animator.StringToHash("archer");
    private static readonly int Mage = Animator.StringToHash("mage");
    private static readonly int Knight = Animator.StringToHash("knight");
    [SerializeField] private Image amblem,bar;
    [SerializeField] private Sprite arrow, magicBall, archerBar, mageBar, knightBar, archerAmblem, mageAmblem, knightAmblem;


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
        }
        else if (ClassBehaviour.isMage)
        {
            _animator.SetTrigger(Mage);
            WeaponBehaviour.instance.arrow.GetComponent<SpriteRenderer>().sprite = magicBall;
            amblem.sprite = mageAmblem;
            bar.sprite = mageBar;
        }
        else if (ClassBehaviour.isKnight)
        {
            _animator.SetTrigger(Knight);
            amblem.sprite = knightAmblem;
            bar.sprite = knightBar;
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
            currentHealth -= damage;
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
        xpBar.value = xpAmount;
    }

    private void CalculateLevel()
    {
        if (xpAmount == 10f)
        {
            level++;
            levelBackup = level;
            levelCounterText.text = level.ToString();
            xpAmount = 0f;
        }
    }
}