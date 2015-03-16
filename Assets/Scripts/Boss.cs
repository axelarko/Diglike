using UnityEngine;
using System.Collections;

public class Boss : MonoBehaviour {
	public PlayerCharacter player;
	public float moveTime = 0.1f;
	private Block block;
	int moveCount;
	bool moving;
	bool zigzag;
	public bool left = true;
	public Collider[] hitColliders;
	void Start () 
	{
		Invoke ("WallOff", 0f);
		player = FindObjectOfType<PlayerCharacter>();

	}
	
	// Update is called once per frame
	void Update () 
	{
		DestroyBlock ();
		if (Input.GetKeyDown (KeyCode.Space))
			zigzag = !zigzag;
		moveTime -= Time.deltaTime;
		if (moveTime <= 0 && left)
		{
			Move();
			moveTime = 0.01f;
		}
		else if (moveTime <= 0 && !left)
			MoveOther();
			moveTime = 0.01f;
	}

	void WallOff()
	{
		Vector3 wallPosition;
		wallPosition = player.transform.position + new Vector3(-2,-08,0);
		{
			gameObject.transform.position = wallPosition;
			//moveCount = 200;
		}
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
		//MoveOther();
	}

	void DestroyBlock()
	{
		 {
			//Block[] blockArray;
			hitColliders = Physics.OverlapSphere(transform.position, 0f);
			foreach (Collider coll in hitColliders)
				if (coll.gameObject.tag == "Block")
					coll.gameObject.GetComponent<Block>().Destroyed();


				

		}
	}
}
