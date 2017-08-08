using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;
using System.Collections;

[RequireComponent (typeof (NavMeshAgent))]
public class Enemy : LivingEntity {

	public enum State {Idle, Chasing, Attacking};
	State currentState;

	public ParticleSystem deathEffect;

	public static event System.Action OnDeathStatic;

	NavMeshAgent pathfinder;
	Transform target;
	LivingEntity targetEntity;
	Material skinMaterial;

	Color originalColour;

	private float attackDistanceThreshold = .5f;
	private float timeBetweenAttacks = 1;
	private int damage = 10;

	float nextAttackTime, myCollisionRadius, targetCollisionRadius;

	bool hasTarget;

	void Awake () {
		pathfinder = GetComponent<NavMeshAgent> ();
		skinMaterial = GetComponent<Renderer> ().material;
		originalColour = skinMaterial.color;
	}

	protected override void Start () {
		base.Start ();
		if (!isServer)
			return;

		StartCoroutine (FindTarget ());
	}

	public void SetCharacteristics (float moveSpeed, int damage, int health, Color skinColor) {
		pathfinder.speed = moveSpeed;

		if (hasTarget) this.damage = damage;
		startingHealth = health;

		deathEffect.startColor = new Color (skinColor.r, skinColor.g, skinColor.b, 1);
		skinMaterial = GetComponent<Renderer> ().material;
		skinMaterial.color = skinColor;
		originalColour = skinMaterial.color;
	}

	public override void TakeDamage (int damage) {
		AudioManager.instance.PlaySound ("Impact", transform.position);
		if (damage <= health) {
			if (OnDeathStatic != null) {
				OnDeathStatic ();
			}
			AudioManager.instance.PlaySound ("Enemy Death", transform.position);
			Destroy(Instantiate(deathEffect.gameObject, transform.position, Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up)) as GameObject, deathEffect.main.startLifetimeMultiplier);
		}
		base.TakeDamage (damage);
	}

	void OnTargetDeath() {
		targetEntity.OnDeath -= OnTargetDeath;
		hasTarget = false;
		currentState = State.Idle;
		StartCoroutine (FindTarget ());
	}

	void Update () {
		if (!isServer)
			return;

		if (hasTarget) {
			if (Time.time > nextAttackTime) {
				float sqrDstToTarget = (target.position - transform.position).sqrMagnitude;
				if (sqrDstToTarget < Mathf.Pow (attackDistanceThreshold + myCollisionRadius + targetCollisionRadius, 2)) {
					nextAttackTime = Time.time + timeBetweenAttacks;
					AudioManager.instance.PlaySound ("Enemy Attack", transform.position);
					StartCoroutine (Attack ());
				}
			}
		}

		if (transform.position.y < -10) {
			TakeDamage (health);
		}
	}

	IEnumerator Attack() {
		currentState = State.Attacking;
		pathfinder.enabled = false;

		Vector3 originalPosition = transform.position;
		Vector3 dirToTarget = (target.position - transform.position).normalized;
		Vector3 attackPosition = target.position - dirToTarget * (myCollisionRadius);

		float attackSpeed = 3;
		float percent = 0;

		skinMaterial.color = Color.red;
		bool hasAppliedDamage = false;

		while (percent <= 1) {

			if(percent >= 0.5f && !hasAppliedDamage) {
				hasAppliedDamage = true;
				DealDamage(damage, targetEntity.gameObject);
			}
			percent += Time.deltaTime * attackSpeed;
			float interpolation = (-Mathf.Pow(percent,2) + percent) * 4;

			RpcAttack (originalPosition, attackPosition, interpolation);

			yield return null;
		}

		skinMaterial.color = originalColour;
		currentState = State.Chasing;
		pathfinder.enabled = true;
	}

	[ClientRpc]
	void RpcAttack(Vector3 originalPosition, Vector3 attackPosition, float interpolation) {
		transform.position = Vector3.Lerp(originalPosition, attackPosition, interpolation);
	}

	// UpdatePath is called once per refreshRate
	IEnumerator UpdatePath() {
		float refreshRate = .25f;

		while (hasTarget) {
			if (currentState == State.Chasing) {
				Vector3 dirToTarget = (target.position - transform.position).normalized;
				Vector3 targetPosition = target.position - dirToTarget * (myCollisionRadius + targetCollisionRadius + attackDistanceThreshold/2);
				if (!dead) {
					RpcSetDestination (targetPosition);
				}
			}
			yield return new WaitForSeconds(refreshRate);
		}
	}

	[ClientRpc]
	void RpcSetDestination(Vector3 position) {
		if (pathfinder.enabled) {
			pathfinder.SetDestination (position);
		}
	}

	IEnumerator FindTarget() {
		float refreshRate = 1.0f;

		while (!hasTarget) {
			yield return new WaitForSeconds(refreshRate);

			GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");
			if (players != null && players.Length > 0) {
				currentState = State.Chasing;
				hasTarget = true;

				GameObject[] targets = GameObject.FindGameObjectsWithTag ("Player");

				target = targets[Random.Range(0, targets.Length)].transform;
				targetEntity = target.GetComponent<LivingEntity> ();
				targetEntity.OnDeath += OnTargetDeath;

				myCollisionRadius = GetComponent<CapsuleCollider> ().radius;
				targetCollisionRadius = target.GetComponent<CapsuleCollider> ().radius;

				StartCoroutine (UpdatePath ());
			}
		}
	}

	protected override void Die() {
		base.Die ();
		dead = true;
		if (targetEntity != null) {
			targetEntity.OnDeath -= OnTargetDeath;
		}
		Destroy (gameObject);
	}
}
