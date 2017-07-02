using UnityEngine;
using System.Collections;

[RequireComponent (typeof (UnityEngine.AI.NavMeshAgent))]
public class Enemy : LivingEntity {

	UnityEngine.AI.NavMeshAgent pathfinder;
	Transform target;

	// Use this for initialization
	protected override void Start () {
		base.Start ();
		pathfinder = GetComponent<UnityEngine.AI.NavMeshAgent> ();
		target = GameObject.FindGameObjectWithTag ("Player").transform;

		StartCoroutine (UpdatePath());
	}
	
	// UpdatePath is called once per refreshRate
	IEnumerator UpdatePath() {
		float refreshRate = .25f;

		while (target != null) {
			Vector3 targetPosition = new Vector3(target.position.x, 0, target.position.z);
			if(!dead) {
				pathfinder.SetDestination (targetPosition);
			}
			yield return new WaitForSeconds(refreshRate);
		}
	}
}
