using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	static GameManager instance{ get; private set; }

	static GameManager() {
		if (!instance) {
			instance = new GameManager();
		}
	}

	private GameManager() {
		// Constructor
	}

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
