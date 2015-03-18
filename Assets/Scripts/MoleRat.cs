using UnityEngine;
using System.Collections;

public class MoleRat : MonoBehaviour {

	private PlayerCharacter player;
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
	public AudioSource source;

	// Use this for initialization
	void Start () {

	
		player = FindObjectOfType<PlayerCharacter>();


	}
	
	// Update is called once per frame
	protected virtual void FixedUpdate () 
	{

		moveTime -= Time.deltaTime;
		if (moveTime <= 0)
		{
			Hunt ();
			//tail.Follow (savedPos);
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
				MoveSound();
				transform.position = transform.position + new Vector3 (-1,0,0);
			}
		}
		 if (player != null && player.transform.position.x > gameObject.transform.position.x)
		{
			{
				Ray ray = new Ray(transform.position, Vector3.right);
				if(Physics.Raycast(ray, out hit, 1))
				{
					CheckTarget(hit.transform.gameObject);
				}
				else
				{
					MoveSound();
					transform.position = transform.position + new Vector3 (1,0,0);
				}

			}
		}
		if (player != null && player.transform.position.y < gameObject.transform.position.y)
		{
			Ray ray = new Ray(transform.position, Vector3.down);
			if(Physics.Raycast(ray, out hit, 1))
			{
				CheckTarget(hit.transform.gameObject);
			}
			else
			{
				MoveSound();
				transform.position = transform.position + new Vector3 (0,0-1,0);
			}
		}

		if (player != null && player.transform.position.y > gameObject.transform.position.y)
		{
			Ray ray = new Ray(transform.position, Vector3.up);
			if(Physics.Raycast(ray, out hit, 1))
			{
				CheckTarget(hit.transform.gameObject);
			}
			else
			{
				transform.position = transform.position + new Vector3 (0,1,0);
				MoveSound();
			}


		}

		countDown  = 1f;
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

			player.HealthUpdate(100000000);

		}
	

	}

	void OnTriggerEnter(Collider other) 
	{
		
		//Destroy(other.gameObject);
		
		if (other.CompareTag ("Block"))  {
			other.GetComponent<Block> ().Destroyed ();
		}
		
		if (other.CompareTag ("Player")) {
			other.GetComponent<PlayerCharacter> ().HealthUpdate(9999999);
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

		source.clip = roar;
	source.Play();
		//AudioSource.PlayClipAtPoint (roar, savedPos);
	}
}
