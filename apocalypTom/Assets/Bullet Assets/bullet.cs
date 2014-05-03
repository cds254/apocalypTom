using UnityEngine;
using System.Collections;

public class bullet : MonoBehaviour {

	public void OnTriggerEnter(Collider c) {
		Debug.Log("zombie died");
		GameObject.FindGameObjectWithTag("Player").GetComponent<Tom>().decreaseMobCount();
		GameObject.FindGameObjectWithTag ("Player").GetComponent<Tom> ().increaseKill ();
		Destroy(c.gameObject);
		Destroy(this.gameObject);
	}
}
