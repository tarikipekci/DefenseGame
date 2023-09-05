using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{
    public static ObjectPooling instance;
    private List<GameObject> pooledObjects = new List<GameObject>(), pooledGreenSlimes = new List<GameObject>();
    private int amountToPool = 20, amountGreenSlimes = 50;
    [SerializeField] private GameObject weapon, greenSlime;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        for (var i = 0; i < amountToPool; i++)
        {
            var obj = Instantiate(weapon);
            obj.SetActive(false);
            pooledObjects.Add(obj);
        }
    
        for (var i = 0; i < amountGreenSlimes; i++)
        {
            var obj = Instantiate(greenSlime);
            obj.SetActive(false);
            pooledGreenSlimes.Add(obj);
        }
    }

    public GameObject GetPooledObject()
    {
        for (var i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }

        return null;
    }
    
    public GameObject GetPooledGreenSlimes()
    {
        for (var i = 0; i < pooledGreenSlimes.Count; i++)
        {
            if (!pooledGreenSlimes[i].activeInHierarchy)
            {
                return pooledGreenSlimes[i];
            }
        }

        return null;
    }
    
    public void UpdateWeaponSprite(Sprite sprite)
    {
        for (var i = 0; i < pooledObjects.Count; i++)
        {
            pooledObjects[i].GetComponent<SpriteRenderer>().sprite = sprite;
        }
    }
}