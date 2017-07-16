using System.Collections;
﻿using UnityEngine;
using UnityEngine.Networking;

public class GunController : NetworkBehaviour {

	public Transform weaponHold;
	public Gun[] guns;
	private Gun gun;

	void Start() {
		EquipGun (0);
	}

	void EquipGun(Gun newGun) {
		if (gun != null) {
			Destroy (gun.gameObject);
		}

		gun = Instantiate (newGun, weaponHold.position, weaponHold.rotation) as Gun;
		gun.transform.parent = weaponHold;
		gun.Player = GetComponent<Player>();
	}

	public void EquipGun(int gunIndex) {
		EquipGun (guns[gunIndex]);
	}

	public void OnTriggerHold () { 
		CmdOnTriggerHold (); 
	}

	[Command] 
	void CmdOnTriggerHold () { 
		RpcOnTriggerHold (); 
	}

	[ClientRpc]
	void RpcOnTriggerHold () {
		if (gun != null) {
			gun.OnTriggerHold ();
		}
	}

	public void OnTriggerRelease() {
		CmdOnTriggerRelease ();
	}

	[Command]
	void CmdOnTriggerRelease () {
		RpcOnTriggerRelease ();
	}

	[ClientRpc]
	void RpcOnTriggerRelease () {
		if (gun != null) {
			gun.OnTriggerRelease ();
		}
	}

	public float GunHeight {
		get {
			return weaponHold.position.y;
		}
	}

	public void Aim() {
		CmdAim ();
	}

	[Command]
	void CmdAim() {
		RpcAim ();
	}

	[ClientRpc]
	void RpcAim() {
		if (gun != null) {
			gun.Aim ();
		}
	}

	public void Reload() {
		if (gun != null) {
			gun.Reload ();
		}
	}
}
