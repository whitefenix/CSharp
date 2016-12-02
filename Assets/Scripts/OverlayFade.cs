using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OverlayFade : MonoBehaviour {

	public float speed = 5.0f;
	public float overlayTransparency = 0.6f;

	private Image overlay;

	private bool fadeIn;
	private bool fadeOut;

	private float tmpA;

	// Use this for initialization
	void Start () 
	{
		overlay = GetComponent<Image> ();
	}
	
	void OnEnable ()
	{
		FadeIn ();
	}

	void Update ()
	{
		if (fadeIn) 
		{
			tmpA = Mathf.Min (overlay.color.a + speed * Time.deltaTime, overlayTransparency);

			overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, tmpA);

			if (tmpA == overlayTransparency) 
			{
				FadeInComplete ();
			}
		} 
		else if (fadeOut) 
		{
			tmpA = Mathf.Max (overlay.color.a - speed * Time.deltaTime, 0);

			overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, tmpA);

			if(tmpA == 0) 
			{
				FadeOutComplete ();	
			}
		}
	}

	void FadeInComplete ()
	{
		fadeIn = false;			
	}

	void FadeOutComplete ()
	{
		fadeOut = false;

		gameObject.SetActive (false);
	}

	public void FadeIn ()
	{
		fadeIn = true;
		fadeOut = false;
	}

	public void FadeOut ()
	{
		fadeIn = false;
		fadeOut = true;
	}
}
