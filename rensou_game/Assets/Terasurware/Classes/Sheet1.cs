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
		public string image1;
		public string hint2;
		public string image2;
		public string hint3;
		public string image3;
		public string hint4;
		public string image4;
		public string hint5;
		public string image5;
		public string answer1;
	}
}