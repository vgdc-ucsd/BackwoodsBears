using UnityEngine;
using System.Collections;

public class DetectPlayer : MonoBehaviour {

	public BearController bc;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerStay2D(Collider2D other) {
		var objTag = other.gameObject.tag;
		if ( objTag == "Player" ) {
			bc.SetSeeingPlayer(true);
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		var objTag = other.gameObject.tag;
		if ( objTag == "Player" ) {
			bc.SetSeeingPlayer(false);
		}
	}
}
