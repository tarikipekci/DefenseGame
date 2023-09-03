using System.Collections;
using UnityEngine;

public class WeaponBehaviour : MonoBehaviour
{
    private bool canShoot = true;
    private GameObject target;
    [SerializeField] private float shootingDistance;
    [SerializeField] public float damage;
    public GameObject arrow;
    [SerializeField] private float speedArrow;
    [SerializeField] public float cooldownToShoot;
    [SerializeField] public float attackSpeed;
    public static WeaponBehaviour instance;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (canShoot)
        {
            canShoot = false;
            StartCoroutine(AllowToShoot());
            var allTargets = GameObject.FindGameObjectsWithTag("Enemy");
            if (allTargets != null)
            {
                target = allTargets[0];
                foreach (var tmpTarget in allTargets)
                {
                    if (Vector2.Distance(transform.position, tmpTarget.transform.position) <
                        Vector2.Distance(transform.position, target.transform.position))
                    {
                        target = tmpTarget;
                    }
                }

                if (Vector2.Distance(transform.position, target.transform.position) < shootingDistance)
                {
                    Fire();
                }
            }
        }
    }

    private void Fire()
    {
        Vector2 direction = target.transform.position - transform.position;
        //var tmpArrow = Instantiate(arrow, transform.position, transform.rotation);
        GameObject tmpArrow = ObjectPooling.instance.GetPooledObject();
        if (tmpArrow != null)
        {
            tmpArrow.transform.position = transform.position;
            tmpArrow.SetActive(true);
        }
        tmpArrow.transform.right = direction;
        tmpArrow.GetComponent<Rigidbody2D>().velocity = direction.normalized * speedArrow;
    }

    private IEnumerator AllowToShoot()
    {
        yield return new WaitForSeconds(cooldownToShoot - attackSpeed);
        canShoot = true;
    }
}