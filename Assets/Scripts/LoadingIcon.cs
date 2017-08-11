using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingIcon : MonoBehaviour
{

	public float speed = -650f;

	// Use this for initialization
	void Start ()
	{
		gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.Rotate(new Vector3(0, 0, speed * Time.deltaTime));
	}
}
