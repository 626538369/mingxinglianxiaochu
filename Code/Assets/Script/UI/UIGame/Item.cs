using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public enum ItemsTypes {
	NONE = 0,
	VERTICAL_STRIPPED,
	HORIZONTAL_STRIPPED,
	PACKAGE,
	BOMB,
	INGREDIENT
}

public class Item : MonoBehaviour {

	//基础元素 底图数组 
	public SpriteDatabase itemsData;
	//消除 生成的  整行 ，列 消除 的元素 
	public StripedItem stripedItem ;

	// 消除生成的 范围性 炸弹 	
	public Sprite packageItem;

	// 消除生成的 彩虹球
	public Sprite[] bombItems;
	// 只用于收集 ，不可消除的元素
//	public Sprite[] ingredientItems;

	//当前Item 的sprite
	public SpriteRenderer sprRenderer;
	// 当前Item 所处在的格子
	public Square square;
	//当前元素  是否在拖动 
	public bool dragThis;

	// 当前点击的位置  
	public Vector3 mousePos;
	// 拖动位置的差值，用来判断是往哪个方向进行了拖动
	public Vector3 deltaPos;
	// 计算得出的方向 信息
	public Vector3 switchDirection;
	// 根据计算得出的方向信息 ，来找到该方向 相邻的为格子信息 ，即交换的格子
	private Square neighborSquare;
	// 交换 格子上的  Item 信息
	private Item switchItem;
	// 掉落中 
	public bool falling;

	private ItemsTypes nextType = ItemsTypes.NONE;
	public ItemsTypes currentType = ItemsTypes.NONE;
	public ItemsTypes debugType = ItemsTypes.NONE;

	// 当前Item 的颜色 值 
	private int COLOR;
	public int color {
		get {
			return COLOR;
		}
		set {
			COLOR = value;
		}
	}

	public ItemsTypes NextType {
		get {
			return nextType;
		}

		set {
			nextType = value;
		}
	}

	public Animator anim;
	public bool destroying;
	public bool appeared;
	public bool animationFinished;
	public bool justCreatedItem;
	private float xScale;
	private float yScale;
	public bool boost;
	public Vector2 moveDirection;


	void Start () {
		falling = true;
		GenColor ();

		// 如果当前元素 是特殊元素  ，一般是消除时 生成的特殊元素
		if (NextType != ItemsTypes.NONE) {
			debugType = NextType;
			currentType = NextType;
			NextType = ItemsTypes.NONE;
			transform.position = square.transform.position + Vector3.back * 0.2f;//1.6.1

			falling = false;
		} 

//		else if (EliminationMgr.Instance.limitType == LIMIT.TIME && UnityEngine.Random.Range (0, 28) == 1) {
//			// 如果当前元素 是普通元素 ，并且 当前模式为时间  有28分之1 的 概率  在当前普通元素上增加 时间增加道具
//			GameObject fiveTimes = Instantiate (Resources.Load ("Prefabs/5sec")) as GameObject;
//			fiveTimes.transform.SetParent (transform);
//			fiveTimes.name = "5sec";
//			fiveTimes.transform.localScale = Vector3.one * 2;
//			fiveTimes.transform.localPosition = Vector3.zero;
//		}
		xScale = transform.localScale.x;
		yScale = transform.localScale.y;
	}

