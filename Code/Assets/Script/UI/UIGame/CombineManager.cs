using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

//1.6
public class CombineManager
{
	// 合并交叉之后的所有的符合要求的消除组合
	List<Combine> combines = new List<Combine> ();
	// 临时的，里面可能包含 交叉的情况 ，
	List<Combine> tempCombines = new List<Combine> ();
	/// <summary>
	/// 每一个Item  可能对应同一个 Combine
	/// </summary>
	Dictionary<Item, Combine> dic = new Dictionary<Item, Combine> ();
	private int maxCols;
	private int maxRows;
	bool vChecking; // 当为false时检测水平方向， 否则就是垂直方向

	public List<List<Item>> GetCombine ()
	{

		List<List<Item>> combinedItems = new List<List<Item>> ();
		maxCols = EliminationMgr.Instance.maxCols;
		maxRows = EliminationMgr.Instance.maxRows;
		combines.Clear ();
		tempCombines.Clear ();
		dic.Clear ();
		int color = -1;
		Combine combine = new Combine ();
		vChecking = false;
		//Horrizontal searching  先检测每一个Item的水平方向 上
		for (int row = 0; row < maxRows; row++) {
			color = -1;
			for (int col = 0; col < maxCols; col++) {
				Square square = EliminationMgr.Instance.GetSquare (col, row);
				if (IsSquareNotNull (square)) {
					CheckMatches (square.item, color, ref combine);
					color = square.item.color;
				}
			}
		}
		vChecking = true;
		//Vertical searching
		for (int col = 0; col < maxCols; col++) {
			color = -1;
			for (int row = 0; row < maxRows; row++) {
				Square square = EliminationMgr.Instance.GetSquare (col, row);
				if (IsSquareNotNull (square)) {
					CheckMatches (square.item, color, ref combine);
					color = square.item.color;
				}
			}
		}

//		Debug.Log (" test combines detected " + tempCombines.Count);
		CheckCombines ();
//		Debug.Log ("combines detected " + combines.Count);
		//inspect combines
		foreach (Combine cmb in combines) {
			if (cmb.nextType != ItemsTypes.NONE) {
				Item item = cmb.items [UnityEngine.Random.Range (0, cmb.items.Count)];

				Item draggedItem = EliminationMgr.Instance.lastDraggedItem;
				if (draggedItem) {
					if (draggedItem.color != item.color)
						draggedItem = EliminationMgr.Instance.lastSwitchedItem;
					//check the dragged item found in this combine or not and change this type
					if (cmb.items.IndexOf (draggedItem) >= 0) {
						item = draggedItem;
					}
				}
				item.NextType = cmb.nextType;
			}
			combinedItems.Add (cmb.items);			
		}
		return combinedItems;
	}

	void CheckCombines ()
	{
		List<Combine> countedCombines = new List<Combine> ();

		//find and merge cross combines (+)  查找和合并交叉组合
		foreach (Combine comb in tempCombines) {
			if (tempCombines.Count >= 2) {
				foreach (Item item in comb.items) {
					Combine newComb = FindCombineInDic (item);  
					if (comb != newComb && countedCombines.IndexOf (newComb) < 0 && countedCombines.IndexOf (comb) < 0 && IsCombineMatchThree (newComb)) {
						countedCombines.Add (newComb);
						countedCombines.Add (comb);
						Combine mergedCombine = MergeCombines (comb, newComb);
						combines.Add (mergedCombine);
						foreach (Item item_ in comb.items) {
							dic [item_] = mergedCombine;						
						}
						foreach (Item item_ in newComb.items) {
							dic [item_] = mergedCombine;						
						}

						break;
					}
				}
			}
		} 

		//find simple combines (3,4,5)  把没有交叉的组合 也加进来
		foreach (Combine comb in tempCombines) {
			if (combines.IndexOf (comb) < 0 && IsCombineMatchThree (comb) && countedCombines.IndexOf (comb) < 0) {
				combines.Add (comb);
				comb.nextType = SetNextItemType (comb);
			}
		}


//		foreach (var pair in dic) {
//
//			if (combines.IndexOf (pair.Value) < 0 && IsCombineMatchThree (pair.Value)) {
//				pair.Value.nextType = SetNextItemType (pair.Value);
//				combines.Add (pair.Value);
//			}
//		}
	}

	Combine MergeCombines (Combine comb1, Combine comb2)
	{ 
		Combine combine = new Combine ();
		combine.hCount = comb1.hCount + comb2.hCount - 1;
		combine.vCount = comb1.vCount + comb2.vCount - 1;
		combine.items.AddRange (comb1.items);
		combine.items.AddRange (comb2.items);
		combine.nextType = SetNextItemType (combine);
		return combine;
	}

