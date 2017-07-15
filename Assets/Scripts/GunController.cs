using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GunController : NetworkBehaviour {

	public Transform weaponHold;
	public Gun startingGun;
	private Gun gun;

	void Start() {
		if (startingGun != null) {
			EquipGun (startingGun);
		}
	}

	void EquipGun(Gun newGun) {
		if (gun != null) {
			Destroy (gun);
		}

		gun = Instantiate (newGun, weaponHold.position, weaponHold.rotation) as Gun;
		gun.transform.parent = weaponHold;
		gun.player = GetComponent<Player>();
	}

	public void Shoot() {
		if (gun != null) {
			gun.Shoot ();
			CmdShoot ();
		}
	}

	[Command]
	void CmdShoot() {
		RpcShoot ();
	}

	[ClientRpc]
	public void RpcShoot() {
		gun.Shoot ();
	}
}
