using UnityEngine;
using System.Collections;

public class BadPotato : MonoBehaviour {
	public bool angry = false;
	public int tobig = 10;
	public int howBig = 0;
	public Vector3 position;
	public float sphereradius = 5;
	public float growthscale = 0.1F;

	public AudioSource source;
	public AudioClip blirArg;
	public AudioClip växer;
	public AudioClip Dör;
	public ParticleSystem explosion;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () 
	{
	
		
			if (angry == true) {
				Anger ();
			}
	}
 
	void OnTriggerEnter(Collider other) 
	{

		//Destroy(other.gameObject);

		if (other.CompareTag ("Block")) {
			other.GetComponent<Block> ().Destroyed ();
		}
	
		if (other.CompareTag ("Player")) {
			other.GetComponent<PlayerCharacter> ().HealthUpdate(9999999);
		}

	}

	public void Anger()
	{
		transform.localScale += new Vector3(growthscale, growthscale, growthscale);

		if (howBig % 20 == 1) {
			Debug.Log(howBig);
			source.clip = växer;
			source.Play ();
		}
		//Collider[] collidersHit = Physics.OverlapSphere (transform.position, sphereradius*growthscale);

   



//		foreach (Collider i in collidersHit) 
//		{
//			i.GetComponent<Block>().Destroyed();
//		}

		howBig++;
		if (howBig >= tobig) 
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
