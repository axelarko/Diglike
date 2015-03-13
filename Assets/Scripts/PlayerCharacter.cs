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

	public enum Item{empty,w,e,r,t,y};
	public Item[] inventory;
	public int selectedInvetoryItem = 0;

	private int basePower = 500000;
	private int power;
	private int baseHealth = 100000;
	private int health;
	// Use this for initialization
	void Start () 
	{
		gameObject.GetComponent<MeshRenderer> ().material.color = Color.yellow;
		power = basePower;
		health = baseHealth;
		HealthUpdate (0);
	}
	
	// Update is called once per frame
	void Update () 
	{
		Moving ();
		TempUpdate ();
		if (health <= 0)
			Death ();
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
		ParticleSystem blood;
		blood = Instantiate (particle, transform.position, Quaternion.identity) as ParticleSystem;
		blood.startColor = Color.red;
		Destroy (blood, 2f);
		gameObject.transform.DetachChildren ();
		Destroy (gameObject);

	}

	void Moving ()
	{
		if (Input.GetButtonDown("Left") && !falling)
		{
			Ray ray = new Ray(transform.position, Vector3.left);
			if(Physics.Raycast(ray, out hit, 1))
			{
				CheckTarget(hit.transform.gameObject);
			}
			else
				transform.position = transform.position + new Vector3 (-1,0,0);
		}
		else if (Input.GetButtonDown("Right") && !falling)
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
	
	void CheckTarget(GameObject obj)
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
	}
		

	void Digging(Block block)
	{
		block.OnStrike(this, power);
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
}
