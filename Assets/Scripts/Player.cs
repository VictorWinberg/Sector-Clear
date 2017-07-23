using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent (typeof (PlayerController))]
[RequireComponent (typeof (GunController))]
public class Player : LivingEntity {

	public float moveSpeed = 5;

	Camera cam;

	Crosshairs crosshairs;
	PlayerController controller;
	GunController gunController;

	public bool aimbot = false;

	protected override void Start () {
		base.Start ();
		controller = GetComponent<PlayerController> ();
		gunController = GetComponent<GunController> ();
		cam = Camera.main;
		crosshairs = FindObjectOfType<Crosshairs> ();
		crosshairs.activate ();
		FindObjectOfType<Spawner> ().OnNewWave += OnNewWave;
	}

	void OnNewWave(int waveNumber) {
		health = startingHealth;
		gunController.EquipGun (waveNumber - 1);
	}

	void LookAtTarget (Vector3 point) {
		controller.LookAt (point);
		crosshairs.transform.position = point;
		if ((new Vector2 (point.x, point.z) - new Vector2 (transform.position.x, transform.position.z)).sqrMagnitude > 1) {
			gunController.Aim ();
		}
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
		Plane groundPlane = new Plane (Vector3.up, Vector3.up * gunController.GunHeight);
		float rayDistance;
		Vector3 point = Vector3.zero;
		if (groundPlane.Raycast (ray, out rayDistance)) {
			point = ray.GetPoint (rayDistance);
			crosshairs.DetectTargets (ray);
			LookAtTarget (point);
		}

		// Weapon input
		if (Input.GetMouseButton(0)) {
			if (aimbot) {
				// Look input
				int enemyLayer = 1 << LayerMask.NameToLayer ("Enemy");
				float minDist = Mathf.Infinity;
				Vector3 closest = point;
				foreach (Collider hit in Physics.OverlapSphere (transform.position, 10f, enemyLayer)) {
					float dist = Vector3.Distance (hit.transform.position, transform.position);
					if (dist < minDist) {
						minDist = dist;
						closest = hit.transform.position;
					}
				}
				crosshairs.DetectTargets (!point.Equals (closest));
				LookAtTarget (closest);
			}
			gunController.OnTriggerHold();
		}
		if (Input.GetMouseButtonUp(0)) {
			gunController.OnTriggerRelease();
		}
		if (Input.GetKeyDown (KeyCode.R)) {
			gunController.Reload();
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
