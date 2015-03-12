using UnityEngine;
using System.Collections;

public class Block : MonoBehaviour {
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
	protected void Start () 
	{
		playerdmg = 1;
		basePos = this.transform.position;
	}
	
	// Update is called once per frame
	protected void Update () 
	{
		if (flashing)
		{
		timeInterval -= Time.deltaTime;
		if (timeInterval >= 0) {
			gameObject.GetComponent<MeshRenderer> ().material.color = Color.Lerp (baseColor, Color.red, timeInterval);
		} else
			flashing = false;
		
		}
		if (critFlash)
		{
			critInterval -= Time.deltaTime;
			if (critInterval >= 0) {
				gameObject.GetComponent<MeshRenderer> ().material.color = Color.Lerp (baseColor, Color.green, critInterval);
				
			} else
				critFlash = false;
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
			maxHealth = (level + 1.25f) * 10;
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
				shakeForce = 2;
				health -= damage*2;
				//critsound
			}
			else
			health -= damage;
			shakeForce = 1;
			player.HealthUpdate(playerdmg);
			//miningsound

		if (health <= 0)
		{
			MinedOut ();
		}
		Flash ();
		}
		else
		{
			int dmg = playerdmg * 2;
			player.HealthUpdate	 (dmg);
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
		Destroy (gameObject);
	}

	public void Destroyed()

	{
		spawner.airSpawn = this.transform.position;
		spawner.CreateAir (posX, posY);
		Destroy (gameObject);
	}



	protected virtual void Flash()
	{
		Invoke ("CriticalFlash", flashTime);
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

}
