using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EnemySpawner : NetworkBehaviour {

	public GameObject enemyPrefab;
	public int numberOfEnemies;

	public override void OnStartServer () {
		for (int i = 0; i < numberOfEnemies; i++) {
			Vector3 spawnPos = new Vector3 (Random.Range (-4.0f, 4.0f), 1, Random.Range (-4.0f, 4.0f));
			Quaternion spawnRot = Quaternion.Euler (0, Random.Range (0.0f, 180.0f), 0);

			GameObject enemy = Instantiate (enemyPrefab, spawnPos, spawnRot) as GameObject;
			NetworkServer.Spawn (enemy);
		}
	}
}
