using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GunController : NetworkBehaviour {

	public Transform weaponHold;
	public Gun startingGun;
	Gun equippedGun;

	void Start() {
		if (startingGun != null) {
			EquipGun (startingGun);
		}
	}

	void EquipGun(Gun gunToEquip) {
		if (equippedGun != null) {
			Destroy (equippedGun.gameObject);
		}

		equippedGun = Instantiate (gunToEquip, weaponHold.position, weaponHold.rotation) as Gun;
		equippedGun.transform.parent = weaponHold;
	}

	[Command]
	public void CmdShoot() {
		if (equippedGun != null) {
			RpcShoot ();
		}
	}

	[ClientRpc]
	public void RpcShoot() {
		equippedGun.Shoot ();
	}
}