	public void GenColor (int exceptColor = -1, bool onlyNONEType = false) {
		int row = square.row;
		int col = square.col;

		int generateSpecialItemColor = EliminationMgr.Instance.getGenerateSpecialItemColor (col,row);
		if(generateSpecialItemColor > 0){
			StartCoroutine (FallingCor (square, true));
			sprRenderer.sprite = itemsData.find("item" + generateSpecialItemColor);
			color = generateSpecialItemColor;
			if(generateSpecialItemColor == (int)Ingredients.Ingredient1 || generateSpecialItemColor == (int)Ingredients.Ingredient2){
				currentType = ItemsTypes.INGREDIENT;
				name = "ingredient_" + color;
			}else if(generateSpecialItemColor == (int)GameEnumManager.CellType.Bomb){
				currentType = ItemsTypes.INGREDIENT;
				NextType = ItemsTypes.PACKAGE;
				sprRenderer.sprite = packageItem;
			}else if(generateSpecialItemColor == (int)GameEnumManager.CellType.RowClear){
				currentType = ItemsTypes.HORIZONTAL_STRIPPED;
				NextType = ItemsTypes.HORIZONTAL_STRIPPED;
				sprRenderer.sprite = stripedItem.horizontal;
			}else if(generateSpecialItemColor == (int)GameEnumManager.CellType.ColumnClear){
				currentType = ItemsTypes.VERTICAL_STRIPPED;
				NextType = ItemsTypes.VERTICAL_STRIPPED;
				sprRenderer.sprite = stripedItem.vertical;
			}else if(generateSpecialItemColor == (int)GameEnumManager.CellType.RainbowBall){
				currentType = ItemsTypes.BOMB;
				NextType = ItemsTypes.BOMB;
				sprRenderer.sprite = bombItems [0];
			}
			return;
		}



		// 生成的颜色 如果有跟 相邻的不同颜色的情况，则在不同颜色中随机一个
		List<int> remainColors = new List<int> ();
		List<int> totalColors = EliminationMgr.Instance.getBaseColorLimitLst ();
		foreach(int i in totalColors){
//		for (int i = 0; i < EliminationMgr.Instance.colorLimit; i++) {
			bool canGen = true;
			if (col > 1) {
				Square neighbor = EliminationMgr.Instance.GetSquare (row, col - 1);
				if (neighbor != null) {
					if (neighbor.item != null) {
						if (neighbor.CanGoInto () && neighbor.item.color == i)
							canGen = false;
					}
				}
			}
			if (col < EliminationMgr.Instance.maxCols - 1) {
				Square neighbor = EliminationMgr.Instance.GetSquare (row, col + 1);
				if (neighbor != null) {
					if (neighbor.item != null) {
						if (neighbor.CanGoOut () && neighbor.item.color == i)
							canGen = false;
					}
				}
			}
			if (row < EliminationMgr.Instance.maxRows) {
				Square neighbor = EliminationMgr.Instance.GetSquare (row + 1, col);
				if (neighbor != null) {
					if (neighbor.item != null) {
						if (neighbor.CanGoOut () && neighbor.item.color == i)
							canGen = false;
					}
				}
			}
			if (canGen && i != exceptColor) {
				remainColors.Add (i);
			}
		}
//		int randColor = UnityEngine.Random.Range (0, EliminationMgr.Instance.colorLimit);
		int randColor = totalColors[UnityEngine.Random.Range (0, totalColors.Count)];
		if (remainColors.Count > 0)
			randColor = remainColors [UnityEngine.Random.Range (0, remainColors.Count)];
		if (exceptColor == randColor) {
			randColor = totalColors[UnityEngine.Random.Range (0, totalColors.Count)];
		}
//		if (exceptColor == randColor)
//			randColor = (randColor++) % items.Length;

//		EliminationMgr.Instance.lastRandColor = randColor;

		sprRenderer.sprite = itemsData.find("item"+randColor);


		//如果Item 的类型 为特殊元素  ，则需要处理一下 
		if (NextType == ItemsTypes.HORIZONTAL_STRIPPED)
			sprRenderer.sprite = stripedItem.horizontal;//stripedItems [color].horizontal;
		else if (NextType == ItemsTypes.VERTICAL_STRIPPED)
			sprRenderer.sprite = stripedItem.vertical;//stripedItems [color].vertical;
		else if (NextType == ItemsTypes.PACKAGE)
			sprRenderer.sprite = packageItem;//packageItems [color];
		else if (NextType == ItemsTypes.BOMB)
			sprRenderer.sprite = bombItems [0];
		else if ((EliminationMgr.Instance.target == Target.INGREDIENT && (EliminationMgr.Instance.ingrTarget [0] == Ingredients.Ingredient1 || EliminationMgr.Instance.ingrTarget [0] == Ingredients.Ingredient2))
			/*&& UnityEngine.Random.Range (0, EliminationMgr.Instance.Limit) == 0*/ && square.row + 1 < EliminationMgr.Instance.maxRows && !onlyNONEType
			&& EliminationMgr.Instance.GetIngredients (0).Count < EliminationMgr.Instance.ingrCountTarget [0] && EliminationMgr.Instance.getIngredientimitLst((int)EliminationMgr.Instance.ingrTarget [0])) {
			int i = 0;
			if (EliminationMgr.Instance.ingrCountTarget [i] > 0) {
				if (EliminationMgr.Instance.GetIngredients (i).Count < EliminationMgr.Instance.ingrCountTarget [i]) {
					StartCoroutine (FallingCor (square, true));
					color = (int)EliminationMgr.Instance.ingrTarget [i];
					currentType = ItemsTypes.INGREDIENT;
					sprRenderer.sprite = itemsData.find ("item"+color);//ingredientItems [i];
					name = "ingredient_" + color;
				}
			}
		} else if ((EliminationMgr.Instance.target == Target.INGREDIENT && (EliminationMgr.Instance.ingrTarget [1] == Ingredients.Ingredient1 || EliminationMgr.Instance.ingrTarget [1] == Ingredients.Ingredient2))
			/*&& UnityEngine.Random.Range (0, EliminationMgr.Instance.Limit) == 0*/ && square.row + 1 < EliminationMgr.Instance.maxRows && !onlyNONEType
			&& EliminationMgr.Instance.GetIngredients (1).Count < EliminationMgr.Instance.ingrCountTarget [1] && EliminationMgr.Instance.getIngredientimitLst((int)EliminationMgr.Instance.ingrTarget [1])) {
			int i = 1;
			if (EliminationMgr.Instance.ingrCountTarget [i] > 0) {
				if (EliminationMgr.Instance.GetIngredients (i).Count < EliminationMgr.Instance.ingrCountTarget [i]) {
					StartCoroutine (FallingCor (square, true));
					color = (int)EliminationMgr.Instance.ingrTarget [i];
					currentType = ItemsTypes.INGREDIENT;
					sprRenderer.sprite =  itemsData.find ("item"+color);//ingredientItems [i];
					name = "ingredient_" + color;
				}
			}
		} 
		else {
			// 如果不是特殊元素的话  元素
			StartCoroutine (FallingCor (square, true));
//			color = Array.IndexOf (items, sprRenderer.sprite);

			color = randColor;
		}
	}

	IEnumerator FallingCor (Square _square, bool animate) {
		falling = true;
		float startTime = Time.time;
		Vector3 startPos = transform.position;
		float speed = 10;
//		if (EliminationMgr.Instance.gameStatus == GameState.PreWinAnimations)
//			speed = 10;
		float distance = Vector3.Distance (startPos, _square.transform.position);
		float fracJourney = 0;
		if (distance > 0.5f) {
			while (fracJourney < 1) {
				speed += 0.2f;
				float distCovered = (Time.time - startTime) * speed;
				fracJourney = (distCovered / distance)*GameDefines.EliminationFallingSpeed;
				transform.position = Vector3.Lerp (startPos, _square.transform.position + Vector3.back * 0.2f, fracJourney);
				yield return new WaitForFixedUpdate ();

			}
		}
		transform.position = _square.transform.position + Vector3.back * 0.2f;
		if (distance > 0.5f && animate) {
			anim.SetTrigger ("stop");
//			SoundBase.Instance.GetComponent<AudioSource> ().PlayOneShot (SoundBase.Instance.drop [UnityEngine.Random.Range (0, SoundBase.Instance.drop.Length)]);
		}
		falling = false;
		justCreatedItem = false;
		transform.position = _square.transform.position + Vector3.back * 0.2f;//1.6.1
	}


	public Vector3 GetMousePosition () {
//		Debug.LogWarning ("Camera.main.name = "+EliminationMgr.Instance.getMainCamera().name);
	
//		return EliminationMgr.Instance.getMainCamera().ScreenToWorldPoint (Input.mousePosition);

		return EliminationMgr.Instance.XiaoChuCamera.ScreenToWorldPoint (Input.mousePosition);
	}


	void Update () {

		if (currentType != debugType && currentType != ItemsTypes.INGREDIENT) {
			NextType = debugType;
			ChangeType ();

		}
		if (dragThis) {
			deltaPos = mousePos - GetMousePosition ();
			if (switchDirection == Vector3.zero) {
				SwitchDirection (deltaPos);
			}
		}
	}


