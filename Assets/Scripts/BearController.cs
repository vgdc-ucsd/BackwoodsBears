using UnityEngine;
using System.Collections;

public class BearController : MonoBehaviour {

	public enum State {
		PROWLING,
		ALERTED,
		PLAYER_ON_GROUND,
		PLAYER_IN_TREE
	};
	public State activity;
	public const State PROWLING = State.PROWLING, 
					   ALERTED = State.ALERTED, 
					   PLAYER_ON_GROUND = State.PLAYER_ON_GROUND, 
					   PLAYER_IN_TREE = State.PLAYER_IN_TREE;
	public GameObject leftEye, rightEye;
	public GameObject player;
	public float patrolDistance;
	public float patrolTime;
	public bool patroling;

	// Use this for initialization
	void Start () {
		activity = State.PROWLING;
	}
	
	// Update is called once per frame
	void Update () {
		switch (activity) {
		case PROWLING:
			if(!patroling)
			StartCoroutine(Patrol());
			break;

		}
	}

	IEnumerator Patrol() {
		patroling = true;
		// Patrol to the left
		Debug.Log ("pre");
		for (float xx = 0.0f; xx < patrolDistance; xx += patrolDistance / patrolTime) {
			transform.position -= new Vector3( patrolDistance / patrolTime, 0, 0 );
			yield return null;
		}

		transform.localScale = new Vector3( transform.localScale.x * -1, transform.localScale.y, transform.localScale.z );

		// Patrol to the right
		for (float xx = 0.0f; xx < patrolDistance; xx += patrolDistance / patrolTime) {
			transform.position += new Vector3( patrolDistance / patrolTime, 0, 0 );
			yield return null;
		}

		transform.localScale = new Vector3( transform.localScale.x * -1, transform.localScale.y, transform.localScale.z );
		patroling = false;

	}

	public bool CanSeePlayer() {
		return false;
	}

	// If we've detected a player is in a tree, shake it
	public void ShakeTree(GameObject tree) {

	}

	// Fire a laser from this bear's eyes at a given target
	public virtual void FireLaser (GameObject target) {}

	// Use your laser to burn down a tree. CAN'T ESCAPE
	public virtual void VaporizeTree (GameObject target) {}
}