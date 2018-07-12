using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using System.Text;
using System.Security;

public class PortBuildingElement
{
	public int _portID;
	
	public Dictionary<int, List<BuildingElement>> _buildingListByKind = new Dictionary<int, List<BuildingElement>>();
	// public List<BuildingElement> _buildingElementList = new List<BuildingElement>();
	
	public BuildingElement GetBuildingElement(int logicID)
	{
		foreach (List<BuildingElement> list in _buildingListByKind.Values)
		{
			foreach (BuildingElement ele in list)
			{
				if (ele._buildingID == logicID)
					return ele;
			}
		}
		
		return null;
	}
}

public class BuildingElement : IComparable<BuildingElement>
{
	public int _portID;
	public int _buildingID;
	public int _buildingType;
	public int _buildingNameID;
	public int _buildingNpcNameID;
	
	public string _buildIcon;//port info ui
	public string _buildingNpcIcon;
	public int _flourishRequire;
	public int _defenseRequire;
	public string _buildingPostion;
	public string _buildingOrientation;
	public string _buildingModelName;
	public string _buildingTextureName;
	public string _buildingParticleName;
	public int _functionID;
	public int _buffID;
	public int _campID;
	public string _buildBG;
	// Compare function, reverse order
	public int CompareTo(BuildingElement other)
	{
		// if (this._flourishRequire > other._flourishRequire)
		// 	return 1;
		// else if (this._flourishRequire < other._flourishRequire)
		// 	return -1;
		// else
		// 	return 0;
		
		if (this._flourishRequire > other._flourishRequire)
			return -1;
		else if (this._flourishRequire < other._flourishRequire)
			return 1;
		else
			return 0;
	}
}

public class PortsBuildingConfig : ConfigBase
{
	public override bool Load (SecurityElement element)
	{
		if(element.Tag != "Items")
			return false;
		
		if(element.Children != null)
		{
			foreach(SecurityElement childrenElement in element.Children)
			{
				if (childrenElement.Tag == "Item")
				{
					// Auto change Linear data constructor to Tree data
					BuildingElement buildingElement;
					if (!LoadBuildingElement(childrenElement, out buildingElement))
						continue;
					
					InsertBuildingElement(buildingElement);
				}
			}
			
			// Sort it
			foreach (PortBuildingElement portElement in _portBuildingElementList.Values)
			{
				foreach (List<BuildingElement> list in portElement._buildingListByKind.Values)
				{
					list.Sort();
				}
			}
			return true;
		}
		return false;
	}
	
	public bool GetPortBuildingElement(int portID, out PortBuildingElement portBuildingElement)
	{
		portBuildingElement = null;
		
		if (!_portBuildingElementList.TryGetValue(portID, out portBuildingElement))
			return false;
			
		return true;
	}
	
	public bool GetBuildingElement(int buildID, out BuildingElement portBuildingElement)
	{
		portBuildingElement = null;
		
		foreach(PortBuildingElement portBuildElement in _portBuildingElementList.Values)
		{
			portBuildingElement = portBuildElement.GetBuildingElement(buildID);
			if (portBuildingElement != null)
				return true;
		}
		return true;
	}
	
	private void InsertBuildingElement(BuildingElement buildingElement)
	{
		if (_portBuildingElementList.ContainsKey(buildingElement._portID))
		{
			PortBuildingElement portElement = _portBuildingElementList[buildingElement._portID];
			
			// An leixing tianchong
			int type = buildingElement._buildingType;
			bool has = portElement._buildingListByKind.ContainsKey(type);
			if (has)
			{
				List<BuildingElement> list = portElement._buildingListByKind[type];
				list.Add(buildingElement);
			}
			else
			{
				List<BuildingElement> list = new List<BuildingElement>();
				list.Add(buildingElement);
				portElement._buildingListByKind.Add(type, list);
			}
		}
		else
		{
			int type = buildingElement._buildingType;
			PortBuildingElement portElement = new PortBuildingElement();
			List<BuildingElement> list = new List<BuildingElement>();
				list.Add(buildingElement);
			portElement._buildingListByKind.Add(type, list);
			
			_portBuildingElementList.Add(buildingElement._portID, portElement);
		}
	}
	
