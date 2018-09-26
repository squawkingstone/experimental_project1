using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour {

	[SerializeField] float time;
	Image image;

	void Awake() { image = GetComponent<Image>(); }

	void Start() { StartCoroutine(FadeInRoutine()); }

	private IEnumerator FadeInRoutine()
	{
		for (float t = 0; t < time; t += Time.deltaTime)
		{
			image.color = new Color(1, 1, 1, 1-t/time);
			yield return null;
		}
		this.gameObject.SetActive(false);
	}

}
