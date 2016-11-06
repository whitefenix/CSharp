using UnityEngine;
using System.Collections;

/**
 * http://wiki.unity3d.com/index.php?title=CameraFacingBillboard 
 */
//[ExecuteInEditMode]
public class CameraFacingBillboard : MonoBehaviour
{
	private Camera m_Camera;

	void Start()
	{
		m_Camera = GameObject.FindWithTag ("MainCamera").GetComponent<Camera>();
	}

	void Update()
	{
		transform.LookAt(transform.position + m_Camera.transform.rotation * Vector3.forward,
			m_Camera.transform.rotation * Vector3.up);
	}
}