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
	public string scene_transition;  // the scene to transition to if this transition triggers a scene 
								  // change, defaults to -1 (no change)
	public List<string> messages; // a list of messages to get sent on transition. This is supposed to
								  // basically just give a way to call functions and trigger events in

	public Transition() 
	{
		inputs = new List<string>();
		outputs = new List<string>();
		scene_transition = "";
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
			if (t.inputs.Count == 0)
			{
				return TriggerTransition(t, event_manager);
			}
			foreach (string i in t.inputs)
			{
				if (i.ToLower() == input.ToLower())
				{
					return TriggerTransition(t, event_manager);
				}
			}
		}
		return "";
	}

	private string TriggerTransition(Transition t, EventManager event_manager)
	{
		if (t.scene_transition != "") { SceneManager.LoadScene(t.scene_transition); }
		foreach (string m in t.messages) { event_manager.Invoke(m); }
		return t.outputs[Random.Range(0, t.outputs.Count)];
	}
}

public class TextInputProcessing : MonoBehaviour {

	[SerializeField] string file;
	[SerializeField] string start_node;
	[SerializeField] InputField input;
	[SerializeField] Text display_text;
	[SerializeField] float display_speed;

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
				input.ActivateInputField();
			}
		);
		current_node = start_node;
		DisplayText(graph[current_node].text);
	}
	
	// Display some bit of text
	void DisplayText(string text)
	{
		StopAllCoroutines();
		StartCoroutine(TypewriterDisplay(text));
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
		else
		{
			DisplayText("I'm sorry, I don't understand.\n\n" + graph[current_node].text);
		}
	}

	// Interrupt the parsing, and switch directly to a given node
	public void Interrupt(string node)
	{
		DisplayText("");
		current_node = node;
		DisplayText(graph[current_node].text);
	}

	private IEnumerator TypewriterDisplay(string text)
	{
		display_text.text = "";
		yield return new WaitForEndOfFrame();
		for (int i = 0; i < text.Length; i++)
		{
			display_text.text += text[i];
			if (text[i] == ',' || text[i] == '.')
			{
				yield return new WaitForSeconds(5f/display_speed);
			}
			else
			{
				yield return new WaitForSeconds(1f/display_speed);
			}
		}
	}

}
