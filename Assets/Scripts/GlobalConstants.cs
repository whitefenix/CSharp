using UnityEngine;

public class GlobalConstants {

	#if UNITY_STANDALONE_WIN

	public const KeyCode XBOX_BTN_A = KeyCode.Joystick1Button0;
	public const KeyCode XBOX_BTN_B = KeyCode.Joystick1Button1;
	public const KeyCode XBOX_BTN_X = KeyCode.Joystick1Button2;
	public const KeyCode XBOX_BTN_Y = KeyCode.Joystick1Button3;

	public const KeyCode XBOX_BTN_LB = KeyCode.Joystick1Button4;
	public const KeyCode XBOX_BTN_RB = KeyCode.Joystick1Button5;

	public const KeyCode XBOX_BTN_START = KeyCode.Joystick1Button7;
	public const KeyCode XBOX_BTN_BACK = KeyCode.Joystick1Button6;

	public const string XBOX_AXIS_LT = "LTriggerWin";
	public const string XBOX_AXIS_RT = "RTriggerWin";

	#elif UNITY_STANDALONE_OSX

	public const KeyCode XBOX_BTN_A = KeyCode.Joystick1Button16;
	public const KeyCode XBOX_BTN_B = KeyCode.Joystick1Button17;
	public const KeyCode XBOX_BTN_X = KeyCode.Joystick1Button18;
	public const KeyCode XBOX_BTN_Y = KeyCode.Joystick1Button19;

	public const KeyCode XBOX_BTN_LB = KeyCode.Joystick1Button13;
	public const KeyCode XBOX_BTN_RB = KeyCode.Joystick1Button14;

	public const KeyCode XBOX_BTN_START = KeyCode.Joystick1Button9;
	public const KeyCode XBOX_BTN_BACK = KeyCode.Joystick1Button10;

	public const string XBOX_AXIS_LT = "LTriggerMac";
	public const string XBOX_AXIS_RT = "RTriggerMac";

	#elif UNITY_XBOX360

	//TODO

	#endif
}
