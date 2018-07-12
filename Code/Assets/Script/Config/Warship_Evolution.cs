using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security;
/// <summary>
/// Warship config.
/// </summary>
public class Warship_Evolution : ConfigBase
{
	public class WarshipEvolutionObject
	{
		
		public int Id
		{
			get{ return _Id; }
			set{ _Id = value;}
		}	
		public int WarshipID
		{
			get{ return _warshipID; }
			set{ _warshipID = value;}
		}	
		public int Evolution_Level
		{
			get{ return _Evolution_Level; }
			set{ _Evolution_Level = value;}
		}
				
		public int Init_Art_Value
		{
			get{ return _Init_Art_Value; }
			set{ _Init_Art_Value = value;}
		}
		
		public int Init_Art_Profile
		{
			get{ return _Init_Art_Profile; }
			set{ _Init_Art_Profile = value;}
		}
			public int Change_Art_Value
		{
			get{ return _Change_Art_Value; }
			set{ _Change_Art_Value = value;}
		}
		
		public int Change_Art_Profile
		{
			get{ return _Change_Art_Profile; }
			set{ _Change_Art_Profile = value;}
		}
		public int Art_Max_Level
		{
			get{ return _Art_Max_Level; }
			set{ _Art_Max_Level = value;}
		}
				
		public int New_Skill
		{
			get{ return _New_Skill; }
			set{ _New_Skill = value;}
		}
		
		public int Need_Level
		{
			get{ return _Need_Level; }
			set{ _Need_Level = value;}
		}
		public int Need_Item1
		{
			get{ return _Need_Item1;}
			set{_Need_Item1 = value;}
		}
		public int Need_Num1
		{
			get{ return _Need_Num1; }
			set{ _Need_Num1 = value;}
		}
		public int Need_Item2
		{
			get{ return _Need_Item2; }
			set{ _Need_Item2 = value;}
		}
		public int Need_Num2
		{
			get{ return _Need_Num2; }
			set{ _Need_Num2 = value;}
		}		
		public int Need_Warship_ID1
		{
			get{ return _Need_Warship_ID1; }
			set{ _Need_Warship_ID1 = value;}
		}
		
		public int Need_Warship_Evolution1
		{
			get{ return _Need_Warship_Evolution1; }
			set{ _Need_Warship_Evolution1 = value;}	
		}
		public int Need_Warship_Num1
		{
			get{return _Need_Warship_Num1;}
			set{_Need_Warship_Num1 = value;}
		}
		
		public int Need_Warship_ID2
		{
			get{return _Need_Warship_ID2;}
			set{_Need_Warship_ID2 = value;}
		}
		
		public int Need_Warship_Evolution2
		{
			get{ return _Need_Warship_Evolution2; }
			set{ _Need_Warship_Evolution2 = value;}
		}
		public int Need_Warship_Num2
		{
			get{ return _Need_Warship_Num2; }
			set{ _Need_Warship_Num2 = value;}
		}
		
				
		public static WarshipEvolutionObject Load (SecurityElement element)
		{
			WarshipEvolutionObject warshipevolutionObject = new WarshipEvolutionObject();
		    warshipevolutionObject.Id = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Id"),""),-1);
			warshipevolutionObject.WarshipID = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Warship_Id"),""),-1);
			warshipevolutionObject.Evolution_Level = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Evolution_Level"),""),-1);
			warshipevolutionObject.Init_Art_Value = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Init_Art_Value"),""),-1);
			warshipevolutionObject.Init_Art_Profile = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Init_Art_Profile"),""),-1);
			warshipevolutionObject.Change_Art_Value= StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Change_Art_Value"),""),-1);
			warshipevolutionObject.Change_Art_Profile= StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Change_Art_Profile"),""),-1);
			warshipevolutionObject.Art_Max_Level = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Art_Max_Level"),""),-1);
			warshipevolutionObject.New_Skill = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("New_Skill"),""),-1);
			warshipevolutionObject.Need_Level = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Need_Level"),""),-1);
			warshipevolutionObject.Need_Item1 = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Need_Item1"),""),-1);
			warshipevolutionObject.Need_Num1 = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Need_Num1"),""),-1);
			warshipevolutionObject.Need_Item2 = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Need_Item2"),""),-1);
			warshipevolutionObject.Need_Num2 = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Need_Num2"),""),-1);	
			warshipevolutionObject.Need_Warship_ID1 = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Need_Warship_ID1"),""),-1);
			warshipevolutionObject.Need_Warship_Evolution1 = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Need_Warship_Evolution1"),""),-1);
			warshipevolutionObject.Need_Warship_Num1 = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Need_Warship_Num1"),""),-1);
			warshipevolutionObject.Need_Warship_ID2 = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Need_Warship_ID2"),""),-1);
			warshipevolutionObject.Need_Warship_Evolution2 = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Need_Warship_Evolution2"),""),-1);
			warshipevolutionObject.Need_Warship_Num2 = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Need_Warship_Num2"),""),-1);	

			
			
