using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent (typeof (Rigidbody))]
public class PlayerController : NetworkBehaviour {

	Vector3 velocity;
	Rigidbody body;

	void Start() {
		body = GetComponent<Rigidbody> ();
	}

	public void Move(Vector3 velocity) {
		this.velocity = velocity;
	}

	public void LookAt(Vector3 lookPoint) {
		Vector3 highCorrectedPoint = new Vector3 (lookPoint.x, transform.position.y, lookPoint.z);
		transform.LookAt (highCorrectedPoint);
	}

	public void FixedUpdate() {
		if (!isLocalPlayer)
			return;

		body.MovePosition (body.position + velocity * Time.fixedDeltaTime);
	}

	/*
	public GameObject bulletPrefab;
	public Transform bulletSpawn;

	void Update() {

		if (!isLocalPlayer)
			return;
		
		var x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
		var z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;

		transform.Rotate(0, x, 0);
		transform.Translate(0, 0, z);

		if (Input.GetKeyDown (KeyCode.Space)) {
			CmdFire ();
		}
	}

	[Command]
	void CmdFire() {
		// Spawn bullet
		GameObject bullet = (GameObject)Instantiate (bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);

		// Add velocity
		bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 6.0f;

		// Spawn bullets on clients
		NetworkServer.Spawn(bullet);

		// Destroy
		Destroy(bullet, 2);
	}
	*/

	public override void OnStartLocalPlayer() {
		GetComponent<MeshRenderer> ().material.color = Color.blue;
	}
}
