using UnityEngine;
using System.Collections;

public class Boss : MonoBehaviour {
	public PlayerCharacter player;
	public float moveTime = 0.1f;
	private Block block;
	int moveCount;
	bool moving;
	bool zigzag;
	public bool rising;
	public bool left = true;
	public Collider[] hitColliders;
	void Start () 
	{
		moving = true;
		Invoke ("WallOff", 0f);
		player = FindObjectOfType<PlayerCharacter>();

	}
	
	// Update is called once per frame
	void Update () 
	{
		DestroyBlock ();
		if (Input.GetKeyDown (KeyCode.Space))
		{
			zigzag = !zigzag;
		}
		moveTime -= Time.deltaTime;
		if (moveTime <= 0 && left && moving)
		{
			Move();
			moveTime = 0.01f;
		}
		else if (moveTime <= 0 && !left && moving)
		{
			MoveOther();
			moveTime = 0.01f;
		}
		else if (rising && moveTime <= 0) //(moveTime <= 0 && rising && moving)
		{
			PlayerHeight ();
			moveTime = 0.1f;
		}

	}

	void WallOff()
	{
		Vector3 wallPosition;
		wallPosition = player.transform.position + new Vector3(-2,-08,0);
		{
			gameObject.transform.position = wallPosition;
		}
		rising = !rising;
		moveCount = 100;
		Destroy(gameObject,20f);
	}

	void Move()
	{
		moving = true;
		if (moving && moveCount > 0) {
			transform.position = transform.position + (new Vector3 (1, 0, 0));
			moveCount -= 1;

		} 
		else
		{

			moving = false;
			moveCount = 100;
			if (zigzag)
			{
				transform.position = transform.position + (new Vector3 (0, 1, 0));
				left = !left;
			}
		}
	}
	void MoveOther()
	{
		moving = true;
		if (moving && moveCount > 0) {
			transform.position = transform.position + (new Vector3 (-1, 0, 0));
			moveCount -= 1;
			
		} 
		else
		{
			moving = false;

			moveCount = 100;
			if (zigzag)
			{
				transform.position = transform.position + (new Vector3 (0, 1, 0));
				left = !left;
			}
		}
	}

	void DestroyBlock()
	{
		 {
			hitColliders = Physics.OverlapSphere(transform.position, 0f);
			foreach (Collider coll in hitColliders)
			if (coll.gameObject.tag == "Block")
			{
				coll.gameObject.GetComponent<Block>().Destroyed();
			}
		}
	}

	void PlayerHeight()
	{
		if (player != null)
		{
			float playerHeight = player.transform.position.y;
			float difference = playerHeight + 10;
			RiseUp (difference);
		}
		else 
		{
			Debug.Log ("No player");
		}
	}

	void RiseUp(float difference)
	{
		Debug.Log (this.transform.position.y + difference);
		if (difference > gameObject.transform.position.y)
		transform.position = transform.position + new Vector3 (0, 1, 0);
	}

	void LocatePlayer()
	{

	}

	void CrashDown()
	{

	}

}
