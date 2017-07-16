using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	private Player player;
	public Player Player {
		get { return this.player; }
		set { this.player = value; }
	}

	[SerializeField]
	private LayerMask collisionMask;
	public Color trailColor;
	[SerializeField]
	private float speed = 10;
	[SerializeField]
	private int damage = 10;

	float lifetime = 3, skinWidth = .1f;

	void Start() {
		Destroy (gameObject, lifetime);

		Collider[] initialCollision = Physics.OverlapSphere (transform.position, .1f, collisionMask);
		if (initialCollision.Length > 0) {
			OnHitObject(initialCollision[0], transform.position);
		}

		GetComponent<TrailRenderer> ().material.SetColor ("_TintColor", trailColor);
	}

	public void SetSpeed(float speed) {
		this.speed = speed;
	}

	void Update () {
		// Movement
		float moveDistance = speed * Time.deltaTime;
		transform.Translate (Vector3.forward * moveDistance);

		// Collision Detection
		CheckCollisions (moveDistance);
	}

	void CheckCollisions (float moveDistance) {
		Ray ray = new Ray (transform.position, transform.forward);
		RaycastHit hit;

		if(Physics.Raycast(ray, out hit, moveDistance + skinWidth, collisionMask, QueryTriggerInteraction.Collide)) {
			OnHitObject(hit.collider, hit.point);
		}
	}

	void OnHitObject (Collider c, Vector3 hitPoint){
		IDamageable damageableObject = c.GetComponent<IDamageable> ();

		if (damageableObject != null && player.isLocalPlayer) {
			player.DealDamage(damage, c.gameObject);
			// damageableObject.TakeHit(damage, hitPoint, transform.forward);
		}
		GameObject.Destroy (gameObject);
	}
}
