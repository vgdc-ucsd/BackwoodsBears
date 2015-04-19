using UnityEngine;
using System.Collections;

public class ShakeTree : MonoBehaviour
{
	private Transform treeRoot;
	private float shakeAmount = 0;

	// Use this for initialization
	void Start ()
	{
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	public void Shake(float maxAngle, float direction)
	{

	}

	public void setTreeRoot(Transform newRoot)
	{
		treeRoot = newRoot;
	}

	public void setShake(float newShake)
	{
		shakeAmount = newShake;
	}
}
