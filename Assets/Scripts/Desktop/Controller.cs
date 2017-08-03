using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;

public class Controller {

	private static Controller instance;

	private static float joystickDeadzone = 0.5f;
	private static float triggerDeadzone = 0.5f;

	public enum Button { A, B, X, Y, Start, Menu, LB, RB };
	public enum Joystick { Left, Right, DPad };
	public enum Trigger { Left, Right };
	public enum Axis { Horizontal, Vertical };

	public static Controller GetInstance() 
	{
		if (instance == null)
			instance = new Controller ();

		return instance;
	}

	public bool GetButtonDown(Button button)
	{
		string keycode = "";
		switch (button)
		{
		case Button.A:
			return GamePad.GetButtonDown (GamePad.Button.A, GamePad.Index.Any);
		case Button.B:
			return GamePad.GetButtonDown (GamePad.Button.B, GamePad.Index.Any);
		case Button.X:
			return GamePad.GetButtonDown (GamePad.Button.X, GamePad.Index.Any);
		case Button.Y:
			return GamePad.GetButtonDown (GamePad.Button.Y, GamePad.Index.Any);
		case Button.Start:
			return GamePad.GetButtonDown (GamePad.Button.Start, GamePad.Index.Any);
		case Button.Menu:
			return GamePad.GetButtonDown (GamePad.Button.Back, GamePad.Index.Any);
		case Button.LB:
			return GamePad.GetButtonDown (GamePad.Button.LeftShoulder, GamePad.Index.Any);
		case Button.RB:
			return GamePad.GetButtonDown (GamePad.Button.RightShoulder, GamePad.Index.Any);;
		}

		return true;

	}

	public Vector2 GetJoystickAxis(Joystick joy)
	{
		Vector2 output;
		switch (joy)
		{
		case Joystick.Left:
			output = GamePad.GetAxis (GamePad.Axis.LeftStick, GamePad.Index.Any, true);
			if (Mathf.Abs (output.x) < joystickDeadzone && Mathf.Abs (output.y) < joystickDeadzone)
				return Vector2.zero;
			return output;
		case Joystick.Right:
			output = GamePad.GetAxis(GamePad.Axis.RightStick, GamePad.Index.Any, true);
			if (Mathf.Abs (output.x) < joystickDeadzone && Mathf.Abs (output.y) < joystickDeadzone)
				return Vector2.zero;
			return output;
		}
		return Vector2.zero;
	}

	/**
	 * A value between 0 - 1 with 1 beind held down completely.
	 */
	public float GetTriggerValue(Trigger trigger)
	{
		float output;
		switch (trigger)
		{
		case Trigger.Left:
			output = GamePad.GetTrigger (GamePad.Trigger.LeftTrigger, GamePad.Index.Any, true);
			if (output < triggerDeadzone)
				return 0f;
			return output;
		case Trigger.Right:
			output = GamePad.GetTrigger(GamePad.Trigger.RightTrigger, GamePad.Index.Any, true);
			if (output < triggerDeadzone)
				return 0f;
			return output;
		}
		return 0f;
	}

}