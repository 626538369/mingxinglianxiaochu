using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security;

public class CharacterConfig : ConfigBase
{
	public class CharacterObject
	{
		
		public struct PartInfo{
			public int partID ;
			public string partModelName;
			public string animationName;
			public int    ItemID;
		};
		
		public int characterID
        {
            get { return _mCharacterID; }
            set { _mCharacterID = value; }
        }
		
		public Color BodyColor
		{
			get {return mBodyColor;}
			set { mBodyColor = value;}
		}
		
		
		public List<PartInfo> PartInfoList
		{
			get { return partList; }
			set { partList = value; }
		}
		
		public List<int> RandomIdleList
		{
			get { return randomIdleList; }
			set { randomIdleList = value; }
		}
		 
		public void  Load(SecurityElement element)
		{			
			this.characterID = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("Character_ID"),""),-1);
			
	
			PartInfo aPartInfo = new PartInfo();
			aPartInfo.partID  = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("HairPartID"),""),-1);
			aPartInfo.partModelName  = StrParser.ParseStr(element.Attribute("HairModelName"),"");
			aPartInfo.animationName =   StrParser.ParseStr(element.Attribute("Animation1"),"");
			aPartInfo.ItemID =   StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("HairPartItemID"),""),0);
			partList.Add(aPartInfo);
			
			
			aPartInfo = new CharacterObject.PartInfo();
			aPartInfo.partID  = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("ClothesPartID"),""),-1);
			aPartInfo.partModelName  = StrParser.ParseStr(element.Attribute("ClothesModelName"),"");
			aPartInfo.animationName =   StrParser.ParseStr(element.Attribute("Animation2"),"");
			aPartInfo.ItemID =   StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("ClothesPartItemID"),""),0);
			partList.Add(aPartInfo);
			
			aPartInfo = new CharacterObject.PartInfo();
			aPartInfo.partID  = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("SkirtPartID"),""),-1);
			aPartInfo.partModelName  = StrParser.ParseStr(element.Attribute("SkirtModelName"),"");
			aPartInfo.animationName =   StrParser.ParseStr(element.Attribute("Animation10"),"");
			aPartInfo.ItemID =   StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("SkirtPartItemID"),""),0);
			partList.Add(aPartInfo);
			

			
			aPartInfo = new CharacterObject.PartInfo();
			aPartInfo.partID  = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("GlovePartID"),""),-1);
			aPartInfo.partModelName  = StrParser.ParseStr(element.Attribute("GloveModelName"),"");
			aPartInfo.animationName =   StrParser.ParseStr(element.Attribute("Animation4"),"");
			aPartInfo.ItemID =   StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("GlovePartItemID"),""),0);
			partList.Add(aPartInfo);
			
			aPartInfo = new CharacterObject.PartInfo();
			aPartInfo.partID  = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("ShoePartID"),""),-1);
			aPartInfo.partModelName  = StrParser.ParseStr(element.Attribute("ShoeModelName"),"");
			aPartInfo.animationName =   StrParser.ParseStr(element.Attribute("Animation5"),"");
			aPartInfo.ItemID =   StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("ShoePartItemID"),""),0);
			partList.Add(aPartInfo);
			
			aPartInfo = new CharacterObject.PartInfo();
			aPartInfo.partID  = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("BraPartID"),""),-1);
			aPartInfo.partModelName  = StrParser.ParseStr(element.Attribute("BraTextureName"),"");
			aPartInfo.animationName =   StrParser.ParseStr(element.Attribute("Animation6"),"");
			aPartInfo.ItemID =   StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("BraPartItemID"),""),0);
			partList.Add(aPartInfo);
						
			aPartInfo = new CharacterObject.PartInfo();
			aPartInfo.partID  = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("UnderWearPartID"),""),-1);
			aPartInfo.partModelName  = StrParser.ParseStr(element.Attribute("UnderWearTextureName"),"");
			aPartInfo.animationName =   StrParser.ParseStr(element.Attribute("Animation7"),"");
			aPartInfo.ItemID =   StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("UnderWearPartID"),""),0);
			partList.Add(aPartInfo);
			
			aPartInfo = new CharacterObject.PartInfo();
			aPartInfo.partID  = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("SocksPartID"),""),-1);
			aPartInfo.partModelName  = StrParser.ParseStr(element.Attribute("SocksTextureName"),"");
			aPartInfo.animationName =   StrParser.ParseStr(element.Attribute("Animation8"),"");
			aPartInfo.ItemID =   StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("SocksPartItemID"),""),0);
			partList.Add(aPartInfo);
			
			aPartInfo = new CharacterObject.PartInfo();
			aPartInfo.partID  = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("GlassesPartID"),""),-1);
			aPartInfo.partModelName  = StrParser.ParseStr(element.Attribute("GlassesModelItemName"),"");
			aPartInfo.animationName =   StrParser.ParseStr(element.Attribute("Animation9"),"");
			aPartInfo.ItemID =   StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("GlassesPartItemID"),""),0);
			partList.Add(aPartInfo);
			
			aPartInfo = new CharacterObject.PartInfo();
			aPartInfo.partID  = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("TrousersPartID"),""),-1);
			aPartInfo.partModelName  = StrParser.ParseStr(element.Attribute("TrousersModelName"),"");
			aPartInfo.animationName =   StrParser.ParseStr(element.Attribute("Animation3"),"");
			aPartInfo.ItemID =   StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("TrousersPartItemID"),""),0);
			partList.Add(aPartInfo);
			
			aPartInfo = new CharacterObject.PartInfo();
			aPartInfo.partID  = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("SuitPartID"),""),-1);
			aPartInfo.partModelName  = StrParser.ParseStr(element.Attribute("SuitModelName"),"");
			aPartInfo.animationName =   StrParser.ParseStr(element.Attribute("Animation11"),"");
			aPartInfo.ItemID =   StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("SuitPartItemID"),""),0);
			partList.Add(aPartInfo);
			
			aPartInfo = new CharacterObject.PartInfo();
			aPartInfo.partID  = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("DressPartID"),""),-1);
			aPartInfo.partModelName  = StrParser.ParseStr(element.Attribute("DressModelName"),"");
			aPartInfo.animationName =   StrParser.ParseStr(element.Attribute("Animation12"),"");
			aPartInfo.ItemID =   StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("DressPartItemID"),""),0);
			partList.Add(aPartInfo);
			
			aPartInfo = new CharacterObject.PartInfo();
			aPartInfo.partID  = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("ShortsPartID"),""),-1);
			aPartInfo.partModelName  = StrParser.ParseStr(element.Attribute("ShortsModelName"),"");
			aPartInfo.animationName =   StrParser.ParseStr(element.Attribute("Animation13"),"");
			aPartInfo.ItemID =   StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("ShortsPartItemID"),""),0);
			partList.Add(aPartInfo);
			
			
			aPartInfo = new CharacterObject.PartInfo();
			aPartInfo.partID  = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("FaceCustomPartID"),""),-1);
			aPartInfo.ItemID =   StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("FaceCustomItemID"),""),0);
			aPartInfo.animationName =  StrParser.ParseStr(element.Attribute("Animation18"),"");
			aPartInfo.partModelName = "";
			partList.Add(aPartInfo);
			
			///裸体部分//
			aPartInfo = new CharacterObject.PartInfo();
			aPartInfo.partID  = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("FacePartID"),""),-1);
			aPartInfo.partModelName  = StrParser.ParseStr(element.Attribute("FaceModelName"),"");
			aPartInfo.animationName =   StrParser.ParseStr(element.Attribute("Animation20"),"");
			partList.Add(aPartInfo);
						
			aPartInfo = new CharacterObject.PartInfo();
			aPartInfo.partID  = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("UpBodyPartID"),""),-1);
			aPartInfo.partModelName  = StrParser.ParseStr(element.Attribute("UpBodyModelName"),"");
			aPartInfo.animationName =   StrParser.ParseStr(element.Attribute("Animation21"),"");
			partList.Add(aPartInfo);
			
			aPartInfo = new CharacterObject.PartInfo();
			aPartInfo.partID  = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("UnderBodyPartID"),""),-1);
			aPartInfo.partModelName  = StrParser.ParseStr(element.Attribute("UnderBodyModelName"),"");
			aPartInfo.animationName =   StrParser.ParseStr(element.Attribute("Animation22"),"");
			partList.Add(aPartInfo);
			
			aPartInfo = new CharacterObject.PartInfo();
			aPartInfo.partID  = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("HandPartID"),""),-1);
			aPartInfo.partModelName  = StrParser.ParseStr(element.Attribute("HandModelName"),"");
			aPartInfo.animationName =   StrParser.ParseStr(element.Attribute("Animation23"),"");
			partList.Add(aPartInfo);
			
			aPartInfo = new CharacterObject.PartInfo();
			aPartInfo.partID  = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("LegPartID"),""),-1);
			aPartInfo.partModelName  = StrParser.ParseStr(element.Attribute("LegModelName"),"");
			aPartInfo.animationName =   StrParser.ParseStr(element.Attribute("Animation24"),"");
			partList.Add(aPartInfo);
			
			aPartInfo = new CharacterObject.PartInfo();
			aPartInfo.partID  = StrParser.ParseDecInt(StrParser.ParseStr(element.Attribute("FootPartID"),""),-1);
			aPartInfo.partModelName  = StrParser.ParseStr(element.Attribute("FootModelName"),"");
			aPartInfo.animationName =  StrParser.ParseStr(element.Attribute("Animation25"),"");
			partList.Add(aPartInfo);
			
			
			string randIdleAnimation = StrParser.ParseStr(element.Attribute("RandomIdle1"),"");
			randomIdleList.Add(101);
			randIdleAnimation = StrParser.ParseStr(element.Attribute("RandomIdle2"),"");
			randomIdleList.Add(102);
			
			randIdleAnimation = StrParser.ParseStr(element.Attribute("RandomIdle3"),"");
			randomIdleList.Add(103);
			
			randIdleAnimation = StrParser.ParseStr(element.Attribute("RandomIdle4"),"");
			randomIdleList.Add(104);
			
			randIdleAnimation = StrParser.ParseStr(element.Attribute("RandomIdle5"),"");
			randomIdleList.Add(105);
			
			this.mBodyColor = StrParser.ParseColor(StrParser.ParseStr(element.Attribute("BodyColor"),""));
			
		}

        private int _mCharacterID;
		
		private Color mBodyColor;
		
		
      	private  List<PartInfo> partList = new List<PartInfo>();
		
		private  List<int> randomIdleList = new List<int>();
   
	}
	
    public override bool Load (SecurityElement element)
	{
		if(element.Tag != "Items")
			return false;
		
		if(element.Children !=null)
		{
			foreach(SecurityElement childrenElement in element.Children)
			{
				CharacterObject characterObject = new CharacterObject();
				characterObject.Load(childrenElement);
				
				if (!_characterObjectDict.ContainsKey(characterObject.characterID))
					_characterObjectDict[characterObject.characterID] = characterObject;
			}
			return true;
		}
		return false;
	}
	
	public bool GetCharacterObject(int characterID, out CharacterObject characterObject)
	{
		characterObject = null;
		
		if (!_characterObjectDict.ContainsKey(characterID))
			return false;
		
		characterObject = _characterObjectDict[characterID];
		return true;
	}
	

	public List<CharacterObject> GetCharacterObjects()
	{
		List<CharacterObject> results = new List<CharacterObject>();
		foreach (KeyValuePair<int, CharacterObject> keyVal in _characterObjectDict)
		{
			results.Add(keyVal.Value);
		}
		
		return results;
	}
	
	protected Dictionary<int,CharacterObject> _characterObjectDict = new  Dictionary<int, CharacterObject>();
}
