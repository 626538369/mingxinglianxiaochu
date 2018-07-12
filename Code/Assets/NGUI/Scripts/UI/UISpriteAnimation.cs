//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2014 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Very simple sprite animation. Attach to a sprite and specify a common prefix such as "idle" and it will cycle through them.
/// </summary>

[ExecuteInEditMode]
[RequireComponent(typeof(UISprite))]
[AddComponentMenu("NGUI/UI/Sprite Animation")]
public class UISpriteAnimation : MonoBehaviour
{
	[HideInInspector][SerializeField] int mFPS = 30;
	[HideInInspector][SerializeField] string mPrefix = "";
	[HideInInspector][SerializeField] bool mLoop = true;
	[HideInInspector][SerializeField] bool mPingPong = false;
	[HideInInspector][SerializeField] bool mMakePixelPerfect = true;

	UISprite mSprite;
	float mDelta = 0f;
	int mIndex = 0;
	bool mActive = true;
	List<string> mSpriteNames = new List<string>();

	int vector = 1;

	/// <summary>
	/// Number of frames in the animation.
	/// </summary>

	public int frames { get { return mSpriteNames.Count; } }

	/// <summary>
	/// Animation framerate.
	/// </summary>

	public int framesPerSecond { get { return mFPS; } set { mFPS = value; } }

	/// <summary>
	/// Set the name prefix used to filter sprites from the atlas.
	/// </summary>

	public string namePrefix { get { return mPrefix; } set { if (mPrefix != value) { mPrefix = value; RebuildSpriteList(); } } }

	/// <summary>
	/// Set the animation to be looping or not
	/// </summary>

	public bool loop { get { return mLoop; } set { mLoop = value; } }
	/// <summary>
	/// Set the animation to be pingPong or not
	/// </summary>
	/// <value><c>true</c> if ping pong; otherwise, <c>false</c>.</value>
	public bool pingPong { get { return mPingPong; } set { mPingPong = value; } }

	public bool makePixelPerfect { get { return mMakePixelPerfect; } set { mMakePixelPerfect = value; } }

	/// <summary>
	/// Returns is the animation is still playing or not
	/// </summary>

	public bool isPlaying { get { return mActive; } }

	/// <summary>
	/// Rebuild the sprite list first thing.
	/// </summary>

	void Start () { RebuildSpriteList(); }

	/// <summary>
	/// Advance the sprite animation process.
	/// </summary>

	void Update ()
	{
		if (mActive && mSpriteNames.Count > 1 && Application.isPlaying && mFPS > 0f)
		{
			if(pingPong)
			{
				mDelta += RealTime.deltaTime;
				float rate = 1f / mFPS;

				if (rate < mDelta)
				{
					mDelta = (rate > 0f) ? mDelta - rate : 0f;
					mIndex += vector;
					if (mIndex >= mSpriteNames.Count-1)
					{
						mIndex = mSpriteNames.Count -1;
						vector = -1;
						mActive = loop;
					}
					if(mIndex <= 0)
					{
						mIndex = 0;
						vector = 1;
						mActive = loop;
					}

					if (mActive)
					{
						mSprite.spriteName = mSpriteNames[mIndex];
						if(makePixelPerfect)
						{
							mSprite.MakePixelPerfect();
						}
					}
				}
			}
			else
			{
				mDelta += RealTime.deltaTime;
				float rate = 1f / mFPS;
				
				if (rate < mDelta)
				{
					
					mDelta = (rate > 0f) ? mDelta - rate : 0f;
					if (++mIndex >= mSpriteNames.Count)
					{
						mIndex = 0;
						mActive = loop;
					}
					
					if (mActive)
					{
						mSprite.spriteName = mSpriteNames[mIndex];
						if(makePixelPerfect)
						{
							mSprite.MakePixelPerfect();
						}
					}
				}
			}
		}
	}

	/// <summary>
	/// Rebuild the sprite list after changing the sprite name.
	/// </summary>

	void RebuildSpriteList ()
	{
		if (mSprite == null) mSprite = GetComponent<UISprite>();
		mSpriteNames.Clear();

		if (mSprite != null && mSprite.atlas != null)
		{
			List<UISpriteData> sprites = mSprite.atlas.spriteList;

			for (int i = 0, imax = sprites.Count; i < imax; ++i)
			{
				UISpriteData sprite = sprites[i];

				if (string.IsNullOrEmpty(mPrefix) || sprite.name.StartsWith(mPrefix))
				{
					mSpriteNames.Add(sprite.name);
				}
			}
			mSpriteNames.Sort();
		}
	}
	
	/// <summary>
	/// Reset the animation to frame 0 and activate it.
	/// </summary>
	
	public void Reset()
	{
		mActive = true;
		mIndex = 0;

		if (mSprite != null && mSpriteNames.Count > 0)
		{
			mSprite.spriteName = mSpriteNames[mIndex];
			if(makePixelPerfect)
			{
				mSprite.MakePixelPerfect();
			}
		}
	}
}
