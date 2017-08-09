using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class Spawner : NetworkBehaviour {

	public bool developerMode;
	public bool random;

	public Wave[] waves;
	public Enemy enemy;

	Wave currentWave;
	int currentWaveNumber;

	int enemiesRemainingToSpawn, enemiesRemainingAlive;
	float nextSpawnTime;

	MapGenerator map;

	public event System.Action<int> OnNewWave;

	public override void OnStartClient () {
		map = FindObjectOfType<MapGenerator> ();
	}

	void Start() {
		if (!isServer)
			return;

		NextWave ();
		nextSpawnTime = Time.time + currentWave.timeBetweenSpawns;
	}

	void Update () {
		if (!isServer || currentWave == null)
			return;

		if ((enemiesRemainingToSpawn > 0 || currentWave.infinite) && Time.time > nextSpawnTime) {
			enemiesRemainingToSpawn--;
			nextSpawnTime = Time.time + currentWave.timeBetweenSpawns;

			StartCoroutine("SpawnEnemy");
		}

		if (developerMode || currentWaveNumber == 1) {
			if(Input.GetKeyDown(KeyCode.Return)) {
				StopCoroutine("SpawnEnemy");
				foreach(Enemy enemy in FindObjectsOfType<Enemy>()) {
					GameObject.Destroy(enemy.gameObject);
				}
				NextWave();
			}
		}
	}

	IEnumerator SpawnEnemy() {
		float spawnDelay = 1;
		float tileFlashSpeed = 4;

		Transform spawnTile = map.getRandomOpenTile ();
		Material tileMaterial = spawnTile.GetComponent<Renderer> ().material;
		Color initialColor = map.getInitialTileColor ();
		Color flashColor = Color.red;
		float spawnTimer = 0;

		while (spawnTimer < spawnDelay) {

			tileMaterial.color = Color.Lerp(initialColor, flashColor, Mathf.PingPong(spawnTimer * tileFlashSpeed, 1));

			spawnTimer += Time.deltaTime;
			yield return null;
		}

		if(spawnTile == null)
			spawnTile = map.getRandomOpenTile ();

		tileMaterial.color = initialColor;

		if (isServer) {
			Enemy spawnedEnemy = Instantiate (enemy, spawnTile.position + Vector3.up, Quaternion.identity) as Enemy;
			spawnedEnemy.OnDeath += OnEnemyDeath;

			spawnedEnemy.SetCharacteristics (currentWave.moveSpeed, currentWave.damage, currentWave.health, currentWave.skinColor);
			NetworkServer.Spawn (spawnedEnemy.gameObject);
		}
		yield return null;
	}

	void OnEnemyDeath (){
		enemiesRemainingAlive--;

		if (enemiesRemainingAlive == 0) {
			NextWave();
		}
	}

	void ResetPlayerPosition() {
		Player[] players = FindObjectsOfType<Player> ();

		foreach(Player player in players)
			player.transform.position = map.getRandomOpenTile().position + Vector3.up * 3;		
	}

	void NextWave() {
		if (currentWaveNumber - 1 < waves.Length) {
			currentWave = waves [currentWaveNumber];

			enemiesRemainingToSpawn = currentWave.enemyCount;
			enemiesRemainingAlive = enemiesRemainingToSpawn;

			RpcOnNewWave (currentWaveNumber);
			currentWaveNumber++;
		}
	}

	[ClientRpc]
	void RpcOnNewWave(int wave) {
		if(OnNewWave != null)
			OnNewWave(wave);

		ResetPlayerPosition();
	}

	[System.Serializable]
	public class Wave {
		public bool infinite;
		public int enemyCount;
		public float timeBetweenSpawns;

		public float moveSpeed;
		public int damage;
		public int health;
		public Color skinColor;
	}
}