	public void SwitchDirection (Vector3 delta) {
		deltaPos = delta;
		if (Vector3.Magnitude (deltaPos) > 0.1f) {
			if (Mathf.Abs (deltaPos.x) > Mathf.Abs (deltaPos.y) && deltaPos.x > 0)
				switchDirection.x = 1;
			else if (Mathf.Abs (deltaPos.x) > Mathf.Abs (deltaPos.y) && deltaPos.x < 0)
				switchDirection.x = -1;
			else if (Mathf.Abs (deltaPos.x) < Mathf.Abs (deltaPos.y) && deltaPos.y > 0)
				switchDirection.y = 1;
			else if (Mathf.Abs (deltaPos.x) < Mathf.Abs (deltaPos.y) && deltaPos.y < 0)
				switchDirection.y = -1;
			if (switchDirection.x > 0) {
				neighborSquare = square.GetNeighborLeft ();
			} else if (switchDirection.x < 0) {
				neighborSquare = square.GetNeighborRight ();
			} else if (switchDirection.y > 0) {
				neighborSquare = square.GetNeighborBottom ();
			} else if (switchDirection.y < 0) {
				neighborSquare = square.GetNeighborTop ();
			}
			if (neighborSquare != null)
				switchItem = neighborSquare.item;
			if (switchItem != null) {
				if (switchItem.square.type != SquareTypes.WIREBLOCK)
					StartCoroutine (Switching ());
				else if ((currentType != ItemsTypes.NONE || switchItem.currentType != ItemsTypes.NONE) && switchItem.square.type != SquareTypes.WIREBLOCK)   //1.4.1   1.6.1
					StartCoroutine (Switching ());
				else
					ResetDrag ();//1.6.1

			} else
				ResetDrag ();
		}

	}

