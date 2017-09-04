using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Sheet1 : ScriptableObject
{	
	public List<Param> param = new List<Param> ();

	[System.SerializableAttribute]
	public class Param
	{
		
		public int ID;
		public string hint1;
		public string hint2;
		public string hint3;
		public string hint4;
		public string hint5;
		public string answer1;
		public string answer2;
	}
}