using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TowerSpawner : MonoBehaviour
{
    private const float Y_MAX = 2.2f;
    private const float Y_MIN = -1.65f;
    private const float TOWER_SPEED = 5f;
    private const float OFF_SCREEN = -10f;
    private const float START_POSITION = 12.5f;
    private const float TOWER_SPACING = 5f;
    private const int POOL_SIZE = 4;
    private const float START_DELAY = 2f;

    [SerializeField] private GameObject towerPrefab;

    private Queue<GameObject> towerPool = new Queue<GameObject>();
    private List<GameObject> activeTowers = new List<GameObject>();

    private void Start()
    {
        CreatePool();
        SpawnInitialTowers();
    }

    private void Update()
    {
        if (!GameManager.Instance.GetTimerActive() || GameManager.Instance.GetTime() < START_DELAY)
        {
            return;
        }

        MoveTowers();
        CheckForTowerLeavingScreen();
    }

    private void CreatePool()
    {
        for (int i = 0; i < POOL_SIZE; i++)
        {
            GameObject tower = Instantiate(towerPrefab);
            tower.SetActive(false);

            towerPool.Enqueue(tower);
        }
    }

    private void SpawnInitialTowers()
    {
        for (int i = 0; i < POOL_SIZE; i++)
        {
            GameObject tower = GetTower();

            tower.transform.position = new Vector3(
                START_POSITION + (i * TOWER_SPACING),
                GetRandomHeight(),
                0
            );
        }
    }

    private GameObject GetTower()
    {
        GameObject tower = towerPool.Dequeue();

        tower.SetActive(true);
        activeTowers.Add(tower);

        return tower;
    }

    private void ReturnTower(GameObject tower)
    {
        tower.SetActive(false);
        activeTowers.Remove(tower);
        towerPool.Enqueue(tower);
    }

    private void MoveTowers()
    {
        foreach (GameObject tower in activeTowers)
        {
            tower.transform.Translate(
                Vector3.left * TOWER_SPEED * Time.deltaTime
            );
        }
    }

    private void CheckForTowerLeavingScreen()
    {
        GameObject tower = activeTowers[0];

        if (tower.transform.position.x < OFF_SCREEN)
        {
            ReturnTower(tower);

            GameObject newTower = GetTower();

            float rightMostX = GetRightMostX();

            newTower.transform.position = new Vector3(
                rightMostX + TOWER_SPACING,
                GetRandomHeight(),
                0
            );
        }
    }

    private float GetRightMostX()
    {
        float rightMostX = float.MinValue;

        foreach (GameObject tower in activeTowers)
        {
            if (tower.transform.position.x > rightMostX)
            {
                rightMostX = tower.transform.position.x;
            }
        }

        return rightMostX;
    }

    private float GetRandomHeight()
    {
        return Random.Range(Y_MIN, Y_MAX);
    }
}
