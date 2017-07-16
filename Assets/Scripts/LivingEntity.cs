using System.Collections;
using System.Collections.Generic;
﻿using UnityEngine;
using UnityEngine.Networking;

public class LivingEntity : NetworkBehaviour, IDamageable {

	[SerializeField]
	protected int startingHealth = 100;
	[SyncVar (hook= "OnChangeHealth")]
	protected int health;
	protected bool dead;

	[SerializeField]
	private RectTransform healthbar;
	[SerializeField]
	private bool destroyOnDeath;
	private NetworkStartPosition[] spawnPoints;

	public event System.Action OnDeath;

	protected virtual void Start() {
		health = startingHealth;
		if (isLocalPlayer) {
			spawnPoints = FindObjectsOfType<NetworkStartPosition> ();
		}
	}

	public void DealDamage(int damage, GameObject hit) {
		CmdDealDamage (damage, hit.GetComponent<NetworkIdentity> ());
	}

	[Command]
	void CmdDealDamage(int damage, NetworkIdentity target) {
		RpcDealDamage(damage, target);
	}

	[ClientRpc]
	void RpcDealDamage(int damage, NetworkIdentity target) {
		LivingEntity entity = target.gameObject.GetComponent<LivingEntity> ();
		entity.TakeDamage (damage);
	}

	public virtual void TakeDamage(int damage) {
		health -= damage;

		if (health <= 0 && !dead) {
			if (destroyOnDeath) {
				Die ();
			} else {
				health = startingHealth;

				if (isLocalPlayer) {
					CmdRespawn ();
				}
			}
		}
	}

	void OnChangeHealth(int health) {
		healthbar.sizeDelta = new Vector2 (health * 2, healthbar.sizeDelta.y);
	}

	[ContextMenu("Self Destruct")]
	protected void Die (){
		dead = true;
		if (OnDeath != null) {
			OnDeath();
		}
		Destroy (gameObject);
	}

	[Command]
	void CmdRespawn() {
		RpcRespawn ();
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
