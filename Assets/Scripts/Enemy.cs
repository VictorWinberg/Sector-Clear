using UnityEngine;
using System.Collections;

[RequireComponent (typeof (UnityEngine.AI.NavMeshAgent))]
public class Enemy : LivingEntity {

	public enum State {Idle, Chasing, Attacking};
	State currentState;

	UnityEngine.AI.NavMeshAgent pathfinder;
	Transform target;
	LivingEntity targetEntity;
	Material skinMaterial;

	Color originalColour;

	float attackDistanceThreshold = .5f;
	float timeBetweenAttacks = 1;
	int damage = 10;

	float nextAttackTime, myCollisionRadius, targetCollisionRadius;

	bool hasTarget;

	protected override void Start () {
		base.Start ();
		pathfinder = GetComponent<UnityEngine.AI.NavMeshAgent> ();
		skinMaterial = GetComponent<Renderer> ().material;
		originalColour = skinMaterial.color;

		StartCoroutine (FindTarget ());
	}

	void OnTargetDeath() {
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
					StartCoroutine (Attack ());
				}
			}
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
			transform.position = Vector3.Lerp(originalPosition, attackPosition, interpolation);

			yield return null;
		}

		skinMaterial.color = originalColour;
		currentState = State.Chasing;
		pathfinder.enabled = true;
	}

	// UpdatePath is called once per refreshRate
	IEnumerator UpdatePath() {
		float refreshRate = .25f;

		while (hasTarget) {
			if (currentState == State.Chasing) {
				Vector3 dirToTarget = (target.position - transform.position).normalized;
				Vector3 targetPosition = target.position - dirToTarget * (myCollisionRadius + targetCollisionRadius + attackDistanceThreshold/2);
				if (!dead) {
					pathfinder.SetDestination (targetPosition);
				}
			}
			yield return new WaitForSeconds(refreshRate);
		}
	}

	IEnumerator FindTarget() {
		float refreshRate = 1.0f;

		while (!hasTarget) {
			if (GameObject.FindGameObjectWithTag ("Player") != null) {
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

			yield return new WaitForSeconds(refreshRate);
		}
	}
}
