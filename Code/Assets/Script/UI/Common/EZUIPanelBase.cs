using UnityEngine;
using System.Collections;

/// <summary>
/// EZUI panel base.
/// </summary>
namespace EZGUI.Common
{
	public abstract class EZUIPanelBase
	{
		protected abstract void InitInfo();
		public abstract void Destroy();
		public abstract void Update();
		
		public bool isRemove = false;
	}
}

   