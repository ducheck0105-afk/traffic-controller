using UnityEngine;
using System.Collections;

//Controls light traffic's lights

public class Junction : MonoBehaviour {
	//used to save light traffic's state
	public bool free = false;
	public bool waiting = false;


	public GameObject redLight;
	public GameObject greenLight;
	
	

	void Update ()
	{
		//change colors depending on state
		if (free)
		{
			redLight.SetActive(false);
			greenLight.SetActive(true);
		}
		else if(waiting)
		{
		}
		else
		{
			redLight.SetActive(true);
			greenLight.SetActive(false);
		}
	}

	public bool IsRed => !waiting && !free;
}
