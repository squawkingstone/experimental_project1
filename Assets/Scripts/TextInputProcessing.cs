using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Transition
{
	public List<string> inputs;   // the strings that can be entered to trigger this transition
	public List<string> outputs;  // the nodes the transition can go to
	public int scene_transition;  // the scene to transition to if this transition triggers a scene 
								  // change, defaults to -1 (no change)
	public List<string> messages; // a list of messages to get sent on transition. This is supposed to
								  // basically just give a way to call functions and trigger events in
}

// should probably have some functions in here to return the transition state
public class Node
{
	public string name;
	public string text;
	public Transition[] transitions;

	public string GetNextNode(string input, EventManager event_manager)
	{
		foreach (Transition t in transitions)
		{
			foreach (string i in t.inputs)
			{
				if (i.ToLower() == input.ToLower())
				{
					// select an output, trigger any events, and do any scene transitions
					if (t.scene_transition != -1) { SceneManager.LoadScene(t.scene_transition); }
					foreach (string m in t.messages) { event_manager.Invoke(m); }
					return t.outputs[Random.Range(0, t.outputs.Count)];
				}
			}
		}
		return "";
	}
}

public class TextInputProcessing : MonoBehaviour {

	[SerializeField] string file;
	[SerializeField] InputField input;
	[SerializeField] Text display_text;

	EventManager event_manager;

	Dictionary<string, Node> graph;
	string current_node;

	void Awake () 
	{ 
		event_manager = GameObject.FindGameObjectWithTag("EventManager").GetComponent<EventManager>(); 
	}

	void Start () 
	{
		// Open the XML file, load every node into the dictionary, and set 
		// current_node to the starting node in the graph
		graph = new Dictionary<string, Node>();
		input.onEndEdit.AddListener(
			(value) => {
				TryNodeTransition(value);
			}
		);
	}
	
	// Display some bit of text
	void DisplayText(string text)
	{
		display_text.text = text;
	}

	// trys to use the input to transition to a new text node
	void TryNodeTransition(string input)
	{
		string n = graph[current_node].GetNextNode(input, event_manager);
		if (n != "")
		{
			current_node = n;
			DisplayText(graph[current_node].text);
		}
	}

	// Interrupt the parsing, and switch directly to a given node
	void Interrupt(string node)
	{
		DisplayText("");
		current_node = node;
		DisplayText(graph[current_node].text);
	}

}
