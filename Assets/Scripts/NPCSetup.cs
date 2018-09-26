using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSetup : MonoBehaviour {

	[SerializeField] Sprite[] sprites;
	[SerializeField] EventManager event_manager;
	[SerializeField] string entrance_event;
	[SerializeField] string exit_event;

	[SerializeField] bool start_in_scene;

	SpriteRenderer sprite_renderer;
	NPCAnimate animate;

	void Awake()
	{
		sprite_renderer = GetComponent<SpriteRenderer>();
		animate = GetComponent<NPCAnimate>();
		sprite_renderer.sprite = sprites[Random.Range(0, sprites.Length)];
		sprite_renderer.enabled = start_in_scene;	
	}

	void Start()
	{
		event_manager.AddListener(entrance_event, () => { Enter(); });
		event_manager.AddListener(exit_event, () => { Exit(); });
	}

	[ContextMenu("Enter")]
	void Enter()
	{
		sprite_renderer.enabled = true;
		sprite_renderer.sprite = sprites[Random.Range(0, sprites.Length)];
		animate.AnimateForward();
	}

	[ContextMenu("Exit")]
	void Exit()
	{
		animate.AnimateBackward();
		StartCoroutine(DisableSprite());
	}

	private IEnumerator DisableSprite()
	{
		for (float t = 0f; t < animate.time; t += Time.deltaTime)
		{
			yield return null;
		}
		sprite_renderer.enabled = false;
	}

}
