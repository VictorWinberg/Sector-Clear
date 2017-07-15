using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

	private Player player;
	public Player Player {
		get { return this.player; }
		set { this.player = value; }
	}

	[SerializeField]
	private LayerMask collisionMask;
	[SerializeField]
	private float speed = 10;
	[SerializeField]
	private int damage = 10;

	public void SetSpeed(float speed) {
		this.speed = speed;
	}

	void Update () {
		// Movement
		float moveDistance = speed * Time.deltaTime;
		transform.Translate (Vector3.forward * moveDistance);

		// Collision Detection
		CheckCollision (moveDistance);
	}

	void CheckCollision (float moveDistance){
		Ray ray = new Ray (transform.position, transform.forward);
		RaycastHit hit;

		if(Physics.Raycast(ray, out hit, moveDistance, collisionMask, QueryTriggerInteraction.Collide)) {
			OnHitObject(hit);
		}
	}

	void OnHitObject (RaycastHit hit){
		IDamageable damageableObject = hit.collider.GetComponent<IDamageable> ();

		if (damageableObject != null && player.isLocalPlayer) {
			player.DealDamage(damage, hit.collider.gameObject);
		}
		GameObject.Destroy (gameObject);
	}
}
