using UnityEngine;
using System.Collections;

public class HunterEnemy : MonoBehaviour {
	private GameObject player;
	protected float moveTime;
	private Block block;
	private Treasure treasure;
	protected RaycastHit hit;
	protected float countDown;
	public TailEnemy tail;
	public Vector3 savedPos;
	public bool hasTail;
	public AudioClip dig;
	public AudioClip roar;
	public AudioClip hunterDeath;
	public AudioSource source;
	public ParticleSystem explosion;

	// Use this for initialization

	void OnTriggerEnter(Collider other) 
	{
		
		//Destroy(other.gameObject);
		
		if (other.CompareTag ("Potato")) {
			player = other.gameObject;
		}

	}
	void Start () {

	
		player = GameObject.FindGameObjectWithTag ("Player");



	}
	
	// Update is called once per frame
	protected virtual void FixedUpdate () 
	{
		gameObject.GetComponent<Renderer>().material.color = Color.Lerp(Color.white, Color.red, -countDown);
		moveTime -= Time.deltaTime;
		if (moveTime <= 0)
		{
			Hunt ();
			if (hasTail)
			{tail.Follow (savedPos);}

		}

	}

	protected virtual void Hunt()

	{
		savedPos = transform.position;
		if (player != null)
		if (player.transform.position.x < gameObject.transform.position.x)
		{
			Ray ray = new Ray(transform.position, Vector3.left);
			if(Physics.Raycast(ray, out hit, 1))
			{
				CheckTarget(hit.transform.gameObject);
			}
			else
			{
				gameObject.transform.rotation = Quaternion.Euler(0f,180f,0f);
				MoveSound();
				transform.position = transform.position + new Vector3 (-1,0,0);
			}
		}
		else if (player.transform.position.x > gameObject.transform.position.x)
		{
			{
				Ray ray = new Ray(transform.position, Vector3.right);
				if(Physics.Raycast(ray, out hit, 1))
				{
					CheckTarget(hit.transform.gameObject);
				}
				else
				{
					gameObject.transform.rotation = Quaternion.Euler(0f,-360f,0f);
					MoveSound();
					transform.position = transform.position + new Vector3 (1,0,0);
				}

			}
		}
		else if (player.transform.position.y < gameObject.transform.position.y)
		{
			Ray ray = new Ray(transform.position, Vector3.down);
			if(Physics.Raycast(ray, out hit, 1))
			{
				CheckTarget(hit.transform.gameObject);
			}
			else
			{
				gameObject.transform.rotation = Quaternion.Euler(0f,-360f,-99f);
				MoveSound();
				transform.position = transform.position + new Vector3 (0,0-1,0);

			}
		}

		else if (player.transform.position.y > gameObject.transform.position.y)
		{

			Ray ray = new Ray(transform.position, Vector3.up);
			if(Physics.Raycast(ray, out hit, 1))
			{
				CheckTarget(hit.transform.gameObject);
			}
			else
			{
				gameObject.transform.rotation = Quaternion.Euler(0f,-360f,99f);
				transform.position = transform.position + new Vector3 (0,1,0);
				MoveSound();
			}


		}

		countDown -= 0.1f/(0.1f*50);
		moveTime = 1+countDown;
	}

 	private void CheckTarget(GameObject obj)
	{
		if (obj.tag.Equals("Block"))
		{
			source.clip = dig;
			source.Play();
			//AudioSource.PlayClipAtPoint (dig, transform.position);
			block = obj.GetComponent<Block>();
			transform.position = block.transform.position;
			block.Destroyed();
		}
		else if (obj.tag.Equals("Treasure"))
		{

			treasure = obj.GetComponent<Treasure>();
			transform.position = treasure.transform.position;
			treasure.Destroyed();

		}
		else if (obj.tag.Equals("Player"))
		{
			player.GetComponent<PlayerCharacter> ().HealthUpdate(9999999);


		}
	
		else if (obj.tag.Equals("Potato"))
		{
			
			//play animation
			// play sound
			source.clip = hunterDeath;
			source.Play();
			//Debug.Log("dead hunter and potato");
			ParticleSystem particle;
			particle = Instantiate (explosion, transform.position, Quaternion.identity) as ParticleSystem;
			particle.startColor = Color.green;
			Destroy (particle.gameObject, 2f);
			particle = Instantiate (explosion, transform.position, Quaternion.identity) as ParticleSystem;
			particle.startColor = Color.red;
			Destroy (particle.gameObject, 2f);

			Destroy(obj);

			Destroy(gameObject,2f);
			
		}

	}

	public void Follow(Vector3 pos)
	{

		savedPos = transform.position;
		transform.position = pos;
		countDown -= 0.1f/(0.1f*50);
		if (hasTail)
		{
			tail.Follow (savedPos);
		}
	}

	void MoveSound()
	{
		//Debug.Log ("move roar");
		source.clip = roar;
		source.Play();
		//AudioSource.PlayClipAtPoint (roar, savedPos);
	}
}
