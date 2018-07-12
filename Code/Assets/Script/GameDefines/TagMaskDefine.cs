using UnityEngine;
using System.Collections;

public class TagMaskDefine
{
	public const string UNTAGGED = "Untagged";
	public const string MAIN_CAMERA = "MainCamera";
	public const string GFAN_ACTOR = "Gfan_Actor";
	public const string GFAN_MONSTER = "Gfan_Monster";
	public const string GFAN_FIGHT_WARSHIP = "Gfan_FightWarship";
	public const string GFAN_NPC = "Gfan_Npc";
	
	public const string GFAN_BUILDING = "Gfan_Building";
	public const string GFAN_TRIGGER = "Gfan_Trigger";
	public const string GAN_2DSTAGE = "Gfan_2DStage";
	
	public static void SetTagRecuisively(GameObject go, string tag)
	{
		go.tag = tag;
		foreach (Transform tf in go.transform)
		{
			SetTagRecuisively(tf.gameObject, tag);
		}
	}
}
