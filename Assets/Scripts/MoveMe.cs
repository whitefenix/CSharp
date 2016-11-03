using UnityEngine;
using System.Collections;

public class MoveMe : MonoBehaviour {

    [SerializeField]
    float speed = 0.000001f;
	
	void FixedUpdate () {
	    if(Input.GetKey("w"))
        {
            Vector3 position = this.transform.position;
            position.y += speed;
            this.transform.position = position;
        }
        if (Input.GetKey("a"))
        {
            Vector3 position = this.transform.position;
            position.x -= speed;
            this.transform.position = position;
        }
        if (Input.GetKey("s"))
        {
            Vector3 position = this.transform.position;
            position.y -= speed;
            this.transform.position = position;
        }
        if (Input.GetKey("d"))
        {
            Vector3 position = this.transform.position;
            position.x += speed;
            this.transform.position = position;
        }

    }
}
