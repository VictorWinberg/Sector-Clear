using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent (typeof (PlayerController))]
[RequireComponent (typeof (GunController))]
public class Player : LivingEntity {

	public float moveSpeed = 5;

	Camera cam;
	PlayerController controller;
	GunController gunController;

	protected override void Start () {
		base.Start ();
		controller = GetComponent<PlayerController> ();
		gunController = GetComponent<GunController> ();
		cam = Camera.main;
	}
	
	void Update () {
		if (!isLocalPlayer) 
			return;

		// Movement input
		var x = Input.GetAxisRaw ("Horizontal");
		var z = Input.GetAxisRaw ("Vertical");

		Vector3 moveInput = new Vector3 (x, 0, z);
		Vector3 moveVelocity = moveInput.normalized * moveSpeed;
		controller.Move (moveVelocity);

		// Look input
		Ray ray = cam.ScreenPointToRay (Input.mousePosition);
		Plane groundPlane = new Plane (Vector3.up, Vector3.zero);
		float rayDistande;

		if (groundPlane.Raycast (ray, out rayDistande)) {
			Vector3 point = ray.GetPoint (rayDistande);
			controller.LookAt (point);
		}

		// Weapon input
		if (Input.GetMouseButton (0) || Input.GetKeyDown(KeyCode.Space)) {
			gunController.Shoot ();
		}
	}

	protected override void Die() {
		base.Die ();
		health = startingHealth;

		if (isLocalPlayer) {
			CmdRespawn ();
		}
	}
}
