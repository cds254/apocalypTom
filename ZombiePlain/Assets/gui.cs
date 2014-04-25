using UnityEngine;
using System;
using System.Collections;

public class gui : MonoBehaviour {

	// Update is called once per frame
	void OnGUI() {
		GridManager gm = GameObject.Find ("Main Camera").GetComponent<GridManager>();
		GameObject tom = GameObject.FindGameObjectWithTag ("Player");
		string str;
		Vector3 tmp = gm.calcGridCoord(tom.transform.position);
		int tmp2 = gm.getBiome (tom.transform.position);

		switch(tmp2) {
		case 1:
			str = "plains";
			break;
		case 2:
			str = "forest";
			break;
		case 3:
			str = "desert";
			break;
		default:
			str = tmp.ToString();
			break;
		}


		GUI.TextArea(new Rect(5, 5, 100, 50), tmp.ToString());
		GUI.TextArea(new Rect(5, 55, 100, 50), str);
		GUI.TextArea(new Rect(5, 110, 100, 50), tom.transform.position.ToString());
	}
}