	IEnumerator Switching () {
		if (switchDirection != Vector3.zero && neighborSquare != null) {
			bool backMove = false;
			EliminationMgr.Instance.DragBlocked = true;
			neighborSquare.item = this;
			square.item = switchItem;
			int matchesHere = neighborSquare.FindMatchesAround ().Count; // 
			int matchesInThisItem = matchesHere;
			int matchesInNeithborItem = square.FindMatchesAround ().Count;
			Debug.Log ("1  matchesHere = " + matchesHere + " | matchesInNeithborItem = " + matchesInNeithborItem);
			bool thisItemHaveMatch = false;
			if (matchesInThisItem >= 4)
				thisItemHaveMatch = true;
			if (matchesInNeithborItem >= 4)
				thisItemHaveMatch = false;
			int matchesHereOneColor = matchesHere;
			matchesHere += matchesInNeithborItem;
			Debug.Log ("2  matchesHere = " + matchesHere + " | matchesHereOneColor = " + matchesInNeithborItem);
			if ((this.currentType > 0 || switchItem.currentType > 0)&& (this.currentType != ItemsTypes.INGREDIENT && switchItem.currentType != ItemsTypes.INGREDIENT))
				matchesHere++;

			// -------------如果有一个是收集类型的，另一个不是同类型，并且不是彩虹球的 特殊元素时，可以交换-----------//
			if (this.currentType == ItemsTypes.INGREDIENT) {
				if (switchItem.currentType == ItemsTypes.HORIZONTAL_STRIPPED || switchItem.currentType == ItemsTypes.VERTICAL_STRIPPED || switchItem.currentType == ItemsTypes.PACKAGE) {
					matchesHere++;
				}
			} else if(switchItem.currentType == ItemsTypes.INGREDIENT){
				if (this.currentType == ItemsTypes.HORIZONTAL_STRIPPED || this.currentType == ItemsTypes.VERTICAL_STRIPPED || this.currentType == ItemsTypes.PACKAGE) {
					matchesHere++;
				}
			}
			//-------------------------------------------------------------------------------------------------- //

			Debug.Log ("3  matchesHere = " + matchesHere + " | thisItemHaveMatch = " + thisItemHaveMatch);
			//如果2个都是 INGREDIENT 这个类型的时候 表示没有什么变化的   清空记录
			if (this.currentType == ItemsTypes.INGREDIENT && switchItem.currentType == ItemsTypes.INGREDIENT)
				matchesHere = 0;
			float startTime = Time.time;
			Vector3 startPos = transform.position;
			float speed = 5;
			float distCovered = 0;
			while (distCovered < 1) {
				distCovered = (Time.time - startTime) * speed;
				transform.position = Vector3.Lerp (startPos, neighborSquare.transform.position + Vector3.back * 0.3f, distCovered);
				switchItem.transform.position = Vector3.Lerp (neighborSquare.transform.position + Vector3.back * 0.2f, startPos, distCovered);
				yield return new WaitForFixedUpdate ();
			}
			Debug.Log ("4  matchesHere = " + matchesHere + " | matchesInNeithborItem = " + matchesInNeithborItem);

			if (matchesHere <= 0 && matchesInNeithborItem <= 0 ||((this.currentType == ItemsTypes.BOMB || switchItem.currentType == ItemsTypes.BOMB) 
					&& (this.currentType == ItemsTypes.INGREDIENT || switchItem.currentType == ItemsTypes.INGREDIENT) &&(matchesHere + matchesInNeithborItem <= 2)) 
//				||
//				(
//					((int)this.currentType >= 1 || (int)switchItem.currentType >= 1) 
//					&& 
//					(this.currentType == ItemsTypes.INGREDIENT || switchItem.currentType == ItemsTypes.INGREDIENT)
//					&& 
//					(matchesHere + matchesInNeithborItem <= 2)
//				)
			) {
				neighborSquare.item = switchItem;
				square.item = this;
				backMove = true;
			} else {

				if (EliminationMgr.Instance.limitType == LIMIT.MOVES)
					EliminationMgr.Instance.Limit--;
				EliminationMgr.Instance.moveID++;

				switchItem.square = square;
				this.square = neighborSquare;
				EliminationMgr.Instance.lastDraggedItem = this;
				EliminationMgr.Instance.lastSwitchedItem = switchItem;

				// 当 至少有一个消除4个  ,判断是哪个位置的 thisItemHaveMatch = true 时，是在拖动的Item 现在的位置 并且说明 另一个交换的Item 消除没有大于4个
				// 反之 thisItemHaveMatch = false 时 就是交换Item 的现在的位置  并且拖动的Item 位置也可能消除4个以上，需要判断一下
				if (matchesHereOneColor == 4 || matchesInNeithborItem == 4) {
					if (thisItemHaveMatch)
						SetStrippedExtra (startPos - neighborSquare.transform.position);
					else {
						EliminationMgr.Instance.lastDraggedItem = switchItem;
						EliminationMgr.Instance.lastSwitchedItem = this;
						switchItem.SetStrippedExtra (startPos - square.transform.position);
						if (matchesInThisItem == 4)
							SetStrippedExtra (startPos - neighborSquare.transform.position);
					}
				}
				// matchesHere 大于5 说明 总消除数 大于5 
				if (matchesHere >= 5) {
					// thisItemHaveMatch 为true 说明 是在拖动的Item 位置 matchesHereOneColor 大于5 说明这个位置的消除数量是大于等于5 的
					// 这个时候说明  另一个交换的Item 位置上 消除时不会大于等于4 的，不会生成特殊元素
					if (thisItemHaveMatch && matchesHereOneColor >= 5)
						NextType = ItemsTypes.BOMB;
					else if (!thisItemHaveMatch && matchesInNeithborItem >= 5) {
						//thisItemHaveMatch 为false 的时候 判断交换位置 是否生成炸弹 ，同上面提到的 如果是判断交换位置，那么拖动位置也是有可能生成的 也就是下面的if 语句了
						EliminationMgr.Instance.lastDraggedItem = switchItem;
						EliminationMgr.Instance.lastSwitchedItem = this;
						switchItem.NextType = ItemsTypes.BOMB;
						if (matchesInThisItem >= 5)
							NextType = ItemsTypes.BOMB;
					}	
				}
				//--------总结上面 默认是小于4个的情况 然后判断等于4 的情况 ，在检测大于等于5的情况-------------------------//


				// 当交换的2个Item  只要都不是特殊的收集元素   需要先检测 彩虹的情况 ，检测顺序  从大到小 
				if (this.currentType != ItemsTypes.INGREDIENT && switchItem.currentType != ItemsTypes.INGREDIENT) {
					CheckChocoBomb (this, switchItem);
					if (this.currentType != ItemsTypes.BOMB || switchItem.currentType != ItemsTypes.BOMB)
						CheckChocoBomb (switchItem, this);
				}

				// 当 2个Item都是 行 或者列 消除的时候 ，出现十字消除
				if ((this.currentType == ItemsTypes.HORIZONTAL_STRIPPED || this.currentType == ItemsTypes.VERTICAL_STRIPPED) && (switchItem.currentType == ItemsTypes.HORIZONTAL_STRIPPED || switchItem.currentType == ItemsTypes.VERTICAL_STRIPPED)) {
					if (this.currentType == switchItem.currentType) {
						if (this.currentType == ItemsTypes.VERTICAL_STRIPPED) {
							this.currentType = ItemsTypes.HORIZONTAL_STRIPPED;
						} else {
							this.currentType = ItemsTypes.VERTICAL_STRIPPED;
						}
					}	
					this.DestroyItem (true);
					switchItem.DestroyItem (true);
				}
				// --- 20180321   当交换的Item  只要有一个行列消除时，就行列消除-----//
				if((this.currentType == ItemsTypes.HORIZONTAL_STRIPPED||this.currentType == ItemsTypes.VERTICAL_STRIPPED) && (switchItem.currentType == ItemsTypes.NONE || switchItem.currentType == ItemsTypes.INGREDIENT)){
					this.DestroyItem (true);
				}
				if((switchItem.currentType == ItemsTypes.HORIZONTAL_STRIPPED || switchItem.currentType == ItemsTypes.VERTICAL_STRIPPED) && (this.currentType == ItemsTypes.NONE || this.currentType == ItemsTypes.INGREDIENT)){
					switchItem.DestroyItem (true);
				}
				//炸弹跟行列 检测
				CheckPackageAndStripped (this, switchItem);
				CheckPackageAndStripped (switchItem, this);
				
				//炸弹跟炸弹 检测
//				CheckPackageAndPackage (this, switchItem);
				CheckPackageAndPackage (switchItem, this);

				// --- 20180321   当交换的Item  只有一个炸弹消除时-----//	
				if(this.currentType == ItemsTypes.PACKAGE && (switchItem.currentType == ItemsTypes.NONE||switchItem.currentType == ItemsTypes.INGREDIENT)){
					this.DestroyItem ();
				}
				if(switchItem.currentType == ItemsTypes.PACKAGE && (this.currentType == ItemsTypes.NONE||this.currentType == ItemsTypes.INGREDIENT)){
					switchItem.DestroyItem ();
				}
				
				if (this.currentType != ItemsTypes.BOMB || switchItem.currentType != ItemsTypes.BOMB)
					EliminationMgr.Instance.FindMatches ();

			}

			startTime = Time.time;
			distCovered = 0;
			while (distCovered < 1 && backMove) {
				distCovered = (Time.time - startTime) * speed;
				transform.position = Vector3.Lerp (neighborSquare.transform.position + Vector3.back * 0.3f, startPos, distCovered);
				switchItem.transform.position = Vector3.Lerp (startPos, neighborSquare.transform.position + Vector3.back * 0.2f, distCovered);
				yield return new WaitForFixedUpdate ();
			}

			if (backMove)
				EliminationMgr.Instance.DragBlocked = false;
		}
		ResetDrag ();
	}	

	void ResetDrag () {
		dragThis = false;

		switchDirection = Vector3.zero;

		neighborSquare = null;
		switchItem = null;
	}

	public void SetAnimationDestroyingFinished () {
//		LevelManager.THIS.itemsHided = true;
		animationFinished = true;
	}

