using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleTransitionMovement : MonoBehaviour {

	[SerializeField] Vector3 start_pos;
	[SerializeField] Vector2 start_scale;
	[SerializeField] Vector3 end_pos;
	[SerializeField] Vector2 end_scale;
	[SerializeField] float time;

	[ContextMenu("Anim Start")]
	void AnimStart() { StartCoroutine(Animate()); }

	private IEnumerator Animate()
	{
		transform.position = start_pos;
		transform.localScale = start_scale;
		for (float t = 0f; t < time; t += Time.deltaTime)
		{
			transform.position = Vector2.Lerp(start_pos, end_pos, t / time);
			transform.localScale = Vector2.Lerp(start_scale, end_scale, t / time);
			yield return null;
		}
	}

}
