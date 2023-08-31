using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOn : MonoBehaviour
{
    public GameObject closestEnemy;
    // maincamera
    private Camera mainCamera;

    // Start is called before the first frame update
   void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        closestEnemy = GetClosestEnemy();
    }

    /// 获得离玩家最近，Tag为Enemy, 并且在相机视野内的物体；
    /// 如果没有找到，返回null
    GameObject GetClosestEnemy()
    {
        GameObject[] gos;
        gos = GetVisibleEnemies().ToArray();
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;

            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }

        }
        return closest;
    }


    // Returns a list of all enemies within a certan distance of the player
    public List<GameObject> GetVisibleEnemies()
    {
        List<GameObject> enemies = new List<GameObject>();
        GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in allEnemies)
        {
            if (IsWithinDistance(enemy, 10f))
            {
                enemies.Add(enemy);
            }
        }
        return enemies;

    }

    bool IsWithinDistance(GameObject enemy, float distance)
    {
        Vector3 screenPoint = mainCamera.WorldToViewportPoint(enemy.transform.position);
        bool onScreen = screenPoint.z > 0 && screenPoint.x > 0 &&
            screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
        return onScreen && Vector3.Distance(transform.position, enemy.transform.position) < distance;
    }
}


