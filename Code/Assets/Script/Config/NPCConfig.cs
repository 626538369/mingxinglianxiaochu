using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security;

public class NPCConfig : ConfigBase
{
	public class NPCObject
	{		
		
		public int NPCID
        {
            get { return _mNPCID; }
            set { _mNPCID = value; }
        }
		
		
		public string NPCName
        {
            get { return mNPCName; }
            set { mNPCName = value; }
        }
		
		public string NPCIcon
        {
            get { return mNPCIcon; }
            set { mNPCIcon = value; }
        }
		
		public int NPCGender
        {
            get { return mNPCGender; }
            set { mNPCGender = value; }
        }
		
		public string NpcAppearance
        {
            get { return mNpcAppearance; }
            set { mNpcAppearance = value; }
        }
		
		public string NpcEquips
        {
            get { return mNpcEquips; }
            set { mNpcEquips = value; }
        }
		public int NPCActing
        {
            get { return _mNpc_Acting; }
            set { _mNpc_Acting = value; }
        }
		public int NPCCharm
        {
            get { return _mNpc_Charm; }
            set { _mNpc_Charm = value; }
        }
		public int Cost_Money
        {
            get { return _mCost_Money; }
            set { _mCost_Money = value; }
        }
		public int Cost_Ingot
        {
            get { return _mCost_Ingot; }
            set { _mCost_Ingot = value; }
        }
		
		public void  Load(SecurityElement element)
		{			
			this._mNPCID = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("NPC_ID"),""),-1);
			this.mNPCName = StrParser.ParseStr(element.Attribute("Npc_Name"),"");
			this.mNPCIcon = StrParser.ParseStr(element.Attribute("Npc_Icon"),"");
			
			this.mNPCGender = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Npc_Sex"),""),0);

			
			this.mNpcAppearance = StrParser.ParseStr(element.Attribute("Npc_Appearance"),"");
			this.mNpcEquips = StrParser.ParseStr(element.Attribute("Npc_Equips"),"");
			
			this._mNpc_Acting = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Npc_Acting"),""),-1);
			this._mNpc_Charm = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Npc_Charm"),""),-1);
			
			this._mCost_Money = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Cost_Money"),""),-1);
			this._mCost_Ingot = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Cost_Ingot"),""),-1);
		}

        private int _mNPCID;
		private int mNPCGender;
		private string mNpcAppearance;
		private string mNpcEquips;
		private string  mNPCName;
		private string  mNPCIcon;
		private int _mNpc_Acting;
		private int _mNpc_Charm;
		private int _mCost_Money;
		private int _mCost_Ingot;
	}
	
    public override bool Load (SecurityElement element)
	{
		if(element.Tag != "Items")
			return false;
		
		if(element.Children !=null)
		{
			foreach(SecurityElement childrenElement in element.Children)
			{
				NPCObject accessoryObject = new NPCObject();
				accessoryObject.Load(childrenElement);
				
				if (!_accessoryDict.ContainsKey(accessoryObject.NPCID))
					_accessoryDict[accessoryObject.NPCID] = accessoryObject;
			}
			return true;
		}
		return false;
	}
	
	public bool GetNPCObject(int npcID, out NPCObject npcObject)
	{
		npcObject = null;
		
		foreach (NPCObject accessoryObj in _accessoryDict.Values)
		{
			if (accessoryObj.NPCID == npcID)
			{
				npcObject = accessoryObj;
				return true;
			}
		}
		
		return false;
	}
	

	
	protected Dictionary<int,NPCObject> _accessoryDict = new  Dictionary<int, NPCObject>();
}
