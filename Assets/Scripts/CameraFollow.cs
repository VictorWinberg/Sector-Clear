using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	public float speed = 1f;
	private GameObject target;
	private Vector3 offset;

	public void SetTarget(GameObject target) {
		this.target = target;
		offset = transform.position - Vector3.zero;
	}

	void LateUpdate () {
		if (target) {
			transform.position = Vector3.Lerp (transform.position, target.transform.position + offset, speed * Time.deltaTime);
		}
	}
}
