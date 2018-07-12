using UnityEngine;
using System.Collections;
using System.Security;
using System.Collections.Generic;

public class AssetPackageConfig
{
	
	public bool Load (SecurityElement element)
	{
		_assetPackage = this.LoadAssetPackage (element);
		return true;
	}
	
	private AssetPackage LoadAssetPackage (SecurityElement element)
	{
		
		if (element.Tag != "Package") {
			Debug.Log ("ERROR: Element Tag is Wrong");
			return null;
		}
		
		AssetPackage assetPackage = new AssetPackage ();
		
		int id = StrParser.ParseDecInt (element.Attribute ("ID"), -1);
		string name = StrParser.ParseStr (element.Attribute ("Name"), "");
		int hierarchy = StrParser.ParseDecInt (element.Attribute ("Hierarchy"), -1);
		int version = StrParser.ParseDecInt (element.Attribute ("Version"), -1);
		
		assetPackage.id = id;
		assetPackage.name = name;
		assetPackage.hierarchy = hierarchy;
		assetPackage.version = version;
		
		foreach (SecurityElement childrenElement in element.Children) {
			if (childrenElement.Tag == "Asset") {
				string assetName = StrParser.ParseStr (childrenElement.Attribute ("Name"), "");
				assetPackage.assetList.Add (assetName);
			}
			
			if (childrenElement.Tag == "Package") {
				AssetPackage childAssetPackage = this.LoadAssetPackage (childrenElement);
				assetPackage.packageList.Add (childAssetPackage);
			}
		}
		_assetPackageDict.Add (name, assetPackage);
		return assetPackage;
	}
	
	//updata package value
	public void nodeValueUpdate (AssetWeigth assetWeigth)
	{
		
		foreach (KeyValuePair<string, int> kv in assetWeigth.nodePackages) 
		{
			
			if(_assetPackageDict.ContainsKey(kv.Key))
			{
				AssetPackage assetPackage = _assetPackageDict[kv.Key];
				assetPackage.nodeValue = kv.Value;
			}

		}
		
		sortAssetPackage(_assetPackage);
	}
	
	public void sortAssetPackage(AssetPackage AssetPackages)
	{
		List<AssetPackage> packageList = AssetPackages.packageList;
		SortedDictionary<int, string> tmpSortDict = new SortedDictionary<int, string>();
		foreach(AssetPackage assetPackage in packageList)
		{
			tmpSortDict.Add(assetPackage.nodeValue, assetPackage.name);
		}
		List<AssetPackage> newPackageList = new List<AssetPackage>();
		foreach(KeyValuePair<int, string> kv in tmpSortDict)
		{
			foreach(AssetPackage assetPackage in packageList)
			{
				if(assetPackage.name == kv.Value)
				{
					newPackageList.Add(assetPackage);
					break;
				}
				else
				{
					continue;
				}
			}
		}
		AssetPackages.packageList = newPackageList;
		foreach(AssetPackage assetPackage in AssetPackages.packageList)
		{
			sortAssetPackage(assetPackage);
		}
	}
	
	/*
	private void packageValueUpdate(AssetPackage assetPackage)
	{
		SortedDictionary<int, AssetPackage> tmpDict = assetPackage.packageList;
		
	}
	*/
	
	//get asset packages
	public AssetPackage AssetPackages
	{
		get
		{
			return _assetPackage;
		}
	}
	
	private AssetPackage _assetPackage;
	private Dictionary<string,AssetPackage> _assetPackageDict = new Dictionary<string,AssetPackage> ();
}

public class AssetPackage
{
	
	public int id;
	public string name;
	public int hierarchy;
	public int version;
	public int nodeValue;
	public List<string> assetList = new List<string>();
	public List<AssetPackage> packageList = new List<AssetPackage>();
	//public SortedDictionary<int, AssetPackage> packageList;
	
	//public bool isLeaf;
}