	/// <summary>
	/// 2个炸弹消除 
	/// </summary>
	/// <param name="item1">Item1.</param>
	/// <param name="item2">Item2.</param>
	void CheckPackageAndPackage (Item item1, Item item2) {
		if (item1.currentType == ItemsTypes.PACKAGE && item2.currentType == ItemsTypes.PACKAGE) {
			int i = 0;
			List<Item> bigList = new List<Item> ();
			List<Item> itemsList = EliminationMgr.Instance.GetItemsAround (item2.square);
			foreach (Item item in itemsList) {
				if (item != null) {
					bigList.AddRange (EliminationMgr.Instance.GetItemsAround (item.square));
				}
			}
			foreach (Item item in bigList) {
				if (item != null) {
					if(item == item1 ||item == item2)
					{
						item.currentType = ItemsTypes.NONE;
						item.debugType = ItemsTypes.NONE;
						item.DestroyItem ();
						continue;
					}
					if (item.currentType != ItemsTypes.BOMB && item.currentType != ItemsTypes.INGREDIENT)
						item.DestroyItem (true,0, "destroy_package");

					if(item.currentType == ItemsTypes.BOMB){
						if (EliminationMgr.Instance.GetRandomItems (1).Count > 0) 
							CheckChocoBomb (item, EliminationMgr.Instance.GetRandomItems (1) [0]);
					}
				}
			}
			GameObject partc2 = Instantiate(Resources.Load("SpecialEffects/BOMBBIG"), item2.transform.position, Quaternion.identity) as GameObject;
//			item1.DestroyItem ();
//			item2.DestroyItem ();
		}
	}

	/// <summary>
	/// 行消除 
	/// </summary>
	public void DestroyHorizontal () {
//		SoundBase.Instance.GetComponent<AudioSource> ().PlayOneShot (SoundBase.Instance.strippedExplosion);
		GameObject partc2 = Instantiate(Resources.Load("SpecialEffects/ROW"), transform.position, Quaternion.identity) as GameObject;
		EliminationMgr.Instance.StrippedShow (gameObject, true);

		List<Item> itemsList = EliminationMgr.Instance.GetRow (square.row);
		foreach (Item item in itemsList) {
			if (item != null) {
				if (item.currentType != ItemsTypes.BOMB && item.currentType != ItemsTypes.INGREDIENT)
					item.DestroyItem (true);
				if(item.currentType == ItemsTypes.BOMB){
					if (EliminationMgr.Instance.GetRandomItems (1).Count > 0) 
						CheckChocoBomb (item, EliminationMgr.Instance.GetRandomItems (1) [0]);
				}
			}
		}
		List<Square> sqList = EliminationMgr.Instance.GetRowSquaresObstacles (square.row);
		foreach (Square item in sqList) {
			if (item != null)
				item.DestroyBlock ();
		}
	}
	/// <summary>
	/// 列消除
	/// </summary>
	public void DestroyVertical () {
		GameObject partc2 = Instantiate(Resources.Load("SpecialEffects/ROW"), transform.position, Quaternion.identity) as GameObject;
		partc2.transform.localEulerAngles = new Vector3(0,0,90);
//		SoundBase.Instance.GetComponent<AudioSource> ().PlayOneShot (SoundBase.Instance.strippedExplosion);
		EliminationMgr.Instance.StrippedShow (gameObject, false);
		List<Item> itemsList = EliminationMgr.Instance.GetColumn (square.col);
		foreach (Item item in itemsList) {
			if (item != null) {
				if (item.currentType != ItemsTypes.BOMB && item.currentType != ItemsTypes.INGREDIENT)
					item.DestroyItem (true);
				if(item.currentType == ItemsTypes.BOMB){
					if (EliminationMgr.Instance.GetRandomItems (1).Count > 0) 
						CheckChocoBomb (item, EliminationMgr.Instance.GetRandomItems (1) [0]);
				}
			}
		}
		List<Square> sqList = EliminationMgr.Instance.GetColumnSquaresObstacles (square.col);
		foreach (Square item in sqList) {
			if (item != null)
				item.DestroyBlock ();
		}
	}
		
	/// <summary>
	/// 一个 行 列消除的特殊元素    一个3*3范围炸弹
	/// </summary>
	/// <param name="item1">Item1.</param>
	/// <param name="item2">Item2.</param>
	void CheckPackageAndStripped (Item item1, Item item2) {
		if (((item1.currentType == ItemsTypes.HORIZONTAL_STRIPPED || item1.currentType == ItemsTypes.VERTICAL_STRIPPED) && item2.currentType == ItemsTypes.PACKAGE)) {

			List<Item> itemsList = EliminationMgr.Instance.GetItemsAround (item2.square);
			int direction = 1;//2.0
			foreach (Item item in itemsList) {
				if (item != null) {
					if (item.currentType != ItemsTypes.BOMB && item.currentType != ItemsTypes.INGREDIENT) {//1.6.1
						if (direction > 0)//2.0
							item.currentType = ItemsTypes.HORIZONTAL_STRIPPED;
						else
							item.currentType = ItemsTypes.VERTICAL_STRIPPED;
						direction *= -1;
						item.DestroyItem (true);
					}
				}
			}
//			item2.DestroyPackage ();
//			item2.DestroyItem (true);
		}
	}
	/// <summary>
	/// 检测彩虹球 ， 当有一个为 特殊的收集元素时  ，不进行检测 
	/// 当参数1 为彩虹球时 ， 根据参数2 的类型来做不同的处理
	/// </summary>
	/// <param name="item1">Item1.</param>
	/// <param name="item2">Item2.</param>
	public void CheckChocoBomb (Item item1, Item item2) {
		if (item1.currentType == ItemsTypes.INGREDIENT || item2.currentType == ItemsTypes.INGREDIENT)
			return;
		if (item1.currentType == ItemsTypes.BOMB) {
			if (item2.currentType == ItemsTypes.NONE)
			{
//				DestroyColor (item2.color, item1);
				item1.DestroyColorForUE(item2.color, item1);
			}
				
			else if (item2.currentType == ItemsTypes.HORIZONTAL_STRIPPED || item2.currentType == ItemsTypes.VERTICAL_STRIPPED)
			{
//				EliminationMgr.Instance.SetTypeByColor (item2.color, ItemsTypes.HORIZONTAL_STRIPPED);
//				EliminationMgr.Instance.DestroyBombs (item2.currentType);
				ItemsTypes nextType = item2.currentType;
				EliminationMgr.Instance.DestroyStartBombsSpeciel(nextType ,item1, item2);
			}

			else if (item2.currentType == ItemsTypes.PACKAGE){
//				EliminationMgr.Instance.SetTypeByColor (item2.color, ItemsTypes.PACKAGE);
//				EliminationMgr.Instance.DestroyBombs (ItemsTypes.PACKAGE);

				EliminationMgr.Instance.DestroyStartBombsSpeciel(ItemsTypes.PACKAGE ,item1 ,item2);
			}
			else if (item2.currentType == ItemsTypes.BOMB)
				EliminationMgr.Instance.DestroyDoubleBomb (square.col);

//			item1.DestroyItem ();
		}

	}

