using UnityEngine;
using System.Collections;

public class animationControllernew : MonoBehaviour {
	public Animator anim;
	float animValue=0;
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		InvokeRepeating ("increaseForAnimation", 0, 2);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.A)) {
			anim.SetFloat ("warmup1",0.2f);	
			anim.SetFloat ("warmup2",0.0f);	
			anim.SetFloat ("warmup3",0.0f);		
		}
		if (Input.GetKeyDown (KeyCode.B)) {
			anim.SetFloat ("warmup2",0.2f);	
			anim.SetFloat ("warmup1",0.0f);	
			anim.SetFloat ("warmup3",0.0f);		
		}if (Input.GetKeyDown (KeyCode.C)) {
			anim.SetFloat ("warmup3",0.2f);	
			anim.SetFloat ("warmup2",0.0f);	
			anim.SetFloat ("warmup1",0.0f);		
		}
//			anim.SetFloat("warmup1",0.2f,0.2f,Time.deltaTime);
	}
	void increaseForAnimation()
	{
//		animValue += 0.1f;
//		anim.SetFloat("warmup1",animValue,animValue,Time.deltaTime);
//		print ("method"+animValue);


	}

}
