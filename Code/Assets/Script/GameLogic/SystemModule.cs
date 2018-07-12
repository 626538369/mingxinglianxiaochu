using UnityEngine;
using System.Collections;

/// <summary>
/// System module base class.
/// </summary>
public abstract class SystemModule
{
	/// <summary>
	/// Initialize this instance.
	/// </summary>
	public abstract bool Initialize();
	
	/// <summary>
	/// Releases all resource used by the <see cref="SystemModule"/> object.
	/// </summary>
	/// <remarks>
	/// Call <see cref="Dispose"/> when you are finished using the <see cref="SystemModule"/>. The <see cref="Dispose"/>
	/// method leaves the <see cref="SystemModule"/> in an unusable state. After calling <see cref="Dispose"/>, you must
	/// release all references to the <see cref="SystemModule"/> so the garbage collector can reclaim the memory that the
	/// <see cref="SystemModule"/> was occupying.
	/// </remarks>
	public abstract void Dispose();
	
	/// <summary>
	/// Start the system with start information.
	/// </summary>
	/// <param name='startInfo'>
	/// Start information.
	/// </param>
	public abstract void Start(System.Object startInfo);
	
	/// <summary>
	/// Update this instance.
	/// </summary>
	public abstract void Update();
	
	/// <summary>
	/// Update engine gui.
	/// </summary>
	public abstract void OnGUI();
}