	public void DestroyColorForUE (int p, Item item1) {	
		StartCoroutine(DestroyColorIE (p, item1));
	}
		// 当彩虹球与普通元素交换时， 需要删除对应颜色的所有元素
	public IEnumerator DestroyColorIE (int p, Item item1) {
		//		SoundBase.Instance.GetComponent<AudioSource> ().PlayOneShot (SoundBase.Instance.colorBombExpl);
		GameObject[] items = GameObject.FindGameObjectsWithTag ("Item");
		foreach (GameObject item in items) {
			if(item == null)
				continue;
			Item it = item.GetComponent<Item> ();
			if(it == null||it.destroying)
				continue;
			if (it.color == p)
			{
//				if (item1.destroying)
//					continue;
				GameObject partc2 = Instantiate(Resources.Load("SpecialEffects/Tron"),item1.transform.position, Quaternion.identity) as GameObject;
				UVChainLightning uv = partc2.GetComponent<UVChainLightning>();
//				Transform t = partc2.transform.Find("EndPos");
//				iTween.MoveTo(t.gameObject,iTween.Hash("position",item.transform.position,"time",0.2f),null,delegate {
//					DestroyObject(partc2);
//				});
				uv.target.DOMove(item.transform.position,0.2f).OnComplete(delegate() {
					DestroyObject(partc2);
				});
				yield return new WaitForSeconds (0.1f);
				if(item == null)
					continue;
				item.GetComponent<Item> ().DestroyItem (true,0, "", true);			
			}
		}
		yield return new WaitForSeconds (0.1f);
		item1.DestroyItemLast (true);
	}
	// 当彩虹球与普通元素交换时， 需要删除对应颜色的所有元素
	public void DestroyColor (int p, Item item1) {
//		SoundBase.Instance.GetComponent<AudioSource> ().PlayOneShot (SoundBase.Instance.colorBombExpl);
		GameObject[] items = GameObject.FindGameObjectsWithTag ("Item");
		foreach (GameObject item in items) {
			if (item.GetComponent<Item> ().color == p)
				item.GetComponent<Item> ().DestroyItem (true,0, "", true);
		}
		item1.DestroyItem (true);
	}

	public void DestroyPackage () {
//		SoundBase.Instance.GetComponent<AudioSource> ().PlayOneShot (SoundBase.Instance.destroyPackage);

		GameObject partc2 = Instantiate(Resources.Load("SpecialEffects/BOMB"), transform.position, Quaternion.identity) as GameObject;
		List<Item> itemsList = EliminationMgr.Instance.GetItemsAround (square);
		foreach (Item item in itemsList) {
			if (item != null) {
				if (item.currentType != ItemsTypes.BOMB && item.currentType != ItemsTypes.INGREDIENT)
					item.DestroyItem (true,0, "destroy_package");
				if(item.currentType == ItemsTypes.BOMB){
					if (EliminationMgr.Instance.GetRandomItems (1).Count > 0) 
						CheckChocoBomb (item, EliminationMgr.Instance.GetRandomItems (1) [0]);
				}
			}
		}
//		SoundBase.Instance.GetComponent<AudioSource> ().PlayOneShot (SoundBase.Instance.explosion);
//		currentType = ItemsTypes.NONE;
//		DestroyItem (true);
	}
	//延迟销毁调用 
	public void DestroyItemLast (bool showScore = false) {
		StartCoroutine (DestroyCorLast (showScore));
	}
	//延迟销毁调用
	IEnumerator DestroyCorLast (bool showScore = false) {

		if (currentType == ItemsTypes.HORIZONTAL_STRIPPED)
			PlayDestroyAnimation ("destroy");
		else if (currentType == ItemsTypes.VERTICAL_STRIPPED)
			PlayDestroyAnimation ("destroy");
		else if (currentType == ItemsTypes.PACKAGE) {
			PlayDestroyAnimation ("destroy");
			yield return new WaitForSeconds (0.1f);
		} else if (currentType != ItemsTypes.INGREDIENT && currentType != ItemsTypes.BOMB) {
			PlayDestroyAnimation ("destroy");
		}else if (currentType == ItemsTypes.BOMB) {
			PlayDestroyAnimation ("destroy");
		}
		while (!animationFinished && currentType == ItemsTypes.NONE)
			yield return new WaitForFixedUpdate ();
		Destroy (gameObject);
	}
	public void DestroyItem (bool showScore = false,int addScore = 0, string anim_name = "", bool explEffect = false) {

		if (destroying)
			return;
		if (this == null)
			return;
//		StopCoroutine (AnimIdleStart ());
		destroying = true;
		square.item = null;

		if (this == null)
			return;

		StartCoroutine (DestroyCor (showScore,addScore, anim_name, explEffect));
	}

