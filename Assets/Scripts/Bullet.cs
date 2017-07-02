using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	// Use this for collision enter
	void OnCollisionEnter (Collision collision) {
		GameObject hit = collision.gameObject;
		Health health = hit.GetComponent<Health> ();

		if (health != null) {
			health.TakeDamage (10);
		}
		Destroy (gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
