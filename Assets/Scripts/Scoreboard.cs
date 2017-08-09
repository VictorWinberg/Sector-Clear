using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Scoreboard : MonoBehaviour {

	public static int score { get; private set; }
	float lastKillTime;
	int killStreak;
	float killStreakExpiry = 3f;

	void Start () {
		Enemy.OnDeathStatic += OnEnemyKilled;
	}

	void OnEnemyKilled() {
		score += 1;
	}

	public void OnPlayerDeath() {
		Enemy.OnDeathStatic -= OnEnemyKilled;
	}
}
