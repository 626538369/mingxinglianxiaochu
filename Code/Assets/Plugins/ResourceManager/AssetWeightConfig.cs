using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security;

public class AssetWeightConfig
{
	public bool Load (SecurityElement element)
	{
		if(element.Tag != "RootNode")
		{
			Debug.Log("ERROR: Element Tag is Wrong");
			return false;
		}
		
		if(element.Children == null)
		{
			Debug.Log("ERROR: Child Element is Null");
			return false;
		}
		
		foreach(SecurityElement noodElement in element.Children)
		{
			
			AssetWeigth assetWeigth = new AssetWeigth();
			
			int nodeID = StrParser.ParseDecInt(noodElement.Attribute ("ID"), -1);
			assetWeigth.id = nodeID;
			
			foreach(SecurityElement packageElement in noodElement.Children)
			{
				string assetName = StrParser.ParseStr (packageElement.Attribute ("Name"), "");
				int assetValue = StrParser.ParseDecInt (packageElement.Attribute ("Value"), -1);
				
				assetWeigth.nodePackages.Add(assetName, assetValue);
			}
			
			assetWeigthDict.Add(nodeID, assetWeigth);
		}
		
		return true;
	}
	
	public AssetWeigth GetAssetWeigthByNode(int node)
	{
		if( node < 0 )
		{
			Debug.Log("ERROR: Node Is Wrong");
			return null;
		}
		
		return assetWeigthDict[node];
	}
	
	public Dictionary<int, AssetWeigth> assetWeigthDict = new Dictionary<int, AssetWeigth>();
}


public class AssetWeigth
{

	public int id;
	public Dictionary<string,int> nodePackages = new Dictionary<string, int>(); 
	
}