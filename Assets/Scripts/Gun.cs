using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {

	private Player player;
	public Player Player {
		get { return this.player; }
		set { this.player = value; }
	}

	public Transform muzzle;
	public Projectile projectile;
	public float msBetweenShots = 100;
	public float muzzleVelocity = 35;

	float nextShotTime;

	public void Shoot() {
		if (Time.time > nextShotTime) {
			nextShotTime = Time.time + msBetweenShots / 1000;

			Projectile newProjectile = Instantiate (projectile, muzzle.position, muzzle.rotation) as Projectile;
			newProjectile.SetSpeed (muzzleVelocity);
			newProjectile.Player = player;
			Destroy(newProjectile.gameObject, 2);
		}
	}
}
