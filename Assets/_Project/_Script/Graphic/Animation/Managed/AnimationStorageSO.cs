using System.Collections.Generic;
using UnityEngine;

namespace Zoomer.Graphic.Animation
{
	[CreateAssetMenu(fileName = "Animation Storage", menuName = "Zoomer SO/Animation/Storage")]
	public sealed class AnimationStorageSO : ScriptableSingleton<AnimationStorageSO>
	{
		[SerializeField] private CharacterAnimationConfigSO[] _charAnimConfigs;
		public static Dictionary<EntityId, CharacterAnimationConfigSO> CharAnimConfigDict { get; private set; }
		public static int CharAnimationCount => CharAnimConfigDict.Count;

		void OnEnable()
		{
			CharAnimConfigDict = new(_charAnimConfigs.Length);
			foreach (var charAnim in _charAnimConfigs)
			{
				CharAnimConfigDict[charAnim.GetEntityId()] = charAnim;
			}
		}
	}
}
