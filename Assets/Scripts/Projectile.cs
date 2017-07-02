using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

	float speed = 10;

	public void SetSpeed(float newSpeed) {
		speed = newSpeed;
	}

	void OnTriggerEnter (Collider collider) {
		GameObject hit = collider.gameObject;
		Health health = hit.GetComponent<Health> ();

		if (health != null) {
			health.TakeDamage (10);
		}
		Destroy (gameObject);
	}

	void Update() {
		transform.Translate (Vector3.forward * Time.deltaTime * speed);
	}
}
