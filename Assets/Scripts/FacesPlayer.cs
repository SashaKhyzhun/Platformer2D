using UnityEngine;
using System.Collections;

public class FacesPlayer : MonoBehaviour {

	public float rotSpeed = 90f;

	Transform player;

	
	void Update () {
		if(player == null) {
			// Find the player
			GameObject go = GameObject.FindWithTag ("Player");

			if(go != null) {
				player = go.transform;
			}
		}

		// Перевіряєм чи є у нас плеєр на екрані прям зараз;

		if(player == null)
			return;	// іф нет - перевіряти наступний фрейм;

		// тут ми знаєм точно, шо у нас є 
		Vector3 dir = player.position - transform.position;
		dir.Normalize();

		float zAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;

		Quaternion desiredRot = Quaternion.Euler( 0, 0,zAngle );

		transform.rotation = Quaternion.RotateTowards( transform.rotation, desiredRot, rotSpeed * Time.deltaTime);

	}
}