	IEnumerator DestroyCor (bool showScore = false,int addScore = 0, string anim_name = "", bool explEffect = false) {

		if (currentType == ItemsTypes.HORIZONTAL_STRIPPED)
			PlayDestroyAnimation ("destroy");
		else if (currentType == ItemsTypes.VERTICAL_STRIPPED)
			PlayDestroyAnimation ("destroy");
		else if (currentType == ItemsTypes.PACKAGE) {
			PlayDestroyAnimation ("destroy");

			yield return new WaitForSeconds (0.1f);

//			GameObject partcl = Instantiate (Resources.Load ("Prefabs/Effects/Firework"), transform.position, Quaternion.identity) as GameObject;
//			partcl.GetComponent<ParticleSystem> ().startColor = LevelManager.THIS.scoresColors [color];
//			Destroy (partcl, 1f);
		} else if (currentType != ItemsTypes.INGREDIENT && currentType != ItemsTypes.BOMB) {
			PlayDestroyAnimation ("destroy");
//			GameObject partc2 = Instantiate(Resources.Load("SpecialEffects/POW"), transform.position, Quaternion.identity) as GameObject;
//			partc2.transform.SetParent(transform);
//			GameObject partcl = LevelManager.THIS.GetExplFromPool ();
//			if (partcl != null) {
//				partcl.GetComponent<ItemAnimEvents> ().item = this;
//				partcl.transform.localScale = Vector3.one * 0.5f;
//				partcl.transform.position = transform.position;
//				partcl.GetComponent<Animator> ().SetInteger ("color", color);
//				SoundBase.Instance.GetComponent<AudioSource> ().PlayOneShot (SoundBase.Instance.destroy [UnityEngine.Random.Range (0, SoundBase.Instance.destroy.Length)]);
//
//			}
//			if (explEffect) {
//				GameObject partcl1 = Instantiate (Resources.Load ("Prefabs/Effects/Replace"), transform.position, Quaternion.identity) as GameObject;
//				Destroy (partcl1, 1f);
//
//			}
		}else if (currentType == ItemsTypes.BOMB) {
//			PlayDestroyAnimation ("destroy");
//			GameObject partc2 = Instantiate(Resources.Load("SpecialEffects/POW"), transform.position, Quaternion.identity) as GameObject;
//			partc2.transform.SetParent(transform);
		}

		// ----------------- 当关卡的类型为时间时， 增加时间的道具效果 ------- //
//		if (EliminationMgr.Instance.limitType == LIMIT.TIME && transform.Find ("5sec") != null) {
//			GameObject FiveSec = transform.Find ("5sec").gameObject;
//			FiveSec.transform.SetParent (null);
//			#if UNITY_5
//			FiveSec.GetComponent<Animation> ().clip.legacy = true;
//			#endif
//
//			FiveSec.GetComponent<Animation> ().Play ("5secfly");
//			Destroy (FiveSec, 1);
//
//			EliminationMgr.Instance.AddsecEffect ();
//		}


		//消除基础元素增加分数
		if(showScore && this.currentType == ItemsTypes.NONE)
		{			

			Vector3 newPosition = new Vector3 (transform.position.x*1024f/7f*((float)Screen.width/1152f),transform.position.y*1024f/7f*((float)Screen.width/1152f),420);
			if (addScore > 0) {
				GameObject obj = Instantiate(Resources.Load("SpecialEffects/Score"), newPosition, Quaternion.identity) as GameObject;
				ScoreItem Scoreobj = obj.GetComponent<ScoreItem>();
				Scoreobj.Init(this.color ,addScore);

//				EliminationMgr.Instance.PopupScore(this.color ,addScore);
			}else{
				int specialScore = EliminationMgr.Instance.getSpecialEliminateScore (this.currentType,this.color);
				if(specialScore > 0){
					GameObject obj = Instantiate(Resources.Load("SpecialEffects/Score"), newPosition, Quaternion.identity) as GameObject;
					ScoreItem Scoreobj = obj.GetComponent<ScoreItem>();
					Scoreobj.Init(this.color ,specialScore);

//					EliminationMgr.Instance.PopupScore(this.color ,specialScore);
				}
			}
		}
		EliminationMgr.Instance.CheckCollectedTarget (gameObject);

		while (!animationFinished && currentType == ItemsTypes.NONE)
			yield return new WaitForFixedUpdate ();
		bool isDestroy = true;
		square.DestroyBlock ();
		if (currentType == ItemsTypes.HORIZONTAL_STRIPPED)
			DestroyHorizontal ();
		else if (currentType == ItemsTypes.VERTICAL_STRIPPED)
			DestroyVertical ();
		else if (currentType == ItemsTypes.PACKAGE)
			DestroyPackage ();
		else if (currentType == ItemsTypes.BOMB) //EliminationMgr.Instance.GameStatus == 3 游戏结束后，消除的Item为彩虹球的时候 随即选择几个消除
		{
			if (EliminationMgr.Instance.GameStatus == GameStatusEnum.PreWinAnimations) {
				isDestroy = false;
				if (EliminationMgr.Instance.GetRandomItems (1).Count > 0) 
					CheckChocoBomb (this, EliminationMgr.Instance.GetRandomItems (1) [0]);
			}
		}	

		if (NextType != ItemsTypes.NONE) {
			Item i = square.GenItem ();
			i.NextType = NextType;
			i.SetColor (color);
			i.ChangeType ();
		}

		if (destroying&&isDestroy) {
			Destroy (gameObject);
		}
	}


	void PlayDestroyAnimation (string anim_name) {
		anim.SetTrigger (anim_name);

	}

//	public void SetColor (int col) {
//		color = col;
//
//
//		sprRenderer.sprite = itemsData.find("item"+color);
//
////		if (color < items.Length)
////			sprRenderer.sprite = items [color];
//	}
	public void SetColor (int col) {

		StartCoroutine (SetColorIE (col));
	}
	IEnumerator SetColorIE (int col) {
		anim.SetTrigger ("appear");
		color = col;
		while (!appeared)
			yield return new WaitForFixedUpdate ();
		sprRenderer.sprite = itemsData.find("item"+color);
	}
	/// <summary>
	/// 根据 2个Item 的坐标 做一个差值，来判断生成一个什么特殊元素
	/// </summary>
	/// <param name="dir">Dir.</param>
	void SetStrippedExtra (Vector3 dir) {
		moveDirection = dir;//1.6

		if (Math.Abs (dir.x) > Math.Abs (dir.y))
			NextType = ItemsTypes.HORIZONTAL_STRIPPED;
		else
			NextType = ItemsTypes.VERTICAL_STRIPPED;
	}

	public void ChangeType () {
		if (this != null)
			StartCoroutine (ChangeTypeCor ());
	}
	IEnumerator ChangeTypeCor () {
		if (NextType == ItemsTypes.HORIZONTAL_STRIPPED) {
			anim.SetTrigger ("appear");
//			SoundBase.Instance.GetComponent<AudioSource> ().PlayOneShot (SoundBase.Instance.appearStipedColorBomb);
			color = 402;
		} else if (NextType == ItemsTypes.VERTICAL_STRIPPED) {
			anim.SetTrigger ("appear");
//			SoundBase.Instance.GetComponent<AudioSource> ().PlayOneShot (SoundBase.Instance.appearStipedColorBomb);
			color = 403;
		} else if (NextType == ItemsTypes.PACKAGE) {
			anim.SetTrigger ("appear");
//			SoundBase.Instance.GetComponent<AudioSource> ().PlayOneShot (SoundBase.Instance.appearPackage);
			color = 401;
		} else if (NextType == ItemsTypes.BOMB) {
			anim.SetTrigger ("appear");
//			SoundBase.Instance.GetComponent<AudioSource> ().PlayOneShot (SoundBase.Instance.appearStipedColorBomb);
			color = 404;
		}
		while (!appeared)
			yield return new WaitForFixedUpdate ();

		if (NextType == ItemsTypes.NONE)
			yield break;

		if (NextType == ItemsTypes.HORIZONTAL_STRIPPED)
			sprRenderer.sprite = stripedItem.horizontal;//stripedItems [color].horizontal;
		else if (NextType == ItemsTypes.VERTICAL_STRIPPED)
			sprRenderer.sprite = stripedItem.vertical;//stripedItems [color].vertical;
		else if (NextType == ItemsTypes.PACKAGE)
			sprRenderer.sprite = packageItem;//packageItems [color];
		else if (NextType == ItemsTypes.BOMB)
			sprRenderer.sprite = bombItems [0];

		debugType = NextType;
		currentType = NextType;
		NextType = ItemsTypes.NONE;

	}

