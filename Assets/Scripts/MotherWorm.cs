using UnityEngine;
using System.Collections;

public class MotherWorm : MonoBehaviour {
	public int digDirection;
	    public int direction;
		public PlayerCharacter player;
		public float moveTime = 0.1f;
		private Block block;
		int moveCount;
		bool moving;
		bool zigzag;



	public bool dorment = true;
		public bool rising;
		public bool left = true;
		public Collider[] hitColliders;
	public GameObject blockSpawn;
	public AudioSource source;
	public AudioClip wormdig;
	public AudioClip Warning;
		
	BlockSpawner spawner;

	public void Bingo (Vector3 plats)
	{
		dorment = false;
		Invoke ("WallOff", 0f);
		AudioSource.PlayClipAtPoint (Warning, plats);

	}

	void Start () 
		{
		//	spawner = FindObjectOfType<BlockSpawner>();
			moving = true;
			//Invoke ("WallOff", 0f);
			player = FindObjectOfType<PlayerCharacter>();
			
		}
		
		// Update is called once per frame
		void FixedUpdate () 
		{
		if (moveCount <= 0)
		{
			Destroy (gameObject);
		}
		if (dorment == false) {
			DestroyBlock ();
			if (Input.GetKeyDown (KeyCode.Space)) {
				zigzag = !zigzag;
			}
			moveTime -= Time.deltaTime;
			if (moveTime <= 0 && left && moving) {
				Move ();
				moveTime = 0.05f;
			} else if (moveTime <= 0 && !left && moving) {
				MoveOther ();
				moveTime = 0.05f;
			} else if (rising && moveTime <= 0) { //(moveTime <= 0 && rising && moving)
				PlayerHeight ();
				moveTime = 0.05f;
			}
		}
		}
		
		void WallOff()
		{
		direction = Random.Range (0, 2);
		Debug.Log(direction);
		if (direction == 0) 
		{

			direction = -1;
			Debug.Log("main");
		} 
		if (direction == 0)
		{

			direction = 1;
			Debug.Log("else");
		}

		if (direction == 1) 
		{
			Debug.Log("turned");
			gameObject.transform.rotation = Quaternion.Euler(0f,180f,-0f);
		}

		digDirection = direction * -1;
		int stepdown = Random.Range (0, 3);
		Vector3 wallPosition;
			
		wallPosition = player.transform.position + new Vector3(30 * (float)direction  ,-1*(float)stepdown  ,0);
			{

				gameObject.transform.position = wallPosition;
			}
			rising = !rising;
			moveCount = 50;
		}
		
		void Move()
		{
			moving = true;
			if (moving && moveCount > 0) {
				
			//GameObject goblock = Instantiate(blockSpawn,  (gameObject.transform.position - new Vector3(6f * (float)digDirection , 0f, 0f)),transform.rotation) as GameObject;
			//spawner.CreateBlock(gameObject.transform.position); //- new Vector3(6f * (float)digDirection , 0f, 0f));

			//Block goScript = goblock.GetComponent<Block> ();

			//goblock.GetComponent<MeshRenderer> ().material.color = Color.Lerp (baseColor, Color.red, timeInterval);
			//goScript.blockID = 0;
			//GameObject go = GameObject.Find ("BlockSpawner");
			/*if (go != null)
				goScript.spawner = go.GetComponent<BlockSpawner>();
			else 
				print("Couldn't find BlockSpawner");
*/
			transform.position = transform.position + (new Vector3 ( (float)digDirection, 0, 0));


				moveCount -= 1;
				
			} 
			else
			{
				
				moving = false;
				moveCount = 50;
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
				
				moveCount = 50;
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
			hitColliders = Physics.OverlapSphere (transform.position, 0f);
			foreach (Collider coll in hitColliders) {
				if (coll.gameObject.tag == "Block") {

					coll.gameObject.GetComponent<Block>().Destroyed();
					source.clip = wormdig;
					source.Play ();
			
				}
				if (coll.gameObject.tag == "Player") {

					coll.GetComponent<PlayerCharacter> ().HealthUpdate(9999999);

				}
			}
		}
		}
		

//	void OnTriggerEnter(Collider other) 
//	{
//		
//		//Destroy(other.gameObject);
//		
//
//		if (other.CompareTag ("Player") ) {
//			other.GetComponent<PlayerCharacter> ().HealthUpdate(9999999);
//		}
//		
//	}


		void PlayerHeight()
		{
			if (player != null && dorment == false )
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
			//Debug.Log (this.transform.position.y + difference);
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
