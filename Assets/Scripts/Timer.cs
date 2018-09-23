using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TimedEvent
{
	public float limit;
	public string node;
}
public class Timer : MonoBehaviour {
	[SerializeField] List<TimedEvent> events;
	[SerializeField] TextInputProcessing textManager;
	int index;
	float time;

	// Use this for initialization
	void Start () {
		index = 0;
		time = events[index].limit;
	}
	
	// Update is called once per frame
	void Update () {
		time -= Time.deltaTime;
		if(time <= 0f){
			if(index == events.Count){
				Destroy(this.gameObject);
				time = Mathf.Infinity;
			}else{
				textManager.Interrupt(events[index].node);
				index = (index < events.Count - 1) ? index + 1 : index;
				time = events[index].limit;
			}
		}
	}
}
