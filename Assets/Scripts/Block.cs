using UnityEngine;
using System.Collections;

public class Block : MonoBehaviour {

	public AudioClip earth;
	public AudioClip earthbreak;
	public AudioClip stone1;
	public AudioClip stone1break;
	public AudioClip stone2;
	public AudioClip stone2break;
	public AudioClip metal;
	public AudioClip metalbreak;
	public AudioClip crit;
	public AudioClip fail;
	public float volume = 0.5f;


	public int health;
	public float maxHealth;
	public int armor;
	public int level;
	public int minedOut;
	public int common;
	public int uncommon;
	public int rare;
	public int epic;
	public int legendary;

	protected int playerdmg;

	public Vector3 basePos;

	public int posY;
	public int posX;

	public int rarityFloor;
	public int rarityCap;

	public Color baseColor;

	public BlockSpawner spawner;
	public int blockID;
	protected string blockName;

	public ParticleSystem explosion;
	public Treasure treasure;

	public int baseDrop = 1;
	public int itemRate;



	//effects for flashing
	public float timeInterval;
	public float critInterval;
	public bool flashing;
	public bool critFlash;

	public bool shaking;
	protected float shakeTime;
	protected int shakeForce;

	//Time intervals for flashing and crits
	public float flashTime;
	public float critTime;



	public Vector3 goposition;
	public PlayerCharacter playerListener;


	public MotherWorm MotherWormPrefab;
	private MotherWorm go;

	protected void Start () 
	{

		playerdmg = 1;
		basePos = this.transform.position;

		playerListener = FindObjectOfType<PlayerCharacter> ();
	}
	
	// Update is called once per frame
	protected void Update () 
	{
		if (flashing)
		{
			timeInterval -= Time.deltaTime;
			if (timeInterval >= 0) {
				gameObject.GetComponent<MeshRenderer> ().material.color = Color.Lerp (baseColor, Color.red, timeInterval);
			} 
			else
			{
				flashing = false;
				gameObject.GetComponent<MeshRenderer>().material.color = baseColor;
			}
			
		}
		if (critFlash)
		{
			critInterval -= Time.deltaTime;
			if (critInterval >= 0) {
				gameObject.GetComponent<MeshRenderer> ().material.color = Color.Lerp (baseColor, Color.green, critInterval);
				
			} 
			else
			{
				critFlash = false;
				gameObject.GetComponent<MeshRenderer>().material.color = baseColor;
			}
		}


		if (shaking)
		{
			shakeTime -= Time.deltaTime;
			if (shakeTime >= 0) {
				float randomizer =	Random.Range (-0.05f, 0.051f)*shakeForce;
				float randomizer2 =	Random.Range (-0.05f, 0.051f)*shakeForce;
				this.transform.position = basePos + new Vector3 (randomizer, randomizer2, 0);
			} else
				shaking = false;
		}
		if (!shaking)
		this.transform.position = basePos;
		if ((-spawner.floorLevel)-30 >= level && spawner != null)
			Destroy(gameObject);
	}


	public virtual void Initialize(int rarityFloor, int rarityCap)
	{
		if (spawner == null)
		{
			spawner = FindObjectOfType<BlockSpawner> ();
		}
			itemRate = Mathf.RoundToInt (baseDrop + (100f +level) / 100f);
			gameObject.GetComponent<MeshRenderer>().enabled = true;
			gameObject.GetComponent<BoxCollider>().enabled = true;
			int blockType = Random.Range (rarityFloor, rarityCap);
			if (blockType < uncommon && blockType > 0) 
			{
				blockID = 1;
		 		baseColor = gameObject.GetComponent<Renderer>().material.color = Color.black;
			}
			else if (blockType < rare && common < blockType) 
			{
				blockID = 2;
				baseColor = gameObject.GetComponent<Renderer>().material.color = Color.grey;
			}
		else if (blockType < epic && rare < blockType) 
		{
			blockID = 3;
			baseColor = gameObject.GetComponent<Renderer>().material.color = Color.white;
		}
			else if (blockType < legendary && epic < blockType) 
			{
				blockID = 4;
				baseColor = gameObject.GetComponent<Renderer>().material.color = Color.yellow;
			}
			else if (legendary < blockType) 
			{
				blockID = 4;
				baseColor = gameObject.GetComponent<Renderer>().material.color = Color.green;
			}

		else if (blockType == 0) 
		{
			blockID = 4;
			gameObject.GetComponent<MeshRenderer>().enabled = false;
			gameObject.GetComponent<BoxCollider>().enabled = false;
		}
			gameObject.name = blockID + " (" + posX + ", " + posY + ")";
			maxHealth = (level + 1.25f) * 1;
			health = Mathf.RoundToInt (maxHealth);
	}

	public virtual void OnStrike(PlayerCharacter player, int damage)
	{
		shaking = true;
		shakeTime = 0.25f;
		if (!flashing)
		{
			if (critFlash)
			{
				CritSound ();
				shakeForce = 2;
				health -= damage*2;
				player.HealthUpdate(playerdmg);
			}
			else
			{
				health -= damage;
				shakeForce = 1;
				player.HealthUpdate(playerdmg);
			}

			if (health <= 0)
			{
				BreakSound ();
				MinedOut ();
				MovePlayerHere(player);
			}
			else
			{
				DigSound();
				//Flash ();
			}
		}
		else
		{
			FailSound ();
			int dmg = playerdmg * 2;
			player.HealthUpdate	 (dmg);
			Flash ();
		}
	}
	protected void FailSound()
	{
		AudioSource.PlayClipAtPoint (fail, new Vector3 (5, 1, 2));
	}

