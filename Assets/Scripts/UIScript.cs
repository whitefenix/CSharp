using UnityEngine;
using System.Collections;

public class UIScript : MonoBehaviour {

    public Texture ViolinExample, ViolinExample2;
    [SerializeField]
    string mainEquip, secondEquip;


    void Update()
    {
        InputHandler();
    }

    void InputHandler()
    {
        if (Input.GetKeyDown(KeyCode.G) == true)
        {
            Debug.Log("G");
        }
        else if (Input.GetKeyDown(KeyCode.H) == true)
        {
            Debug.Log("H");
        }
    }

    void OnGUI()
    {
        GUI.Box(new Rect(100, Screen.height - 200, 200, 200), ViolinExample, GUIStyle.none);
        GUI.Box(new Rect(Screen.width - 300, Screen.height - 200, 200, 200), ViolinExample, GUIStyle.none);

    }
}
