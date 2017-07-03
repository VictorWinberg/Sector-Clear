using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LivingEntity : NetworkBehaviour, IDamageable {

	public const int startingHealth = 100;
	[SyncVar (hook= "OnChangeHealth")]
	public int health;
	protected bool dead;

	public RectTransform healthbar;
	public bool destroyOnDeath;
	private NetworkStartPosition[] spawnPoints;

	public event System.Action OnDeath;

	protected virtual void Start() {
		health = startingHealth;
		if (isLocalPlayer) {
			spawnPoints = FindObjectsOfType<NetworkStartPosition> ();
		}
	}

	public void TakeHit (int damage, RaycastHit hit) {
		if (!isServer)
			return;

		TakeDamage(damage);
	}

	public void TakeDamage (int damage) {
		if (!isServer)
			return;

		health -= damage;

		if (health <= 0 && !dead) {
			if (destroyOnDeath) {
				Die ();
			} else {
				health = startingHealth;
				RpcRespawn ();
			}
		}
	}

	void OnChangeHealth(int health) {
		healthbar.sizeDelta = new Vector2 (health * 2, healthbar.sizeDelta.y);
	}

	protected void Die (){
		dead = true;
		if (OnDeath != null) {
			OnDeath();
		}
		Destroy (gameObject);
	}

	[ClientRpc]
	void RpcRespawn() {
		if (isLocalPlayer) {
			Vector3 spawnPoint = Vector3.zero;

			if (spawnPoints != null && spawnPoints.Length > 0) {
				spawnPoint = spawnPoints [Random.Range (0, spawnPoints.Length)].transform.position;
			}

			transform.position = spawnPoint;
		}
	}
}
