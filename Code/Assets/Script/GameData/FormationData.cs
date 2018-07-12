using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//lsj
namespace GameData
{
	public class FormationData 
	{
		public static readonly int MaxCellRow = 3;
		public static readonly int MaxCellCol = 3;
		
		public int formationTpyeID;
		public int formationNameID;
		public int currFormationDegree;
		
		public int aiTacticsId;//服务器用的Ai策略ID
		public int roleFormationId;//服务器专用的ID
		public bool isUsed;//当前阵型是否正在使用
		
		// public Dictionary<int,SingleFormationDegree> _dictSingleFormationDegree= new Dictionary<int,SingleFormationDegree>();
		// public Dictionary<int,SingleLocation> _dictSingleLocation = new Dictionary<int, SingleLocation>();

		// //同一类别阵型不同等级下的数据
		// public class SingleFormationDegree
		// {
		// 	public int _formationDegree;
		// 	public int _formationDescID;
		// 	public int _maxFormationTypeLocationID;
		// 	public int _formationStudyDegree;
		// 	public int _formationUpgradeConsume;
		// 	public int _tonLimit;
		// 	public int _shipLimit;
		// 	public int _bufferID;
		// }
		
		//同一类别阵型不同位置ＩＤ对应的位置
		public class SingleLocation
		{
			public int logicIndex;
			public int _formationLocationID;
			public int _locationX;
			public int _locationY;
			public bool canUse;
			
			public bool isArrayEye = false;
			public BuffData buffData = null; 
		}
		
		public int formationDegree;
		public int formationDescID;
		public int maxFormationTypeLocationID;
		public int formationStudyDegree;
		public int formationUpgradeConsume;
		public int tonLimit;
		public int shipLimit = -1;
		public int lastLvlShipLimit = -1;
		public int bufferID;
		public string icon;
		
		public List<SingleLocation> formationLocationList = new List<SingleLocation>();
		
		//locationID 位置ID-----   shipID当前位置的舰船的ID　　如果没有则舰船ID为－１
		public Dictionary<int, long> _dictFormationLocationShip = new Dictionary<int, long>();
		
		public void FillDataFromConfig()
		{
		
		}
		
		public string GetDisplayName(){
			string format = Globals.Instance.MDataTableManager.GetWordText(20800004);
			string fmtName = GUIFontColor.Purple219039252 + Globals.Instance.MDataTableManager.GetWordText(formationNameID);
			return  string.Format(format, fmtName, currFormationDegree);
		}	
	}
}

