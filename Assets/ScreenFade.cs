using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFade : MonoBehaviour {

	public Image FadeImg;
	public float fadeSpeed = 1.5f;
	public bool sceneStarting = true;

	void Awake()
	{
		FadeImg.rectTransform.localScale = new Vector2(Screen.width, Screen.height);
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void FadeToClear(float _fadeSpeed)
	{
		// Lerp the colour of the image between itself and transparent.
		FadeImg.color = Color.Lerp(FadeImg.color, Color.clear, _fadeSpeed * Time.deltaTime);
	}


	void FadeToBlack(float _fadeSpeed)
	{
		// Lerp the colour of the image between itself and black.
		FadeImg.color = Color.Lerp(FadeImg.color, Color.black, _fadeSpeed * Time.deltaTime);
	}


	public void FadeInScene(float _fadeTime)
	{
		// Fade the texture to clear.
		FadeToClear(_fadeTime);

		// If the texture is almost clear...
		if (FadeImg.color.a <= 0.05f)
		{
			// ... set the colour to clear and disable the RawImage.
			FadeImg.color = Color.clear;
			FadeImg.enabled = false;

			// The scene is no longer starting.
			sceneStarting = false;
		}
	}

	public void FadeOutScene(float _fadeTime){
		FadeImg.enabled = true;

		FadeToBlack (_fadeTime);

		if (FadeImg.color.a >= 0.95f)
		{
			// ... set the colour to clear and disable the RawImage.
			FadeImg.color = Color.black;
			//FadeImg.enabled = false;

			// The scene is no longer starting.
			//sceneStarting = false;
		}


	}


}
