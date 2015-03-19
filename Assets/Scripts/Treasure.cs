using UnityEngine;
using System.Collections;

public class Treasure : Block {
	private int treasureID;

	private string treasureType;
	private int value = 0;
	private PlayerCharacter.Item item =PlayerCharacter.Item.empty;

	public override void Initialize(int floor, int cap)
	{
		GetComponent<MeshRenderer>().material.color = Color.magenta;
		treasureID = Random.Range (floor, cap + 1);
		if (treasureID == 0)
			treasureType = "Death Scissors of Certain Breakfast";
		else if (treasureID == 1)
			treasureType = "Devious Shovel of Broken Teeth";
		else if (treasureID == 2)
			treasureType = "Screaming Screwdriver of Offended Bears";
		else if (treasureID == 3)

		{
			treasureType = "Potion";
			value = 10;
		}
		 else if (treasureID == 101) 
		{
			item = PlayerCharacter.Item.w;
		}

			treasureType = "Potion";
			value = 10;

		{
			FindPlayer();
		}

	}

	public void PickUp(PlayerCharacter player)
	{
		player.PickUp (treasureType, value);
		Destroy (gameObject);

	if (treasureID < 100) {
		player.PickUp (treasureType, value);
		Destroy (gameObject);
	} 
	else 
	{
		player.AddToInvetory(item);
	}
	}

	void FindPlayer()
	{
		{
			PlayerCharacter player = FindObjectOfType<PlayerCharacter>();

			if (player != null)
			{
				player.CheckTarget(gameObject);
			}

		}
	}

}
