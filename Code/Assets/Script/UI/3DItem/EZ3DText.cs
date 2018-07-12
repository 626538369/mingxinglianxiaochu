using UnityEngine;
using System.Collections;

public abstract class EZ3DText : MonoBehaviour
{
	public abstract void SetText(string text);
	public abstract void Reposition(Vector3 worldPos);
}
