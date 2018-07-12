using UnityEngine;
using System.Collections;
using System.Security;
using System.Collections.Generic;

namespace ResAssetBundle
{
	public class AssetPri
	{
		
		protected class Policy
		{
			public Policy (string playerType, int[] policyCollections, int policy3Num, int policy4Num, int policy5Num)
			{
				this.playerType = playerType;
				this.policyCollections = policyCollections;
				this.policy3Num = policy3Num;
				this.policy4Num = policy4Num;
				this.policy5Num = policy5Num;
			}
			
			public string PlayerType {
				get{ return playerType;}
			}
			
			public int[] PolicyCollections {
				get{ return policyCollections;}
			}
			
			public int Policy3Num {
				get{ return policy3Num;}
			}
			
			public int Policy4Num {
				get{ return policy4Num;}
			}
			
			public int Policy5Num {
				get{ return policy5Num;}
			}
			
			protected string playerType;
			protected int[] policyCollections;
			protected int policy3Num;
			protected int policy4Num;
			protected int policy5Num;
		}

		public void Load (SecurityElement e)
		{
			if (e.Tag != "Packages")
				return;
			if(e.Children == null)
				return;
			if (e.Children != null) {
				foreach (SecurityElement child in e.Children) {
					if (child.Tag == "Policys") {
						foreach (SecurityElement childChild in child.Children) {
							string playerType = StrParser.ParseStr (childChild.Attribute ("PlayerType"), "");
							string policyCollection = StrParser.ParseStr (childChild.Attribute ("PolicyCollection"), "");
							string[] policyCollections = policyCollection.Split ('#');
							int[] policyCollectionsInt = new int[policyCollections.Length];
							for (int i=0; i<policyCollections.Length; i++) {
								policyCollectionsInt [i] = StrParser.ParseDecInt (policyCollections [i], 0);
							}
							int policy3Num = StrParser.ParseDecInt (childChild.Attribute ("Policy3Num"), 0);
							int policy4Num = StrParser.ParseDecInt (childChild.Attribute ("Policy4Num"), 0);
							int policy5Num = StrParser.ParseDecInt (childChild.Attribute ("Policy5Num"), 0);
							
							Policy policyObject = new Policy (playerType, policyCollectionsInt, policy3Num, policy4Num, policy5Num);
							_policyDict [playerType] = policyObject;
						}
						
					} else {
						if (child.Tag != "Package") {
							return;
						}
						int level = StrParser.ParseDecInt(child.Attribute ("Level"), 0);
						List<string> assets = new List<string> ();
						foreach (SecurityElement childChild in child.Children) {
							if (childChild.Tag != "Asset") {
								return;
							}
							string fileName = StrParser.ParseStr (childChild.Attribute ("FileName"), "");
							assets.Add (fileName);
						}
						_packageDict [level] = assets;
					}
					
				}
			}
		}
		
		public LinkedList<Dictionary<int, List<string>>> GetAssetsInLevelAndNetwork (int level, int network , LinkedList<Dictionary<int, List<string>>> list)
		{
			if(level==_currentLevel && (network == _currentNetWork)){
				return list;
			}
			
			_linkedList = new LinkedList<Dictionary<int, List<string>>>();
			
			string playerType="";
			_currentLevel = level;
			_currentNetWork = network;
			
			if (network == 0) {
				playerType = "0";
			} else if (network == 1){
				playerType = "1";
			}
			
			Policy policy = _policyDict[playerType];
			int[] policyCollections = policy.PolicyCollections;
			
			foreach (int policyCollection in policyCollections) {
				switch (policyCollection) {
				case 1:
					policyRule1 (policy);
					break;
				case 2:
					policyRule2 (policy);
					break;
				case 3:
					policyRule3 (policy);
					break;
				case 4:
					policyRule4 (policy);
					break;
				case 5:
					policyRule5 (policy);
					break;
				}
			}
			
			/*
			LinkedList<Dictionary<int, List<string>>> linkedList = new LinkedList<Dictionary<int, List<string>>>();
			linkedList = _linkedList;
			int count =linkedList.Count;
			Debug.Log("linkedList count:"+count);
			for(int i =0;i<count;i++)
			{
				Dictionary<int, List<string>> tmpDict = linkedList.First.Value;
				foreach (KeyValuePair<int, List<string>> tmpK in tmpDict) {
					Debug.Log("KEY:"+tmpK.Key);
					List<string> l = tmpK.Value;
					foreach(string fd in l)
					{
						Debug.Log("VALUE:"+fd);
					}
					
				}
				linkedList.RemoveFirst();
			}
			*/
			
			return _linkedList;
		}
		
		private void policyRule4 (Policy policy)
		{
			int maxLevel = 100;
			for (int i= maxLevel; i>(maxLevel-policy.Policy4Num); i--) {
				List<string> list = listCopy(_packageDict [i]);
				Dictionary<int, List<string>> currentPackageDict = new Dictionary<int, List<string>> ();
				currentPackageDict.Add (i, list);
				_linkedList.AddLast (currentPackageDict);
			}
		}
		
		private void policyRule5 (Policy policy)
		{
			int x = policy.Policy5Num / 2;
			int y = policy.Policy5Num % 2;
			
			int begin, end;
			if (y == 0) {
				begin = _currentLevel - x;
				end = _currentLevel + x;
			} else {
				begin = _currentLevel - (x - 1);
				end = _currentLevel + x;
			}
			for (int i = begin; i<=end; i++) {
				if(i == _currentLevel)
				{
					continue;
				}
				List<string> list = listCopy(_packageDict [i]);
				Dictionary<int, List<string>> currentPackageDict = new Dictionary<int, List<string>> ();
				currentPackageDict.Add (i, list);
				_linkedList.AddLast (currentPackageDict);
			}
		}
		
		private void policyRule1 (Policy policy)
		{
			return;
		}
		
		private void policyRule2 (Policy policy)
		{
			List<string> list = listCopy(_packageDict [_currentLevel]);
			Dictionary<int, List<string>> currentPackageDict = new Dictionary<int, List<string>> ();
			currentPackageDict.Add (_currentLevel, list);
			_linkedList.AddLast (currentPackageDict);
		}
		
		private void policyRule3 (Policy policy)
		{
			for (int i=1; i<=policy.Policy3Num; i++) {
				List<string> list = listCopy(_packageDict [i]);
				Dictionary<int, List<string>> currentPackageDict = new Dictionary<int, List<string>> ();
				currentPackageDict.Add (i, list);
				_linkedList.AddLast (currentPackageDict);
			}
		}
		
		private List<string> listCopy(List<string> list)
		{
			List<string> copy = new List<string>();
			foreach(string str in list)
			{
				copy.Add(str);
			}
			return copy;
		}
		
		private Dictionary<int, List<string>> _packageDict = new Dictionary<int, List<string>> ();
		//private Dictionary<string, List<string>> _currentPackageDict = new Dictionary<string, List<string>> ();
		private Dictionary<string, Policy> _policyDict = new Dictionary<string, Policy> ();
		private LinkedList<Dictionary<int, List<string>>> _linkedList = new LinkedList<Dictionary<int, List<string>>> ();
		private int _currentLevel;
		private int _currentNetWork;
		//private string _strategy;
	}
}
