using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

public class XMLParser {

	public static Dictionary<string, Node> xmlParse(){
		string context = System.IO.File.ReadAllText("Assets\\XML_Scripts\\baseline.xml");
		XmlDocument document = new XmlDocument();
		document.LoadXml(context);
		Dictionary<string,Node> tree = new Dictionary<string, Node>();
		XmlNodeList nodes = document.GetElementsByTagName("node");
		foreach(XmlNode node in nodes){
			Node n = new Node();
			XmlNodeList content = node.ChildNodes;
			foreach(XmlNode child in content){
				if(child.Name == "name"){
					n.name = child.InnerText;
				}
				if(child.Name == "text"){
					n.text = child.InnerText;
				}
				if(child.Name == "transition"){
					XmlNodeList transitions = child.ChildNodes;
					Transition trans = new Transition();
					foreach(XmlNode t in transitions){
						if(t.Name == "input"){
							trans.inputs.Add(t.InnerText);
						}
						if(t.Name == "output"){
							trans.outputs.Add(t.InnerText);
						}
						if(t.Name == "scene_trans"){
							trans.scene_transition = t.InnerText;
						}
						if(t.Name == "message"){
							trans.messages.Add(t.InnerText);
						}
					}
					n.transitions.Add(trans);
				}
			}
			tree.Add(n.name, n);
		}
		return tree;
	}
}
