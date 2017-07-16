using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent (typeof (Rigidbody))]
public class PlayerController : NetworkBehaviour {

	Vector3 velocity;
	Rigidbody body;

	void Start() {
		body = GetComponent<Rigidbody> ();
	}

	void FixedUpdate () {
		if (!isLocalPlayer)
			return;

		body.MovePosition (body.position + velocity * Time.fixedDeltaTime);
	}

	public override void OnStartLocalPlayer() {
		GetComponent<MeshRenderer> ().material.color = Color.blue;
	}

	public void Move(Vector3 velocity) {
		this.velocity = velocity;
	}

	public void LookAt (Vector3 lookPoint) {
		Vector3 heightCorrectedPoint = new Vector3 (lookPoint.x, transform.position.y, lookPoint.z);
		transform.LookAt (heightCorrectedPoint);
	}
}
