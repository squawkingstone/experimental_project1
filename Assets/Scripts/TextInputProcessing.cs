using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class TextTransition
{
	public string input;
	public int node;
}

/*
	for this, each node in this "graph" correlates to some bit of text input. For
	now it's matching the text exactly, but this could be made more robust if we
	need it. 

	The TextTest scene has a demo of how it works and hopefully that demo's pretty
	clear, it's just the text that displays, and then that text being entered will
	transition between other numbered nodes in the array. Again, we could make 
	that interface nicer if we need to (which we probably will)

	also randomized stuff, so like you can transition to random nodes for the same
	input or to random nodes given arbitrary input
 */
[System.Serializable]
public class TextNode
{
	public string text;
	public TextTransition[] transitions;
	private Dictionary<string, int> transition_dict = null;

	// should update this to be safer
	public void LoadTransitions()
	{
		transition_dict = new Dictionary<string, int>();
		foreach (TextTransition t in transitions)
		{
			transition_dict.Add(t.input, t.node);
		}
	}

	public int GetTransition(string input)
	{
		if (!transition_dict.ContainsKey(input)) { return -1; }
		return transition_dict[input];
	}
}

/* maybe include some way of interrupting the behavior... I think I'll need
	some way of interrupting the sort of "flow" of the text processing, that
	way we can do real time stuff. 
 */
public class TextInputProcessing : MonoBehaviour {

	[SerializeField] TextNode[] text_graph;
	[SerializeField] InputField input;
	[SerializeField] Text display_text;

	int index = 0;

	void Start () 
	{
		// process each node
		foreach (TextNode t in text_graph)
		{
			t.LoadTransitions();
		}
		// set up listener on command enter
		input.onEndEdit.AddListener(
			(value) => 
			{
				int new_index = text_graph[index].GetTransition(value);
				TryNodeTransition(new_index);
				input.text = "";
			}
		);
		DisplayText(text_graph[index].text);
	}
	
	// Display some bit of text
	void DisplayText(string text)
	{
		display_text.text = text;
	}

	// trys to use the input to transition to a new text node
	void TryNodeTransition(int new_index)
	{
		if (new_index == -1)
		{
			DisplayText("I'm not sure what you mean...\n\n" + text_graph[index].text);
		}
		else
		{
			index = new_index;
			DisplayText(text_graph[index].text);
		}
	}

	void Interrupt(int i)
	{

	}

}
