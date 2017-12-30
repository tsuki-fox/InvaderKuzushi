using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Core
{
	public delegate void OnCleanedHandler();
	public delegate void OnInitializedHandler();
	public delegate void OnReleasedHandler();

	public interface ICore
	{
		event OnCleanedHandler onCleaned;
		event OnInitializedHandler onInitialized;
		event OnReleasedHandler onReleased;

		void Clean();
		void Initialize();
	}
}