using UnityEngine;
using System.Collections;

public class Animation_mole : MonoBehaviour {
	public bool downMove;
	public bool rightMove;
	public bool idleMove;
	public Animator anim;
	public Animator animator;
	// Use this for initialization

	void Start () 
	{
		anim = GetComponent<Animator> ();
		anim.Play ("right");
	}


	//IEnumerator runinganimation
	// Update is called once per frame
	void Update () {
		anim.SetBool ("down", downMove);
		anim.SetBool ("idle", rightMove);
		anim.SetBool ("right", idleMove);


		anim.Play ("right");
		anim.bo
	}}
}
