using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour {
	public int playerHealth;
	public Text healthText;
	private float depth;
	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public void PlayerHealth(int health)
	{
		healthText.text = health.ToString();
	}

	public void GameOver(float playerdepth)
	{
		depth = playerdepth;
		Invoke ("EndScreen", 5f);
	}

	void EndScreen()
	{
		Debug.Log ("You dug " + depth + " meters before dying horribly. :(");
		//Game Over screen
	}
}
