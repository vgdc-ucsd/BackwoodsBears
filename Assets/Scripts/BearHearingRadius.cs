using UnityEngine;
using System.Collections;

public class BearHearingRadius : MonoBehaviour {

	public BearController bc;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerStay2D(Collider2D other) {
		string otherTag = other.gameObject.tag;
		if (otherTag == "Player") {
			if ( Input.anyKey ) {
				// If the player is pressing any key (not pressing no keys)
				// then the bear can hear you
				bc.AlertBear();
			}
		}
	}
}
