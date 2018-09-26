using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAnimate : MonoBehaviour {

	[SerializeField] Vector3 start_pos;
	[SerializeField] Vector2 start_scale;
	[SerializeField] Vector3 end_pos;
	[SerializeField] Vector2 end_scale;
	public float time;

	public void AnimateForward() { StartCoroutine(AnimateForwardRoutine()); }
	public void AnimateBackward() { StartCoroutine(AnimateBackwardRoutine()); }

	private IEnumerator AnimateForwardRoutine()
	{
		transform.position = start_pos;
		transform.localScale = start_scale;
		for (float t = 0f; t < time; t += Time.deltaTime)
		{
			transform.position = Vector3.Lerp(start_pos, end_pos, t / time);
			transform.localScale = Vector2.Lerp(start_scale, end_scale, t / time);
			yield return null;
		}
	}

	private IEnumerator AnimateBackwardRoutine()
	{
		transform.position = start_pos;
		transform.localScale = start_scale;
		for (float t = 0f; t < time; t += Time.deltaTime)
		{
			transform.position = Vector3.Lerp(end_pos, start_pos, t / time);
			transform.localScale = Vector2.Lerp(end_scale, start_scale, t / time);
			yield return null;
		}
	}

}
