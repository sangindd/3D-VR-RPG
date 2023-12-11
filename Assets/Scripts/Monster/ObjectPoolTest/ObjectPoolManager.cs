using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : SingletonClass<ObjectPoolManager>
{
    #region Declaration & Definition
    // Set Monster Prefab
    [SerializeField] public GameObject monsterPrefab;

    // Get Spawn Point 
    protected List<Transform> spawnPoint = new();

    // Get Object Pool
    protected List<GameObject> monsterPool = new();

    // Set Monster Component
    public int maxMonsters = 1;
    public float respawnTime = 5.0f;
    public string spawnGroup;

    private bool stopRespawn;
    public bool StopRespawn
    {
        get { return stopRespawn; }
        set
        {
            stopRespawn = value;
            if (stopRespawn)
            {
                CancelInvoke();
            }
        }
    }
    #endregion

    #region Unity Default Method
    private new void Awake()
    {
        CreateMonsterPool();

        Transform spawnPointGroup = GameObject.Find(spawnGroup)?.transform;

        foreach (Transform point in spawnPointGroup)
            spawnPoint.Add(point);

        InvokeRepeating("CreateMonster", 2.0f, respawnTime);
    }
    #endregion

    #region Method
    protected void CreateMonster()
    {
        int idx = Random.Range(0, spawnPoint.Count);

        GameObject monster = GetMonsterInPool();
        monster?.transform.SetPositionAndRotation(spawnPoint[idx].position, spawnPoint[idx].rotation);

        monster?.SetActive(true);
    }

    protected void CreateMonsterPool()
    {
        for (int i = 0; i < maxMonsters; i++)
        {
            var monster = Instantiate<GameObject>(monsterPrefab);
            monster.name = $"Monster_{i:00}";
            monster.SetActive(false);

            monsterPool.Add(monster);
        }
    }

    protected GameObject GetMonsterInPool()
    {
        foreach (var monster in monsterPool)
        {
            if (monster.activeSelf == false)
                return monster;
        }

        return null;
    }
    #endregion
}