	ItemsTypes SetNextItemType (Combine combine)
	{
//		Debug.Log (combine.hCount + " " + combine.vCount);
		if (combine.hCount > 4 || combine.vCount > 4)
			return ItemsTypes.BOMB;
		if (combine.hCount >= 3 && combine.vCount >= 3)
			return ItemsTypes.PACKAGE;
		if (combine.hCount > 3 || combine.vCount > 3) {
			if (EliminationMgr.Instance.lastDraggedItem) {
				Vector2 dir = EliminationMgr.Instance.lastDraggedItem.moveDirection;
				if (Math.Abs (dir.x) > Math.Abs (dir.y))
					return ItemsTypes.HORIZONTAL_STRIPPED;
				else
					return ItemsTypes.VERTICAL_STRIPPED;
				
			}
			return (ItemsTypes)UnityEngine.Random.Range (1, 3);
		}
		return ItemsTypes.NONE;
	}

	/// <summary>
	/// 检测当前的Item 可以组合结果 left top 方向
	/// </summary>
	/// <param name="item">Item.</param>
	/// <param name="color">Color.</param>
	/// <param name="combine">Combine.</param>
	void CheckMatches (Item item, int color, ref Combine combine)
	{

		//返回 combine 
		combine = FindCombine (item);

		AddItemToCombine (combine, item);
	}

	//
	void AddItemToCombine (Combine combine, Item item)
	{
		combine.AddingItem = item;
		dic [item] = combine;
		// 如果检测 Combine的list的行 列 数量达到3个及以上，达到消除的基本目标
		if (IsCombineMatchThree (combine)) {
			//IndexOf 查找是否在list 中，如果不存在返回-1
			if (tempCombines.IndexOf (combine) < 0) {
				tempCombines.Add (combine);
				//Debug.Log("add " + combine.GetHashCode());
			}
		}
	}

	bool IsCombineMatchThree (Combine combine)
	{
		if (combine.hCount > 2 || combine.vCount > 2) {
			return true;
		}
		return false;
	}

	bool IsSquareNotNull (Square square)
	{
		if (square == null)
			return false;
		if (square.item == null)
			return false;
		return true;
	}

	/// <summary>
	/// 找出当前Item某一个方向上相邻的Item ，如果颜色相同  并且不是特殊元素（现有彩虹球与收集的元素除外）
	/// 如果已存在相对应列表  则返回 ，否则返回一个新的 Combine
	/// </summary>
	/// <returns>The combine.</returns>
	/// <param name="item">Item.</param>
	Combine FindCombine (Item item)
	{
		Combine combine = null;
		Item leftItem = item.GetLeftItem ();
		if (CheckColor (item, leftItem) && !vChecking)
			combine = FindCombineInDic (leftItem);
		if (combine != null)
			return combine;
		Item topItem = item.GetTopItem ();
		if (CheckColor (item, topItem) && vChecking)
			combine = FindCombineInDic (topItem);
		if (combine != null)
			return combine;

		return new Combine ();

	}

	Combine FindCombineInDic (Item item)
	{
		Combine combine;
		if (dic.TryGetValue (item, out combine)) {
			return combine;
		}
		return new Combine ();
	}

	bool CheckColor (Item item, Item nextItem)
	{
		if (nextItem) {
			if (nextItem.color == item.color && nextItem.currentType != ItemsTypes.BOMB && nextItem.currentType != ItemsTypes.INGREDIENT)//2.0
				return true;
		}
		return false;
	}

}

public class Combine
{
	private Item addingItem;
	public List<Item> items = new List<Item> ();
	public int vCount;
	public int hCount;
	Vector2 latestItemPositionH = new Vector2 (-1, -1);
	Vector2 latestItemPositionV = new Vector2 (-1, -1);
	public ItemsTypes nextType;

	/// <summary>
	/// 比较列，当不在同一列时，如果在同一行则累计，不在同一行 重新开始累计 ，然后同理比较行
	/// </summary>
	/// <value>The adding item.</value>
	public Item AddingItem {
		get {
			return addingItem;
		}

		set {
			addingItem = value;
			if (CompareColumns (addingItem)) { // 比较列  其实是在看一行内容，记录对应行 相同元素个数
				if (latestItemPositionH.y != addingItem.square.row && latestItemPositionH.y > -1)
					hCount = 0;
				hCount++;
				latestItemPositionH = new Vector2 (addingItem.square.col, addingItem.square.row);

			} else if (CompareRows (addingItem)) {
				if (latestItemPositionV.x != addingItem.square.col && latestItemPositionV.x > -1)
					vCount = 0;
				vCount++;
				latestItemPositionV = new Vector2 (addingItem.square.col, addingItem.square.row);

			}
			if (hCount > 0 && vCount == 0) {
				vCount = 1;
			}
			items.Add (addingItem);
			//Debug.Log(" c: " + addingItem.square.col + " r: " + addingItem.square.row + " h: " + hCount + " v: " + vCount + " color: " + addingItem.color + " code: " + GetHashCode());
		}

	}

	bool CompareRows (Item item)
	{
		if (items.Count > 0) {
			if (item.square.row > PreviousItem ().square.row)
				return true;
		} else
			return true;

		return false;
	}

	//如果 Item的列数 大于 当前数组中最后一个Item的列数时 ，或者数组没有数据时，返回true
	bool CompareColumns (Item item)
	{
		if (items.Count > 0) {
			if (item.square.col > PreviousItem ().square.col)
				return true;
		} else
			return true;

		return false;
	}


	Item PreviousItem ()
	{
		return items [items.Count - 1];
	}
}
