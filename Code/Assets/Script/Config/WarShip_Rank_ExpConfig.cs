using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security;
/// <summary>
/// Warship config.
/// </summary>
public class WarShip_Rank_ExpConfig : ConfigBase
{
	public class WarShip_Rank_ExpConfigObject
	{
		
		public int WarShip_Rank
		{
			get{ return _WarShip_Rank; }
			set{ _WarShip_Rank = value;}
		}	
		public int Exp
		{
			get{ return _Exp; }
			set{ _Exp = value;}
		}	
		
				
		public static WarShip_Rank_ExpConfigObject Load (SecurityElement element)
		{
			WarShip_Rank_ExpConfigObject WarShipRankExpConfigObject = new WarShip_Rank_ExpConfigObject();
		   
			WarShipRankExpConfigObject.WarShip_Rank = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("WarShip_Rank"),""),-1);
			WarShipRankExpConfigObject.Exp = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Exp"),""),-1);
			
			return WarShipRankExpConfigObject;
		}	
		
		protected int _WarShip_Rank;
		protected int _Exp;
		

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
				WarShip_Rank_ExpConfigObject WarShipRankExpConfigObject = WarShip_Rank_ExpConfigObject.Load(childrenElement);
				
				if (!_warshipevolutionObjectDict.ContainsKey(WarShipRankExpConfigObject.WarShip_Rank))
					_warshipevolutionObjectDict[WarShipRankExpConfigObject.WarShip_Rank] = WarShipRankExpConfigObject;
			}
		}

		return true;
	}
	
	public bool GetWarshipElement(int WarShip_Rank, out WarShip_Rank_ExpConfigObject element)
	{
		element = null;
		
		if ( !_warshipevolutionObjectDict.TryGetValue(WarShip_Rank, out element) )
			return false;
		
		return true;
	}
	


	public WarShip_Rank_ExpConfigObject GetEXPByRank(int iSRank)
	{
		foreach(WarShip_Rank_ExpConfigObject obj in _warshipevolutionObjectDict.Values)
		{
			if(obj.WarShip_Rank == iSRank)
			{
				return obj;				
			}
		}
		return null;
	}
	
	protected Dictionary<int, WarShip_Rank_ExpConfigObject> _warshipevolutionObjectDict = new Dictionary<int, WarShip_Rank_ExpConfigObject>();
	

}


