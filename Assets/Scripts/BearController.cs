using UnityEngine;
using System.Collections;

public class BearController : MonoBehaviour {

	public enum State { 
		PROWLING,
		ALERTED,
		PLAYER_ON_GROUND,
		PLAYER_IN_TREE,
		DEAD
	};
	public State activity;
	public const State PROWLING = State.PROWLING, 
					   ALERTED = State.ALERTED, 
					   PLAYER_ON_GROUND = State.PLAYER_ON_GROUND, 
					   PLAYER_IN_TREE = State.PLAYER_IN_TREE,
					   DEAD = State.DEAD;
//	public GameObject leftEye, rightEye;
	public PolygonCollider2D visionCone;
	public GameObject player;
	public float patrolDistance;
	public float patrolTime;
	public bool patroling;
	public bool playerInCone;

	// Use this for initialization
	void Start () {
		activity = State.PROWLING;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("Cancel")) {
			Kill ();
		}

		switch (activity) {
		case PROWLING:
			if (playerInCone) {
				activity = ALERTED;
			}
			else if (!patroling)
				StartCoroutine(Patrol());
			break;
		case ALERTED:
			break;
		case PLAYER_IN_TREE:
			break;
		case PLAYER_ON_GROUND:
			break;
		}
	}

	IEnumerator Patrol() {
		patroling = true;

		// Patrol to the left
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

	// Play death animation
	public void Kill() {
		if (activity != DEAD) {
			activity = DEAD;
			StartCoroutine (CoKill ());
		}
	}

	// Death animation
	private IEnumerator CoKill() {
		// Play death animation

		// Wait for a few seconds
//		yield return new WaitForSeconds (3f);

		Debug.Log ("A Bear has been killed.");

		// Fade out & destroy

		Color tempColor = GetComponent<Renderer>().material.color;

		for (int frames = 60; frames > 0; frames--) {

			float alpha = frames / 60.0f;
			tempColor = new Color (tempColor.r, tempColor.g, tempColor.b, alpha);
			GetComponent<Renderer> ().material.color = tempColor;
			yield return null;

		}

		// Set alpha to zero explicitly
		tempColor = new Color (tempColor.r, tempColor.g, tempColor.b, 0.0f);
		GetComponent<Renderer> ().material.color = tempColor;

		GetComponent<BoxCollider2D> ().enabled = false; // Disable box collider
		Destroy (this);
	}

	public void SetSeeingPlayer(bool b) {
		playerInCone = b;
	}

	// If we've detected a player is in a tree, shake it
	public void ShakeTree(GameObject tree) {

	}

	// Fire a laser from this bear's eyes at a given target
	public virtual void FireLaser (GameObject target) {}

	// Use your laser to burn down a tree. CAN'T ESCAPE
	public virtual void VaporizeTree (GameObject target) {}
}