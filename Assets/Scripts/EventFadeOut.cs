using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventFadeOut : MonoBehaviour {

	[SerializeField] GameObject fade_box;
	[SerializeField] EventManager event_manager;
	[SerializeField] float time;
	Image image;

	void Awake() 
	{ 	
		image = fade_box.GetComponent<Image>();
		fade_box.SetActive(false); 
	}

	void Start() 
	{
		event_manager.AddListener("FadeOut", () => { StartCoroutine(FadeOutRoutine()); } );
	}

	private IEnumerator FadeOutRoutine()
	{
		fade_box.SetActive(true);
		for (float t = 0; t < time; t += Time.deltaTime)
		{
			image.color = new Color(0, 0, 0, t / time);
			yield return null;
		}
	}
}
