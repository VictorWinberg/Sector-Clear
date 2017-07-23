using UnityEngine;
using System.Collections;

public class Crosshairs : MonoBehaviour {

	public SpriteRenderer dot;
	public Color dotHighlightColor;
	Color dotDefaultColor;

	void Start() {
		dotDefaultColor = dot.color;
		GetComponent<SpriteRenderer> ().enabled = false;
	}

	public void activate() {
		Cursor.visible = false;
		GetComponent<SpriteRenderer> ().enabled = true;
	}

	void Update() {
		transform.Rotate(Vector3.forward * -40 * Time.deltaTime);
	}

	public void DetectTargets(Ray ray) {
		int enemyLayer = 1 << LayerMask.NameToLayer ("Enemy");
		DetectTargets (Physics.Raycast (ray, 100, enemyLayer));
	}

	public void DetectTargets(bool detected) {
		if (detected) {
			dot.color = dotHighlightColor;
		} else {
			dot.color = dotDefaultColor;
		}
	}
}
