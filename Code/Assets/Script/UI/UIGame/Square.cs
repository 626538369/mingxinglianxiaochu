using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour {

	public Item item;
	public int row;
	public int col;
	//当前格子类型 
	public SquareTypes type;

	// 记录了 当前这个格子里面 有什么类型的效果  例如冰块，或者石头什么的 
	public List<GameObject> block = new List<GameObject> ();


	void Start () {
		if (row == EliminationMgr.Instance.maxRows - 1) {
			if (EliminationMgr.Instance.target == Target.INGREDIENT && (EliminationMgr.Instance.ingrTarget [0] == Ingredients.Ingredient1 || EliminationMgr.Instance.ingrTarget [1] == Ingredients.Ingredient1 || EliminationMgr.Instance.ingrTarget [0] == Ingredients.Ingredient2 || EliminationMgr.Instance.ingrTarget [1] == Ingredients.Ingredient2)) {
				GameObject obj = Instantiate (Resources.Load ("Prefabs/arrow_ingredients")) as GameObject;
				obj.transform.SetParent (transform);
				obj.transform.localScale = new Vector3(0.6f,0.6f,0.6f);
				obj.transform.localPosition = Vector3.zero + Vector3.down * 1f;
			}
		}
	}

	//  参数为true时 有掉落过程 ， 否则直接设置位置 
	public Item GenItem (bool falling = true)
	{
		if (IsNone () && !CanGoInto ())
			return null;
		GameObject item = Instantiate (EliminationMgr.Instance.itemPrefab) as GameObject;
		item.transform.localScale = Vector2.one * 0.6f;
		item.GetComponent<Item> ().square = this;

		item.transform.SetParent (transform.parent);
		if (falling) {
			item.transform.position = transform.position + Vector3.back * 0.2f + Vector3.up * 3f;
			item.GetComponent<Item> ().justCreatedItem = true;
		} else
			item.transform.position = transform.position + Vector3.back * 0.2f;
		this.item = item.GetComponent<Item> ();
		return this.item;
	}

	// 是否是空的 
	public bool IsNone ()
	{
		return type == SquareTypes.NONE;
	}

	/// <summary>
	/// 是否是 可消除 但是没有元素的类型
	/// </summary>
	/// <returns><c>true</c> if this instance is have destroyble obstacle; otherwise, <c>false</c>.</returns>
	public bool IsHaveDestroybleObstacle ()
	{
		return type == SquareTypes.SOLIDBLOCK || type == SquareTypes.THRIVING;

	}
	//
	public bool CanGoOut ()
	{
		return type != SquareTypes.WIREBLOCK;
	}

	public bool CanGoInto ()
	{
		return type != SquareTypes.SOLIDBLOCK && type != SquareTypes.UNDESTROYABLE && type != SquareTypes.NONE && type != SquareTypes.THRIVING;
	}

	// 获取当前位置  是否为 固体 ， 即不可生成元素 
	public bool IsHaveSolidAbove ()
	{
		for (int i = row; i >= 0; i--) {
			if (EliminationMgr.Instance.GetSquare (col, i).type == SquareTypes.SOLIDBLOCK || EliminationMgr.Instance.GetSquare (col, i).type == SquareTypes.UNDESTROYABLE || EliminationMgr.Instance.GetSquare (col, i).type == SquareTypes.THRIVING)
				return true;
		}
		return false;
	}

	/// <summary>
	/// 找出 在特定方向上 可以消除的元素 集合 
	/// EliminationMgr.Instance.onlyFalling 
	/// this.item.justCreatedItem
	/// 在满足条件的情况下 这2个变量 意义在于，在找的时候，如果正在掉落，并且是新生成的 
	/// </summary>
	/// <returns>The matches around.</returns>
	/// <param name="separating">Separating.</param>
	/// <param name="matches">Matches.</param>
	/// <param name="countedSquaresGlobal">Counted squares global.</param>
	public List<Item> FindMatchesAround (FindSeparating separating = FindSeparating.NONE, int matches = 3, Hashtable countedSquaresGlobal = null)
	{
		bool globalCounter = true;
		List<Item> newList = new List<Item> ();
		if (countedSquaresGlobal == null) {
			globalCounter = false;
			countedSquaresGlobal = new Hashtable ();
		}
		Hashtable countedSquares = new Hashtable ();
		countedSquares.Clear ();
		if (this.item == null)
			return newList;

		if (separating != FindSeparating.HORIZONTAL) {
			countedSquares = this.FindMoreMatches (this.item.color, countedSquares, FindSeparating.VERTICAL, countedSquaresGlobal);
		}

		foreach (DictionaryEntry de in countedSquares) {
			EliminationMgr.Instance.countedSquares.Add (EliminationMgr.Instance.countedSquares.Count - 1, de.Value);
		}

		if (countedSquares.Count < matches)
			countedSquares.Clear ();

		if (separating != FindSeparating.VERTICAL) {
			countedSquares = this.FindMoreMatches (this.item.color, countedSquares, FindSeparating.HORIZONTAL, countedSquaresGlobal);
		}

		foreach (DictionaryEntry de in countedSquares) {
			EliminationMgr.Instance.countedSquares.Add (EliminationMgr.Instance.countedSquares.Count - 1, de.Value);
		}

		if (countedSquares.Count < matches)
			countedSquares.Clear ();

		foreach (DictionaryEntry de in countedSquares) {
			newList.Add ((Item)de.Value);
		}
		return newList;
	}
	Hashtable FindMoreMatches (int spr_COLOR, Hashtable countedSquares, FindSeparating separating, Hashtable countedSquaresGlobal = null)
	{
		bool globalCounter = true;
		if (countedSquaresGlobal == null) {
			globalCounter = false;
			countedSquaresGlobal = new Hashtable ();
		}
		if (this.item == null)
			return countedSquares;
		if (this.item.destroying)
			return countedSquares;
		if (this.item.color == spr_COLOR && !countedSquares.ContainsValue (this.item) && this.item.currentType != ItemsTypes.INGREDIENT && item.currentType != ItemsTypes.BOMB) {  //2.0
			if (EliminationMgr.Instance.onlyFalling && this.item.justCreatedItem)
				countedSquares.Add (countedSquares.Count - 1, this.item);
			else if (!EliminationMgr.Instance.onlyFalling)
				countedSquares.Add (countedSquares.Count - 1, this.item);
			else
				return countedSquares;

			if (separating == FindSeparating.VERTICAL) {
				if (GetNeighborTop () != null)
					countedSquares = GetNeighborTop ().FindMoreMatches (spr_COLOR, countedSquares, FindSeparating.VERTICAL);
				if (GetNeighborBottom () != null)
					countedSquares = GetNeighborBottom ().FindMoreMatches (spr_COLOR, countedSquares, FindSeparating.VERTICAL);
			} else if (separating == FindSeparating.HORIZONTAL) {
				if (GetNeighborLeft () != null)
					countedSquares = GetNeighborLeft ().FindMoreMatches (spr_COLOR, countedSquares, FindSeparating.HORIZONTAL);
				if (GetNeighborRight () != null)
					countedSquares = GetNeighborRight ().FindMoreMatches (spr_COLOR, countedSquares, FindSeparating.HORIZONTAL);
			}
		}
		return countedSquares;
	}
	public Square GetNeighborTop (bool safe = false)
	{
		if (row == 0 && !safe)
			return null;
		return EliminationMgr.Instance.GetSquare (col, row - 1, safe);
	}
	public Square GetNeighborBottom (bool safe = false)
	{
		if (row >= EliminationMgr.Instance.maxRows && !safe)
			return null;
		return EliminationMgr.Instance.GetSquare (col, row + 1, safe);
	}
	public Square GetNeighborLeft (bool safe = false)
	{
		if (col == 0 && !safe)
			return null;
		return EliminationMgr.Instance.GetSquare (col - 1, row, safe);
	}

	public Square GetNeighborRight (bool safe = false)
	{
		if (col >= EliminationMgr.Instance.maxCols && !safe)
			return null;
		return EliminationMgr.Instance.GetSquare (col + 1, row, safe);
	}

	/// <summary>
	/// 找到 当前格子里面的Item 下落的位置 ，如果是空的，就继续向下找，如果找到的不是一个特殊的元素格子，并且格子里面没有元素 则向下掉落
	/// </summary>
	public void FallOut ()
	{
		if (item != null) {
			Square nextSquare = GetNeighborBottom ();
			if (nextSquare != null) {
				if (nextSquare.IsNone ()) {
					for (int i = row + 1; i < EliminationMgr.Instance.maxRows; i++) {
						if (EliminationMgr.Instance.GetSquare (col, i) != null) {
							if (!EliminationMgr.Instance.GetSquare (col, i).IsNone ()) {
								nextSquare = EliminationMgr.Instance.GetSquare (col, i);
								break;
							}
						}
					}
				}
				if (nextSquare.CanGoInto ()) {
					if (nextSquare.item == null) {
						item.CheckNeedToFall (nextSquare);
					}
				}
			}
		}
	}

	/// <summary>
	/// 获取 当前格子 旁边的4个格子信息
	/// </summary>
	/// <returns>The all neghbors.</returns>
	public List<Square> GetAllNeghbors ()
	{
		List<Square> sqList = new List<Square> ();
		Square nextSquare = null;
		nextSquare = GetNeighborBottom ();
		if (nextSquare != null)
			sqList.Add (nextSquare);
		nextSquare = GetNeighborTop ();
		if (nextSquare != null)
			sqList.Add (nextSquare);
		nextSquare = GetNeighborLeft ();
		if (nextSquare != null)
			sqList.Add (nextSquare);
		nextSquare = GetNeighborRight ();
		if (nextSquare != null)
			sqList.Add (nextSquare);
		return sqList;
	}

	/// <summary>
	///  消除特殊格子 
	/// </summary>
	public void DestroyBlock ()
	{
		if (type == SquareTypes.UNDESTROYABLE)
			return;
		if (type != SquareTypes.SOLIDBLOCK && type != SquareTypes.THRIVING) {
			List<Square> sqList = GetAllNeghbors ();
			foreach (Square sq in sqList) {
				if (sq.type == SquareTypes.SOLIDBLOCK || sq.type == SquareTypes.THRIVING)
					sq.DestroyBlock ();
			}
		}
		if (block.Count > 0) {
			if (type == SquareTypes.BLOCK) {
				EliminationMgr.Instance.CheckCollectedTarget (gameObject.transform.Find ("Block(Clone)").gameObject);
//				LevelManager.THIS.PopupScore (LevelManager.THIS.scoreForBlock, transform.position, 0);
				EliminationMgr.Instance.targetBlocks--;
				block [block.Count - 1].GetComponent<SpriteRenderer> ().enabled = false;
			}
			if (type == SquareTypes.WIREBLOCK) {
//				LevelManager.THIS.PopupScore (LevelManager.THIS.scoreForWireBlock, transform.position, 0);
			}
			if (type == SquareTypes.SOLIDBLOCK) {
//				LevelManager.THIS.PopupScore (LevelManager.THIS.scoreForSolidBlock, transform.position, 0);
			}
			if (type == SquareTypes.THRIVING) {
//				LevelManager.THIS.PopupScore (LevelManager.THIS.scoreForThrivingBlock, transform.position, 0);
				EliminationMgr.Instance.thrivingBlockDestroyed = true;
			}
			//Destroy( block[block.Count-1]);
			if (type != SquareTypes.BLOCK) {
//				SoundBase.Instance.GetComponent<AudioSource> ().PlayOneShot (SoundBase.Instance.block_destroy);

				block [block.Count - 1].GetComponent<Animation> ().Play ("BrickRotate");
				block [block.Count - 1].GetComponent<SpriteRenderer> ().sortingOrder = 4;
				block [block.Count - 1].AddComponent<Rigidbody2D> ();
				block [block.Count - 1].GetComponent<Rigidbody2D> ().AddRelativeForce (new Vector2 (Random.insideUnitCircle.x * Random.Range (30, 200), Random.Range (100, 150)), ForceMode2D.Force);
			}
			GameObject.Destroy (block [block.Count - 1], 1.5f);
			if (block.Count > 1)
				type = SquareTypes.BLOCK;
			block.Remove (block [block.Count - 1]);

			if (block.Count == 0)
				type = SquareTypes.EMPTY;
		}

	}
}
