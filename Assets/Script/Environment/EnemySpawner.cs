using GameJam;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    public GameObject enemyPrefab;
    public bool active;

    public Transform target;
    public float radiusFromTarget;
    public float delay;
    public float currentTime;

    public Transform hull;

    public ParticleSystemOverlapAction endgame;

    public float lastSpawnd;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (active)
        {
            currentTime = Time.time;
            if (lastSpawnd == 0 || Time.time > lastSpawnd)
            {
                var pos = target.position;
                var x = Random.Range(pos.x - radiusFromTarget, pos.x + radiusFromTarget);
                var y = Random.Range(pos.y - radiusFromTarget, pos.y + radiusFromTarget);

                var obj = Instantiate(enemyPrefab, new Vector3(x, y, -33), Quaternion.identity);
                obj.GetComponent<NetEnemyMovement>().target = target;

                obj.transform.parent = hull;

                lastSpawnd = Time.time + delay;
            }
        }
	}
}