			return warshipevolutionObject;
		}	
		
		protected int _Id;
		protected int _warshipID;
		protected int _Evolution_Level;
		protected int _Init_Art_Value;
		protected int _Init_Art_Profile;
		protected int _Change_Art_Value;		
		protected int _Change_Art_Profile;
		protected int _Art_Max_Level;
		protected int _New_Skill;
		protected int _Need_Level;
		protected int _Need_Item1;
		protected int _Need_Num1;
		protected int _Need_Item2;
		protected int _Need_Num2;
		protected int _Need_Warship_ID1;
		protected int _Need_Warship_Evolution1;
		protected int _Need_Warship_Num1;
		protected int _Need_Warship_ID2;
		protected int _Need_Warship_Evolution2;
		protected int _Need_Warship_Num2;

	}
	
	public static readonly int 	MaxFillSpeedAttr = 3;
	
	/// <summary>
	/// tzz added for fill speed
	/// Gets the actual fill speed.
	/// </summary>
	/// <returns>
	/// The actual fill speed.
	/// </returns>
	/// <param name='xmlFillSpeed'>
	/// Xml fill speed.
	/// </param>
	public static int GetActualFillSpeed(int xmlFillSpeed){
		int tMaxSpeed = 1;	
			
		for(int i = 2;i <= MaxFillSpeedAttr;i++){
			tMaxSpeed *= i;
		}
		
		return tMaxSpeed / xmlFillSpeed;
	}
	
	public override bool Load (SecurityElement element)
	{
		if(element.Tag != "Items")
			return false;
		
		if(element.Children !=null)
		{
			foreach(SecurityElement childrenElement in element.Children)
			{
				WarshipEvolutionObject warshipevolutionObject = WarshipEvolutionObject.Load(childrenElement);
				
				if (!_warshipevolutionObjectDict.ContainsKey(warshipevolutionObject.Id))
					_warshipevolutionObjectDict[warshipevolutionObject.Id] = warshipevolutionObject;
			}
		}

		return true;
	}
	
	public bool GetWarshipElement(int warshipID, out WarshipEvolutionObject element)
	{
		element = null;
		
		if ( !_warshipevolutionObjectDict.TryGetValue(warshipID, out element) )
			return false;
		
		return true;
	}
	

	
	public string GetgirlobjectByID(int Id)
	{
		return _girlStringDict[Id];
	}
	
//	public WarshipEvolutionObject GetWarshipEvolutionObjectByID(int iSWarshipID)
//	{
//		foreach(WarshipEvolutionObject obj in _warshipevolutionObjectDict.Values)
//		{
//			if(obj.WarshipID == iSWarshipID )
//			{
//				return obj;
//			}
//		}
//		return null;
//	}
	public WarshipEvolutionObject GetWarshipEvolutionObjectByIDAndEvolution(int iSWarshipID,int isEvolution_Level)
	{
		foreach(WarshipEvolutionObject obj in _warshipevolutionObjectDict.Values)
		{
			if(obj.WarshipID == iSWarshipID)
			{
				if(obj.Evolution_Level == isEvolution_Level)
				{
					return obj;
				}
			}
		}
		return null;
	}
	List<WarshipEvolutionObject> lis = new List<WarshipEvolutionObject>();
	public List<WarshipEvolutionObject> GetListById(int Id)
	{
		lis.Clear();
		foreach(WarshipEvolutionObject obj in _warshipevolutionObjectDict.Values)
		{
			if(obj.WarshipID == Id&&obj.New_Skill>0)
			{
				lis.Add(obj);
			}
		}
		return lis;
	}
	
	protected Dictionary<int, WarshipEvolutionObject> _warshipevolutionObjectDict = new Dictionary<int, WarshipEvolutionObject>();
	
	public static Dictionary<int, string> _girlStringDict = new Dictionary<int, string>();
}

