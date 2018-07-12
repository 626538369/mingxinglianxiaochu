using UnityEngine;
using System.Collections;
using GameData.PlayerImageData;

public class CharacterSetPositionFromHeight : MonoBehaviour {

	public bool isNpc = false;

	void Start () {

		if(!isNpc)
		{
			UpdateCharacterPosition();
		}
	}

	public void UpdateCharacterPosition()
	{
		string roleAppearance = Globals.Instance.MGameDataManager.MActorData.starData.nRoleAppearance;
		
		PlayerImageData playerImageData = new PlayerImageData();
		playerImageData.ParserCustomData(roleAppearance);
		
		float defaultHeight = 165f;
		float height = playerImageData.nBodyHeight ;
		
		float positionY = -1f+(height - defaultHeight) * 0.0030303f;
		this.transform.localPosition = new Vector3(-1f,positionY , -1f);
	}

	public float UpdateCharacterPosition_Npc( string nRoleAppearance )
	{
		string roleAppearance = nRoleAppearance;
		
		PlayerImageData playerImageData = new PlayerImageData();
		playerImageData.ParserCustomData(roleAppearance);
		
		float defaultHeight = 165f;
		float height = playerImageData.nBodyHeight ;
		
		float positionY = 0.01f+(height - defaultHeight) * 0.0030303f;
		this.transform.localPosition = new Vector3(-1f,positionY , -1f);
		return positionY;
	}
}
