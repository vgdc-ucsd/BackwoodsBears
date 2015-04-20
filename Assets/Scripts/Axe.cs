using UnityEngine;
using System.Collections;
using UnityStandardAssets._2D;

public class Axe : MonoBehaviour
{
	private Tree target;
	private ParticleSystem particles;

	// Use this for initialization
	void Start ()
	{
		target = null;
		particles = GetComponent<ParticleSystem> ();
		particles.emissionRate = 0;
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	public void ParticleBurst()
	{
		particles.Emit (5);
	}

	public void OnTriggerEnter2D(Collider2D other)
	{ 
		var tree = other.GetComponent<Tree>();
		
		if (tree != null)
		{
			target = tree;
			Debug.Log("The tree is now");
		}
	}
	
	public void OnTriggerExit2D(Collider2D other)
	{
		var tree = other.GetComponent<Tree>();
		
		if (tree != null)
		{
			target = null;
			Debug.Log("No more tree");
		}
	}

	public Tree getTree()
	{
		return target;
	}
}
