using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BlockSpawner : MonoBehaviour {
	public int rowSize;
	public Block blockPrefab;
	public Vector3 spawnPoint;
	public Vector3 airSpawn;
	public int floorLevel;
	public int maxRarity;
	public int minRarity;
	public GameObject MoleRat;




	// Use this for initialization
	void Start () {
		Debug.Log (" miku 120 bpm ");
		Instantiate (MoleRat, (spawnPoint + new Vector3 (0, 40, 0)), Quaternion.identity);


		//block = Resources.Load ("Prefabs/Block") as Block;


		Invoke ("InitializeGame", 0f);
	}

	void InitializeGame()
	{
		spawnPoint = this.transform.position;
		float startLevel = 10;
		maxRarity = 96;
		for (int i = 1; i <= startLevel; i++)
		{
			SpawnRow (minRarity, maxRarity);
		}
		maxRarity = 101;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			SpawnRow(minRarity, maxRarity);
		}
	}

	void SpawnRow(int floor, int rarity)
	{
		Block block;
		for (int i = 1; i <= rowSize; i++)
		{

			block = Instantiate (blockPrefab, (spawnPoint + new Vector3(1,0,0)), Quaternion.identity) as Block;
			block.posY = -(floorLevel)+1;
			block.posX = i-1;
			block.level = -(floorLevel)+1;
			block.rarityFloor = minRarity;
			block.rarityCap = rarity;
			spawnPoint = block.transform.position;
			block.spawner = this;
			block.Initialize(minRarity, maxRarity);
		}
		floorLevel -= 1;
		spawnPoint = this.transform.position + new Vector3 (0, (floorLevel), 0);
	}
	public void CreateBlock(Vector3 location)
	{
		Block block;
		block = Instantiate (blockPrefab, (location), Quaternion.identity) as Block;
		/*block.posY = -(floorLevel)+1;
		block.posX = i-1;*/
		block.level = -(floorLevel)+1;
		block.rarityFloor = minRarity;
		block.rarityCap = maxRarity;
		//spawnPoint = block.transform.position;
		block.spawner = this;
		block.Initialize(minRarity, maxRarity);
	}

	public void CreateAir(int x, int y)
	{
		Block block;
		block = Instantiate (blockPrefab, airSpawn, Quaternion.identity) as Block;
		block.Initialize(0, 1);
		block.spawner = this;

	}

	public void LevelUp(int playerlevel)
	{
		SpawnRow(minRarity, maxRarity);
	}

}
