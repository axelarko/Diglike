using UnityEngine;
using System.Collections;

public class TailEnemy : HunterEnemy 
{
	protected override void FixedUpdate()
	{
		gameObject.GetComponent<Renderer>().material.color = Color.Lerp(Color.white, Color.red, -countDown);

	}
}
