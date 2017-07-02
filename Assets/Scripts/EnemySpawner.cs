using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EnemySpawner : NetworkBehaviour {

	public GameObject enemyPrefab;
	public int numberOfEnemies;

	public override void OnStartServer () {
		for (int i = 0; i < numberOfEnemies; i++) {
			Vector3 spawnPos = new Vector3 (Random.Range (-8.0f, 8.0f), 0, Random.Range (-8.0f, 8.0f));
			Quaternion spawnRot = Quaternion.Euler (0, Random.Range (0.0f, 180.0f), 0);

			GameObject enemy = (GameObject)Instantiate (enemyPrefab, spawnPos, spawnRot);
			NetworkServer.Spawn (enemy);
		}
	}
}