	/// <summary>
	/// 20180321 改变 类型  并且引爆
	/// </summary>
	public void ChangeTypeAndDestroy () {
		if (this != null)
			StartCoroutine (ChangeTypeCorAndDestroy ());
	}
	IEnumerator ChangeTypeCorAndDestroy () {
		if (NextType == ItemsTypes.HORIZONTAL_STRIPPED) {
			anim.SetTrigger ("appear");
			//			SoundBase.Instance.GetComponent<AudioSource> ().PlayOneShot (SoundBase.Instance.appearStipedColorBomb);
			color = 402;
		} else if (NextType == ItemsTypes.VERTICAL_STRIPPED) {
			anim.SetTrigger ("appear");
			//			SoundBase.Instance.GetComponent<AudioSource> ().PlayOneShot (SoundBase.Instance.appearStipedColorBomb);
			color = 403;
		} else if (NextType == ItemsTypes.PACKAGE) {
			anim.SetTrigger ("appear");
			//			SoundBase.Instance.GetComponent<AudioSource> ().PlayOneShot (SoundBase.Instance.appearPackage);
			color = 401;
		} else if (NextType == ItemsTypes.BOMB) {
			anim.SetTrigger ("appear");
			//			SoundBase.Instance.GetComponent<AudioSource> ().PlayOneShot (SoundBase.Instance.appearStipedColorBomb);
			color = 404;
		}
		while (!appeared)
			yield return new WaitForFixedUpdate ();

		if (NextType == ItemsTypes.NONE)
			yield break;

		if (NextType == ItemsTypes.HORIZONTAL_STRIPPED)
			sprRenderer.sprite = stripedItem.horizontal;//stripedItems [color].horizontal;
		else if (NextType == ItemsTypes.VERTICAL_STRIPPED)
			sprRenderer.sprite = stripedItem.vertical;//stripedItems [color].vertical;
		else if (NextType == ItemsTypes.PACKAGE)
			sprRenderer.sprite = packageItem;//packageItems [color];
		else if (NextType == ItemsTypes.BOMB)
			sprRenderer.sprite = bombItems [0];

		debugType = NextType;
		currentType = NextType;
		NextType = ItemsTypes.NONE;
	}


	public void StartFalling () {
		if (!falling)
			StartCoroutine (FallingCor (square, true));
	}

	public bool GetNearEmptySquares () {
		bool nearEmptySquareDetected = false;
		if (square.row < EliminationMgr.Instance.maxRows - 1 && square.col < EliminationMgr.Instance.maxCols) {
			Square checkingSquare = EliminationMgr.Instance.GetSquare (square.col + 1, square.row + 1, true);
			if (checkingSquare.CanGoInto () && checkingSquare.item == null && !falling) {//2.0
				checkingSquare = EliminationMgr.Instance.GetSquare (square.col + 1, square.row + 1, true);
				if (checkingSquare.CanGoInto ()) {
					if (checkingSquare.item == null) {
						square.item = null;
						checkingSquare.item = this;
						square = checkingSquare;
						StartFalling ();//2.0
						nearEmptySquareDetected = true;
					}
				}
			}
		}
		if (square.row < EliminationMgr.Instance.maxRows - 1 && square.col > 0) {
			Square checkingSquare = EliminationMgr.Instance.GetSquare (square.col - 1, square.row + 1, true);
			if (checkingSquare.CanGoInto () && checkingSquare.item == null && !falling) {//2.0
				checkingSquare = EliminationMgr.Instance.GetSquare (square.col - 1, square.row + 1, true);
				if (checkingSquare.CanGoInto ()) {
					if (checkingSquare.item == null) {
						square.item = null;
						checkingSquare.item = this;
						square = checkingSquare;
						StartFalling ();//2.0
						nearEmptySquareDetected = true;
					}
				}
			}
		}

		return nearEmptySquareDetected;
	}
	public void SmoothDestroy () {
		StartCoroutine (SmoothDestroyCor ());
	}

	IEnumerator SmoothDestroyCor () {
		square.item = null;
		anim.SetTrigger ("disAppear");
		yield return new WaitForSeconds (1);
		Destroy (gameObject);
	}
	public void CheckNeedToFall (Square _square) {
		_square.item = this;
		square.item = null;
		square = _square;   //need to count all falling items and drop them down in the same time
	}

	public void SetAppeared () {
		appeared = true;
//		StartIdleAnim ();
		if (currentType == ItemsTypes.PACKAGE)
			anim.SetBool ("package_idle", true);
		else if (currentType == ItemsTypes.BOMB)
		{
			anim.SetBool ("package_circle", true);
		}
		else if (currentType == ItemsTypes.HORIZONTAL_STRIPPED)
		{
			anim.SetBool ("package_row", true);
		}
		else if (currentType == ItemsTypes.VERTICAL_STRIPPED)
		{
			anim.SetBool ("package_colum", true);
		}

	}

	public Item GetLeftItem () {
		Square sq = square.GetNeighborLeft ();
		if (sq != null) {
			if (sq.item != null)
				return sq.item;
		}
		return null;
	}

	public Item GetTopItem () {
		Square sq = square.GetNeighborTop ();
		if (sq != null) {
			if (sq.item != null)
				return sq.item;
		}
		return null;
	}
}

[System.Serializable]
public class StripedItem {
	public Sprite horizontal;
	public Sprite vertical;
}
