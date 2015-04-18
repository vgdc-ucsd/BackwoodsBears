using UnityEngine;
using System.Collections;

public class TitleManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButton("Submit")) {
			Application.LoadLevel("MichaelScene"); // Eventually load the woods
			Debug.Log ("It is time to enter the woods...");
		}
	}
}