using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour 
{

	public float Seconds = 1;
	public float EnemyDuration = 3;
	public float EnemyDeath = 120;


	// Update is called once per frame
	void Update () 
	{


		if (Time.time >= EnemyDuration) 
		{
			transform.position += new Vector3 (1, 0, 0);
			EnemyDuration = Time.time + Seconds;
			
		}

	
		if (EnemyDuration >= EnemyDeath) 
		{
			Destroy(gameObject);
		}


	}


}