	protected void DigSound()
	{/*
		if (blockID == 0) {AudioSource.PlayClipAtPoint (earth, gameObject.transform.position, volume);}
		if (blockID == 1) {AudioSource.PlayClipAtPoint (stone1, gameObject.transform.position, volume);}
		if (blockID == 2) {AudioSource.PlayClipAtPoint (stone2, gameObject.transform.position, volume);}
		if (blockID == 3) {AudioSource.PlayClipAtPoint (metal, gameObject.transform.position, volume);}*/

		PlayerPosition ();
		
		if (blockID == 0) {
			
			
			AudioSource.PlayClipAtPoint (earth, gameObject.transform.position,  volume);}
		if (blockID == 1) {AudioSource.PlayClipAtPoint (stone1, gameObject.transform.position, volume);}
		if (blockID == 2) {AudioSource.PlayClipAtPoint (stone2, gameObject.transform.position, volume);}
		if (blockID == 3) {AudioSource.PlayClipAtPoint (metal, gameObject.transform.position, volume);}
	}
	protected void CritSound()
	{
		AudioSource.PlayClipAtPoint (crit, new Vector3 (5, 1, 2));
	}

	protected void BreakSound()
	{

		/*
		if (earth != null && stone1 != null && stone2 != null && metal != null)
		{
			if (blockID == 0) {AudioSource.PlayClipAtPoint (earth, new Vector3 (5, 1, 2));}
			if (blockID == 1) {AudioSource.PlayClipAtPoint (stone1, new Vector3 (5, 1, 2));}
			if (blockID == 2) {AudioSource.PlayClipAtPoint (stone2, new Vector3 (5, 1, 2));}
			if (blockID == 3) {AudioSource.PlayClipAtPoint (metal, new Vector3 (5, 1, 2));}
		}*/

		PlayerPosition ();
		
		if (blockID == 0) {AudioSource.PlayClipAtPoint (earthbreak, new Vector3 (5, 1, 2),volume);}
		if (blockID == 1) {AudioSource.PlayClipAtPoint (stone1break, new Vector3 (5, 1, 2),volume);}
		if (blockID == 2) {AudioSource.PlayClipAtPoint (stone2break, new Vector3 (5, 1, 2),volume);}
		if (blockID == 3) {AudioSource.PlayClipAtPoint (metalbreak, new Vector3 (5, 1, 2),volume);}


	}

	protected void PickUpSound()
	{

	}

	public float PlayerPosition ()
	{
		if (playerListener != null) {
			goposition = playerListener.transform.position; // - gameObject.transform.position;
		
			float x1 = goposition.x;
			float y1 = goposition.y;
		
			float x2 = gameObject.transform.position.x;
			float y2 = gameObject.transform.position.y;
		
			volume = (1) / Mathf.Sqrt (Mathf.Pow (x2 - x1, 2) + Mathf.Pow (y2 - y1, 2));
		

		} 
		return volume;
	}

	public void RiskOfWorm()
	{
		int wormbingo = Random.Range (0, 15);
		if (wormbingo == 1) 
		{
			Debug.Log("Bingo");
			
			if (go  != null)
			{
				Destroy(go);
				
			}
			
			go = Instantiate(MotherWormPrefab, new Vector3 (-99f, 112f, 99f),transform.rotation) as MotherWorm;
			go.GetComponent<MotherWorm> ().Bingo( gameObject.transform.position);
		}
	}

	protected virtual void MinedOut()
	{
		transform.position = basePos;
		ParticleSystem particle;
		particle = Instantiate (explosion, transform.position, Quaternion.identity) as ParticleSystem;
		particle.startColor = baseColor;
		Destroy (particle.gameObject, 2f);
		spawner.airSpawn = this.transform.position;
		spawner.CreateAir (posX, posY);
		int dropchance = Random.Range (0, 101);
		if (dropchance <= itemRate)
		{
			Treasure reward;
			reward = Instantiate (treasure, transform.position,Quaternion.identity) as Treasure;
			reward.spawner = spawner;
			reward.level = level;
			// rolls quality
			reward.Initialize (0, 3);
		}
		RiskOfWorm ();
		BreakSound ();
		Destroy (gameObject);
	}

	public void Destroyed()
	{
		BreakSound ();
		transform.position = basePos;
		ParticleSystem particle;
		particle = Instantiate (explosion, transform.position, Quaternion.identity) as ParticleSystem;
		particle.startColor = baseColor;
		Destroy (particle.gameObject, 2f);
		spawner.airSpawn = this.transform.position;
		spawner.CreateAir (posX, posY);
		Destroy (gameObject);
	}



	public virtual void Flash()
	{
		timeInterval = flashTime;
		if (!flashing)
		flashing = !flashing;
	}

	protected void CriticalFlash()
	{
		critInterval = critTime;
		if (!critFlash)
		critFlash = !critFlash;
	}

	public void Pulse()
	{
		if (!flashing)
		{
			critInterval = critTime;
			if (!critFlash)
			critFlash = !critFlash;
		}
	/*	else
		{
			timeInterval = flashTime;
			if (!flashing)
			flashing = !flashing;
		}*/
	}
	void MovePlayerHere(PlayerCharacter player)
	{
		if (player.transform.position.y == gameObject.transform.position.y + 1f) 
		{
			spawner.LevelUp(player.level+1);
		}
		player.transform.position = gameObject.transform.position;

	}
}
