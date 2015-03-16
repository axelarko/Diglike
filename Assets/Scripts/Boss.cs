using UnityEngine;
using System.Collections;

public class Boss : MonoBehaviour {
	public PlayerCharacter player;
	public float moveTime = 0.1f;
	private Block block;
	int moveCount;
	bool moving;
	public Collider[] hitColliders;
	void Start () 
	{
		player = FindObjectOfType<PlayerCharacter>();

	}
	
	// Update is called once per frame
	void Update () 
	{
		DestroyBlock ();
		if (Input.GetKeyDown(KeyCode.Space))
			Invoke ("WallOff", 3f);
		moveTime -= Time.deltaTime;
		if (moveTime <= 0)
		{
			Move();
			moveTime = 0.1f;
		}

	}

	void WallOff()
	{
		Vector3 wallPosition;
		wallPosition = player.transform.position + new Vector3(-10,-8,0);
		{
			gameObject.transform.position = wallPosition;
			moveCount = 200;
		}
	}

	void Move()
	{
		moving = true;
		if (moving && moveCount > 0) {
			transform.position = transform.position + (new Vector3 (1, 0, 0));
			moveCount -= 1;

		} else
		moving = false;
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
