﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstanceColour : MonoBehaviour {

	public Color color;

	void Awake () {
		GetComponent<Renderer> ().material.SetColor ("_Color", color);
	}
}
