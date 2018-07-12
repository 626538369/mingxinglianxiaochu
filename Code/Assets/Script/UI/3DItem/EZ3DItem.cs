using UnityEngine;
using System.Collections;

public abstract class EZ3DItem : MonoBehaviour 
{
	public abstract void SetValue(params object[] args);
	public abstract void Reposition(Vector3 worldPos);
}

public class EZ3DItemData
{}
