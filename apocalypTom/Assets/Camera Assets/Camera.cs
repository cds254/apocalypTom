using UnityEngine;
using System.Collections;

public class Camera : MonoBehaviour {
	public float followDistance = 0;

	private float trackSmooth = 1f;
	
	private float x;
	private float z;
	
	private float xMargin = 1f;
	private float zMargin = .25f;
	
	private float xMin;
	private float zMin;
	
	private float xMax;
	private float zMax;

	private GameObject target;

	public void start() {
		target = GameObject.FindGameObjectWithTag ("Player");
	}

	// Allow camera set on player after spawn
	public void SetTarget(GameObject t) {
		target = t;
	}

	public void LateUpdate() {
		if(target) {
			if(Mathf.Abs(transform.position.x - target.transform.position.x) > xMargin){
				x = Mathf.Lerp (transform.position.x, target.transform.position.x, trackSmooth * Time.deltaTime);
			}
			if(Mathf.Abs(transform.position.z - target.transform.position.z - followDistance) > zMargin) {
				z = Mathf.Lerp (transform.position.z, target.transform.position.z - followDistance, trackSmooth * Time.deltaTime);
			}
			transform.position = new Vector3(x, transform.position.y, z);
		}
		else {
			Debug.Log ("No player found, trying again.");
			target = GameObject.FindGameObjectWithTag ("Player");
		}
	}
	
	private float IncrementTowards(float current, float target, float accel) {
		if (current == target) {
			return current;
		} else {
			float dir = Mathf.Sign(target - current);
			current += accel * Time.deltaTime * dir;
			return (dir == Mathf.Sign (target - current)) ? current : target;
		}
	}
}
