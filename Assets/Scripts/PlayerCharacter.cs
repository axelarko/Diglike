using UnityEngine;
using System.Collections;

public class PlayerCharacter : MonoBehaviour {
	private Block block;
	private Treasure treasure;
	private Chest chest;
	private	RaycastHit hit;
	public bool falling = false;
	public int level;


	public BlockSpawner spawner;
	public GameManager manager;
	public ParticleSystem particle;
	public AudioClip dead;

	public enum Item{empty,w,e,r,t,y};
	public Item[] inventory;
	public int selectedInvetoryItem = 0;


	private int basePower = 10;
	private int power;
	private int baseHealth = 100000;
	private int health;

	private float beat;
	private float digWindow;
	public int pulse;
	public bool canDig = false;

	public float leftBounds;
	public float rightBounds;


	// Use this for initialization
	void Start () 
	{

		//gameObject.GetComponent<MeshRenderer> ().material.color = Color.yellow;
		power = basePower;
		health = baseHealth;
		HealthUpdate (0);
	}
	
	// Update is called once per frame
	void Update () 
	{

		TempUpdate ();
		Moving ();
		if (health <= 0)
		{
			Death ();
		}
	}

	void FixedUpdate()
	{
		BeatPulse ();
	}


	void TempUpdate (){
		if(Input.GetKeyDown(KeyCode.P)){
			AddToInvetory(Item.w);
		}
		if(Input.GetKeyDown(KeyCode.O)){
			AddToInvetory(Item.e);
		}
	}


	public void AddToInvetory(Item itemToAdd){
		if (!InventoryFull ()) {
			int spot = 0;
			foreach (Item i in inventory) {
				if (i == Item.empty) {
					inventory[spot] = itemToAdd;
					return;
				}
				else{
					spot++;
				}
			}
		} 
		else {
			inventory[selectedInvetoryItem] = itemToAdd;
		}
	}
	public bool InventoryFull()
	{
		foreach (Item i in inventory) {
			if(i == Item.empty){
				return false;
			}
		}
		return true;
	}

	void DigAnimation()
	{

	}

	void Death()
	{

		manager.GameOver ();
		AudioSource.PlayClipAtPoint (dead, new Vector3 (5, 1, 2));
		ParticleSystem blood;
		blood = Instantiate (particle, transform.position, Quaternion.identity) as ParticleSystem;
		blood.startColor = Color.red;
		Destroy (blood, 2f);
		gameObject.transform.DetachChildren ();
		Destroy (gameObject);

	}

	void Moving ()
	{
		if (Input.GetButtonDown("Left") && !falling && gameObject.transform.position.x != leftBounds)
		{
			Ray ray = new Ray(transform.position, Vector3.left);
			if(Physics.Raycast(ray, out hit, 1))
			{
				CheckTarget(hit.transform.gameObject);
			}
			else

				transform.position = transform.position + new Vector3 (-1,0,0);
		}
		else if (Input.GetButtonDown("Right") && !falling && gameObject.transform.position.x != rightBounds)
		{
			{
				Ray ray = new Ray(transform.position, Vector3.right);
				if(Physics.Raycast(ray, out hit, 1))
				{
					CheckTarget(hit.transform.gameObject);
				}
				else
				transform.position = transform.position + new Vector3 (1,0,0);
			}
		}
		else if (Input.GetButtonDown("Down") && !falling)
		{
			Ray ray = new Ray(transform.position, Vector3.down);
			if(Physics.Raycast(ray, out hit, 1))
			{
				CheckTarget(hit.transform.gameObject);
			}
		}
		Falling ();
	}
	
	public void CheckTarget(GameObject obj)
	{
		if (obj.tag.Equals("Block"))
		{
			block = obj.GetComponent<Block>();
			Digging (block);
		}
		else if (obj.tag.Equals("Treasure"))
		{
			treasure = obj.GetComponent<Treasure>();
			if(treasure.transform.position == gameObject.transform.position+new Vector3(0,-1,0))
		{
			   level +=1;
			   spawner.LevelUp(level);
		}
			transform.position = treasure.transform.position;
			treasure.PickUp(this);
		}
			else if (obj.tag.Equals("Chest"))
		{
			chest = obj.GetComponent<Chest>();
			chest.OnStrike(this, 1);
		}
		else if (obj.tag.Equals("Potato"))
		{
			transform.position = obj.transform.position;
		}
	}
		

	void Digging(Block block)
	{
		if (canDig && digWindow > 0) {
			block.OnStrike (this, power);
			canDig = !canDig;
		} else
		{
			block.critInterval = 0;
			block.Flash();
		}
	}

	public void Falling()
	{
		Ray ray = new Ray(transform.position, Vector3.down);
		if (Physics.Raycast (ray, out hit, 1)) 
		{
			if (falling)
			falling = !falling;
		//	Debug.Log ("You're not falling at all man");
		} 
		else
		{
			if (!falling) 
		
				falling = !falling;
		transform.position = transform.position + new Vector3 (0,-1,0);
			level +=1;
			spawner.LevelUp(level);
		 }

	}

	public void PickUp(string treasureType, int value)
	{
		Debug.Log ("Acquired " + treasureType + "!");
		if (treasureType == "Potion") 
		{
			HealthUpdate(-value);
		}
	}

	public void HealthUpdate(int value)
	{
		health -= value;
		manager.PlayerHealth(health);
	}

	void BeatPulse()
	{
		digWindow -= Time.deltaTime;
		beat -= Time.deltaTime;
		if (beat <= 0) 
		{
			if (!canDig)
			{
				canDig = !canDig;
			}
			Ray ray;
			Ray ray1;
			Ray ray2;
			ray = new Ray(transform.position,Vector3.down);
			ray1 = new Ray(transform.position,Vector3.left);
			ray2 = new Ray(transform.position,Vector3.right);
			if(Physics.Raycast(ray, out hit, 1))
			{
				if (hit.transform.gameObject.CompareTag("Block"))
				hit.transform.gameObject.GetComponent<Block>().Pulse();
			}
			if(Physics.Raycast(ray1, out hit, 1))
			{
				if (hit.transform.gameObject.CompareTag("Block"))
				hit.transform.gameObject.GetComponent<Block>().Pulse();
			}
			if(Physics.Raycast(ray2, out hit, 1))
			{
				if (hit.transform.gameObject.CompareTag("Block"))
				hit.transform.gameObject.GetComponent<Block>().Pulse();
			}
			//beat = 0.47625f;
			beat = 0.5f;
			digWindow = beat/(1/3);

		}
	}

}
