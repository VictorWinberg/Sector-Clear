using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour {

	public Image fadeCanvas;
	public GameObject gameOverUI;

	public RectTransform waveBanner, healthbar;
	public Text waveTitle, waveEnemyCount, scoreUI, gameOverScore, healthbarHp;

	public Spawner spawner;
	public Player player;

	private bool lobby = true;

	void Update() {
		scoreUI.text = Scoreboard.score.ToString("D6");
		float healthPercent = 0;
		if (player != null) {
			healthPercent = player.health / (float)(player.startingHealth);
			healthbarHp.text = player.health + "/" + player.startingHealth;
		}
		healthbar.localScale = new Vector3 (healthPercent, 1, 1);
	}

	public void OnNewWave(int waveNumber) {
		lobby = waveNumber == 0;
		waveTitle.text = "- Wave " + HumanFriendlyInteger.IntegerToWritten (waveNumber) + " -";
		string enemyCount = (spawner.waves [waveNumber].infinite) ? "Infinite" : spawner.waves [waveNumber].enemyCount + "";
		waveEnemyCount.text = "Enemies: " + enemyCount;
		StopCoroutine ("AnimateWaveBanner");
		StartCoroutine ("AnimateWaveBanner");
	}

	IEnumerator AnimateWaveBanner () {
		float delayTime = 1.5f;
		float speed = 3f;
		float animatePercent = 0f;
		int dir = 1;

		float endDelayTime = Time.time + 1 / speed + delayTime;

		while (animatePercent >= 0) {
			animatePercent += Time.deltaTime * speed * dir;

			if(animatePercent >= 1) {
				animatePercent = 1;
				if(Time.time > endDelayTime) {
					dir = -1;
				}
			}

			waveBanner.anchoredPosition = Vector2.up * Mathf.Lerp(-150, 150, animatePercent);
			yield return null;
		}
	}

	public void OnGameOver () {
		if (lobby)
			return;

		Cursor.visible = true;
		StartCoroutine(Fade(Color.clear, new Color(1, 1, 1, .8f), 1));
		gameOverScore.text = scoreUI.text;
		scoreUI.gameObject.SetActive (false);
		healthbar.transform.parent.gameObject.SetActive (false);
		gameOverUI.SetActive (true);
	}

	IEnumerator Fade(Color from, Color to, float time) {
		float speed = 1 / time;
		float percent = 0;

		while (percent < 1) {
			percent += Time.deltaTime * speed;
			fadeCanvas.color = Color.Lerp(from, to, percent);
			yield return null;
		}
	}

	public void Disconnect() {
		if (player.isServer)
			NetworkManager.singleton.StopHost();
		if (player.isClient)
			NetworkManager.singleton.StopClient();
	}
}
