﻿using System.Collections;
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

	public Transition() 
	{
		inputs = new List<string>();
		outputs = new List<string>();
		scene_transition = -1;
		messages = new List<string>();
	}
}

// should probably have some functions in here to return the transition state
public class Node
{
	public string name;
	public string text;
	public List<Transition> transitions;

	public Node()
	{
		name = "";
		text = "";
		transitions = new List<Transition>();
	}

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
					string o = t.outputs[Random.Range(0, t.outputs.Count)];
					Debug.Log(o);
					return o;
				}
			}
		}
		return "";
	}
}

public class TextInputProcessing : MonoBehaviour {

	[SerializeField] string file;
	[SerializeField] string start_node;
	[SerializeField] InputField input;
	[SerializeField] Text display_text;

	[SerializeField] EventManager event_manager;

	Dictionary<string, Node> graph;
	string current_node;

	void Start () 
	{
		graph = XMLParser.xmlParse();
		Debug.Log("START");
		input.onEndEdit.AddListener(
			(value) => {
				TryNodeTransition(value);
				input.text = "";
			}
		);
		current_node = start_node;
		DisplayText(graph[current_node].text);
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