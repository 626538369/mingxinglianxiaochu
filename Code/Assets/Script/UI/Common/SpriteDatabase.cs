using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpriteDatabase : ScriptableObject 
{
	public List<Sprite> sprites;
	
	Dictionary<string, Sprite> spriteDict_;
	
	public void reset()
	{
		spriteDict_ = null;
	}
	
	public Sprite find(string name)
	{
		if(spriteDict_ == null)
		{
			spriteDict_ = new Dictionary<string, Sprite>();
			foreach(var s in sprites)
				spriteDict_[s.name] = s;
		}
		if (!spriteDict_.ContainsKey(name))
			return null;
		return spriteDict_[name];
	}
}