using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Scoreboard : NetworkBehaviour {

	public static int score { get; private set; }
	float lastKillTime;
	int killStreak;
	float killStreakExpiry = 1f;

	void Start () {
		Enemy.OnDeathStatic += OnEnemyKilled;
		FindObjectOfType<Player> ().OnDeath += OnPlayerDeath;
	}

	void OnEnemyKilled() {
		if (Time.time < lastKillTime + killStreakExpiry) {
			killStreak++;
		} else {
			killStreak = 0;
		}

		lastKillTime = Time.time;

		score += 5 + 2 * killStreak;
	}

	void OnPlayerDeath() {
		Enemy.OnDeathStatic -= OnEnemyKilled;
	}
}
