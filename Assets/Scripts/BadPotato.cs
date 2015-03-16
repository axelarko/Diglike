using UnityEngine;
using System.Collections;

public class BadPotato : MonoBehaviour {
	public bool angry = false;
	public int tobig = 0;
	public float time =1;
	public float cd = 3;
	public Vector3 position;
	public float sphereradius = 5;
	public float growthscale = 0.1F;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Time.time >= time) 
		{
		
			if (angry == true) {
				Anger ();
			}
		}
	}
 
	public void Anger()
	{
		transform.localScale += new Vector3(growthscale, growthscale, growthscale);
		Collider[] collidersHit = Physics.OverlapSphere (position, sphereradius);
		foreach (Collider i in collidersHit) 
		{
			i.GetComponent<Block>().Destroyed();
		}
		tobig++;
		cd = Time.time + time;

		if (tobig >= 5) 
		{
			Destroy(gameObject);
			//bam
		}
	}
	
	public void HeIsMadNow()
	{
		angry = true;
	}
}
