using UnityEngine;
using System;
using System.Collections;

public class gui : MonoBehaviour {

	// Update is called once per frame
	void OnGUI() {
		GridManager gm = GameObject.Find ("Main Camera").GetComponent<GridManager>();
		Tom tom = GameObject.FindGameObjectWithTag ("Player").GetComponent<Tom>();

		GUI.TextArea(new Rect(5, Screen.height - 25, 75, 20), "Health: " + tom.getHealth().ToString());
		GUI.TextArea(new Rect(Screen.width - 130, Screen.height - 25, 125, 20), "Mob Count: " + tom.getMobCount().ToString());
	}
}
