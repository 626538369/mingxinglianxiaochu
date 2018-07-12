using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security;

public class AccessoryConfig : ConfigBase
{
	public class AccessoryObject
	{		
		public struct AccessoryObjectInfo
		{
			public  Vector3 nPosition;
			public  Vector3 nOritation;
			public  Vector3 nScale;
			public  string  nBoneName;
		}
		public int AccessoryID
        {
            get { return _mAccessoryID; }
            set { _mAccessoryID = value; }
        }
		
		
		public string ModelName
        {
            get { return mModelName; }
            set { mModelName = value; }
        }
		
		public string BoneName
        {
            get { return mBoneName; }
            set { mBoneName = value; }
        }
		
		public Vector3 Position
        {
            get { return mPosition; }
            set { mPosition = value; }
        }
		
		public Vector3 Oritation
        {
            get { return mOritation; }
            set { mOritation = value; }
        }
		
		public Vector3 Scale
        {
            get { return mScale; }
            set { mScale = value; }
        }
		
		public void  Load(SecurityElement element)
		{			
			this._mAccessoryID = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Accessory_ID"),""),-1);
			this.mModelName = StrParser.ParseStr(element.Attribute("AccessoryModelName"),"");
			this.mBoneName = StrParser.ParseStr(element.Attribute("BoneName"),"");
			
			this.mPosition.x = StrParser.ParseFloat(StrParser.ParseStr(element.Attribute("PositionX"),""),0);
			this.mPosition.y = StrParser.ParseFloat(StrParser.ParseStr(element.Attribute("PositionY"),""),0);
			this.mPosition.z = StrParser.ParseFloat(StrParser.ParseStr(element.Attribute("PositionZ"),""),0);
			
			this.mOritation.x = StrParser.ParseFloat(StrParser.ParseStr(element.Attribute("OrtationX"),""),0);
			this.mOritation.y = StrParser.ParseFloat(StrParser.ParseStr(element.Attribute("OrtationY"),""),0);
			this.mOritation.z = StrParser.ParseFloat(StrParser.ParseStr(element.Attribute("OrtationZ"),""),0);
			
			this.mScale.x = StrParser.ParseFloat(StrParser.ParseStr(element.Attribute("ScaleX"),""),0);
			this.mScale.y = StrParser.ParseFloat(StrParser.ParseStr(element.Attribute("ScaleY"),""),0);
			this.mScale.z = StrParser.ParseFloat(StrParser.ParseStr(element.Attribute("ScaleZ"),""),0);
		}

        private int _mAccessoryID;
		private Vector3 mPosition;
		private Vector3 mOritation;
		private Vector3 mScale;
		private string  mModelName;
		private string  mBoneName;

		public List<AccessoryObjectInfo> AccessoryObjectInfoList
		{
			get { return mAccessoryObjectInfoList; }
			set { mAccessoryObjectInfoList = value; }	
		}

   		private List<AccessoryObjectInfo> mAccessoryObjectInfoList = new List<AccessoryObjectInfo>();
	}
	
    public override bool Load (SecurityElement element)
	{
		if(element.Tag != "Items")
			return false;
		
		if(element.Children !=null)
		{
			foreach(SecurityElement childrenElement in element.Children)
			{
				AccessoryObject accessoryObject = new AccessoryObject();
				accessoryObject.Load(childrenElement);
				
				if (!_accessoryDict.ContainsKey(accessoryObject.AccessoryID))
					_accessoryDict[accessoryObject.AccessoryID] = accessoryObject;
			}
			return true;
		}
		return false;
	}
	
	public bool GetAccessoryObject(string accessory, out AccessoryObject accessoryObject)
	{
		accessoryObject = null;
		
		foreach (AccessoryObject accessoryObj in _accessoryDict.Values)
		{
			if (accessoryObj.ModelName == accessory)
			{
				accessoryObject = accessoryObj;
				return true;
			}
		}
		
		return false;
	}
	

	
	protected Dictionary<int,AccessoryObject> _accessoryDict = new  Dictionary<int, AccessoryObject>();
}
