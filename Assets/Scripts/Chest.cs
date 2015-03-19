using UnityEngine;
using System.Collections;

public class Chest : Block 
{
	public override void OnStrike(PlayerCharacter player, int damage)
	{
		shaking = true;
		shakeTime = 0.25f;
			if (!critFlash)
			{
			critFlash = !critFlash;
				shakeForce = 5;
				health -= 1;
				//critsound
			}

			if (health <= 0)
			{
				MinedOut ();
			}
	}

	protected override void MinedOut()
	{
		ParticleSystem particle;
		transform.position = basePos;
		particle = Instantiate (explosion, basePos, Quaternion.identity) as ParticleSystem;
		particle.startColor = Color.yellow;
		Destroy (particle.gameObject, 2f);
		spawner.airSpawn = this.transform.position;
		spawner.CreateAir (posX, posY);
		int itemTier = Random.Range (0, 101);
		if (itemTier >= 80)
		{
			Treasure reward;
			reward = Instantiate (treasure, transform.position,Quaternion.identity) as Treasure;
			reward.spawner = spawner;
			reward.level = level;
			reward.Initialize (4, 6); // rolls high tier quality
		}
		else
		{
			Treasure reward;
			reward = Instantiate (treasure, transform.position,Quaternion.identity) as Treasure;
			reward.spawner = spawner;
			reward.level = level;
			reward.Initialize (0, 3); // rolls low tier quality
		}
		Destroy (gameObject);
		base.MinedOut ();
	}
	

}
