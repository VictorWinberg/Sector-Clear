using UnityEngine;
using System.Collections;

public interface IDamageable {

	void TakeHit(int damage, RaycastHit hit);
}