	// private bool LoadPortBuildingElement(SecurityElement element, out PortBuildingElement portBuildingElement)
	// {
	// 	portBuildingElement = new PortBuildingElement();
	// 	
	// 	string attribute = element.Attribute("Port_ID");
	// 	if (attribute != null)
	// 		portBuildingElement._portID = StrParser.ParseDecInt(attribute, -1);
	// 	
	// 	if (element.Children == null)
	// 		return false;
	// 	
	// 	foreach(SecurityElement childrenElement in element.Children)
	// 	{
	// 		if (childrenElement.Tag == "Building")
	// 		{
	// 			BuildingElement buildingElement;
	// 			if (!LoadBuildingElement(childrenElement, out buildingElement))
	// 				continue;
	// 			
	// 			buildingElement._portID = portBuildingElement._portID;
	// 			
	// 			// An leixing tianchong
	// 			int type = buildingElement._buildingType;
	// 			bool has = portBuildingElement._buildingListByKind.ContainsKey(type);
	// 			if (has)
	// 			{
	// 				List<BuildingElement> list = portBuildingElement._buildingListByKind[type];
	// 				list.Add(buildingElement);
	// 			}
	// 			else
	// 			{
	// 				List<BuildingElement> list = new List<BuildingElement>();
	// 				list.Add(buildingElement);
	// 				portBuildingElement._buildingListByKind.Add(type, list);
	// 			}
	// 			
	// 			portBuildingElement._buildingElementList.Add(buildingElement);
	// 		}
	// 		else
	// 			continue;
	// 	}
	// 	
	// 	return true;
	// }
	
	private bool LoadBuildingElement(SecurityElement element, out BuildingElement buildingElement)
	{
		buildingElement = new BuildingElement();
		
		string attribute = element.Attribute("Port_ID");
		if (attribute != null)
			buildingElement._portID = StrParser.ParseDecInt(attribute, -1);
		
		attribute = element.Attribute("Build_ID");
		if (attribute != null)
			buildingElement._buildingID = StrParser.ParseDecInt(attribute, -1);
		
		attribute = element.Attribute("Build_Type");
		if (attribute != null)
			buildingElement._buildingType = StrParser.ParseDecInt(attribute, -1);
		
		attribute = element.Attribute("Build_Name");
		if (attribute != null)
			buildingElement._buildingNameID = StrParser.ParseDecInt(attribute, -1);
		
		attribute = element.Attribute("Build_NPC_Icon");
		if (attribute != null)
			buildingElement._buildingNpcIcon = StrParser.ParseStr(attribute, "");
		
		attribute = element.Attribute("Build_Icon");
		if (attribute != null)
			buildingElement._buildIcon = StrParser.ParseStr(attribute, "");
		
		attribute = element.Attribute("Build_NPC_Name");
		if (attribute != null)
			buildingElement._buildingNpcNameID = StrParser.ParseDecInt(attribute, -1);
		
		attribute = element.Attribute("Flourish_Require");
		if (attribute != null)
			buildingElement._flourishRequire = StrParser.ParseDecInt(attribute, -1);
		
		attribute = element.Attribute("Defense_Require");
		if (attribute != null)
			buildingElement._defenseRequire = StrParser.ParseDecInt(attribute, -1);
		
		attribute = element.Attribute("Build_Location");
		if (attribute != null)
			buildingElement._buildingPostion = attribute;
		
		attribute = element.Attribute("Build_Faceto");
		if (attribute != null)
			buildingElement._buildingOrientation = attribute;
		
		attribute = element.Attribute("Build_Model");
		if (attribute != null)
			buildingElement._buildingModelName = StrParser.ParseStr(attribute, "");

		attribute = element.Attribute("Build_Texture");
		if (attribute != null)
			buildingElement._buildingTextureName = StrParser.ParseStr(attribute, "");
		
		attribute = element.Attribute("Build_Particle");
		if (attribute != null)
			buildingElement._buildingParticleName = StrParser.ParseStr(attribute, "");
		
		attribute = element.Attribute("Function_ID");
		if (attribute != null)
			buildingElement._functionID = StrParser.ParseDecInt(attribute, -1);
		
		attribute = element.Attribute("Buff_ID");
		if (attribute != null)
			buildingElement._buffID = StrParser.ParseDecInt(attribute, -1);
		
		attribute = element.Attribute("Camp");
		if (attribute != null)
			buildingElement._campID = StrParser.ParseDecInt(attribute, -1);
		
		attribute = element.Attribute("Build_Picture");
		if (attribute != null)
			buildingElement._buildBG = StrParser.ParseStr(attribute, "");
		_BuildingMapList.Add(buildingElement._buildingID,buildingElement._portID);
		
		return true;	
	}
	
	public void GetMapID(int nBuildingID)
	{
		
	}
	
	public bool GetMapID(int nBuildingID, out int nMapID)
	{
		nMapID = -1;
		if(!_BuildingMapList.TryGetValue(nBuildingID, out nMapID))
			return false;
		return true;
	}
	
	private Dictionary<int, PortBuildingElement> _portBuildingElementList = new Dictionary<int, PortBuildingElement>();
	private Dictionary<int, int> _BuildingMapList = new Dictionary<int, int>();
}
