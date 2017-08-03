﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstanceColour : MonoBehaviour {

	public Color color;

	void Start () {
		if (color != null) {
			GetComponent<Renderer> ().material.SetColor ("_Color", color);
		}
	}
}
