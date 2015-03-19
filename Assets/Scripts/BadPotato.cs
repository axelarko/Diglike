using UnityEngine;
using System.Collections;

public class BadPotato : MonoBehaviour {
	public bool angry = false;
	public int tobig = 10;
	public int howBig = 0;
	public Vector3 position;
	public float sphereradius = 5;
	public float growthscale = 0.1F;
	public bool doomed = false;
	
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
		
		if (other.CompareTag ("Block") && angry ) {
			other.GetComponent<Block> ().Destroyed ();
		}
		
		if (other.CompareTag ("Player") && angry && doomed ) {
			other.GetComponent<PlayerCharacter> ().HealthUpdate(9999999);


		}
		
		if (other.CompareTag ("Player") != angry && doomed == false) 
		{
			int angrychanse = Random.Range(10,25);
			int healing;
			
			healing = Mathf.RoundToInt (gameObject.transform.position.y - 50);
			other.GetComponent<PlayerCharacter> ().HealthUpdate(healing);

			
			if(angrychanse != 13)
			{
				ParticleSystem particle;
				particle = Instantiate (explosion, transform.position, Quaternion.identity) as ParticleSystem;
				particle.startColor = Color.magenta;
				Destroy (particle.gameObject, 2f);
				source.clip = Dör;
				source.Play ();


				Destroy(gameObject,0.2f);
				doomed = true;
				
			}
			
			
			
			if(angrychanse == 13)
			{
				doomed = true;
				InvokeRepeating("BlackDeath",0f, 1f);



				Invoke("HeIsMadNow",4f);
			}
			
		}
		
	}
	
	public void BlackDeath()
	
	{
		source.clip = blirArg;
		source.Play ();
		ParticleSystem particle;
		particle = Instantiate (explosion, transform.position, Quaternion.identity) as ParticleSystem;
		particle.startColor = Color.black;
		Destroy (particle.gameObject, 2f);
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
			source.clip = Dör;
			source.Play();
			Destroy(gameObject,0.2f);
			//bam
		}
	}
	
	public void HeIsMadNow()
	{

		angry = true;
	}
}