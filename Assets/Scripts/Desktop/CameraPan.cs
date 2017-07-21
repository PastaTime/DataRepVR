using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPan : MonoBehaviour {

	public enum Position { A, B };

	public Transform positionA;
	public Transform positionB;

	public float panTime = 2f;

	private bool panning = false;
	private float counter = 0f;

	private Position currentPosition;

	void Start () {
		// Set Camera to Position A
		Pan (positionA, positionB, 0);
		currentPosition = Position.A;
	}

	// Update is called once per frame
	void Update () {
		if (!panning) {
			return;
		}

		if (counter > panTime) {
			panning = false;
			counter = 0f;
			return;
		}
			
		counter += Time.deltaTime;

		switch (currentPosition) {
		case Position.A:
			Pan (positionB, positionA, Mathf.Clamp01 (counter / panTime));
			break;
		case Position.B:
			Pan (positionA, positionB, Mathf.Clamp01 (counter / panTime));
			break;
		}
		
	}

	public void MoveTo(Position pos) {
		if (currentPosition == pos)
			return;
		currentPosition = pos;
		panning = true;
		if (counter != 0f)
			counter = panTime - counter;
		else
			counter = 0f;
	}

	private void Pan(Transform start, Transform end, float fraction) {
		Vector3 position = Vector3.Lerp (start.position, end.position, fraction);
		Vector3 eulers = Vector3.Lerp (start.eulerAngles, end.eulerAngles, fraction);
		Vector3 scale = Vector3.Lerp (start.localScale, end.localScale, fraction);

		transform.position = position;
		transform.eulerAngles = eulers;
		transform.localScale = scale;
	}
}
