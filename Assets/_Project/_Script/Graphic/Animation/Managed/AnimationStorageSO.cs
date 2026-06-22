using System.Collections.Generic;
using UnityEngine;

namespace Zoomer.Graphic.Animation
{
	[CreateAssetMenu(fileName = "Animation Storage", menuName = "Zoomer SO/Animation/Storage")]
	public sealed class AnimationStorageSO : ScriptableSingleton<AnimationStorageSO>
	{
		[SerializeField] private CharacterAnimationSO[] _charAnimConfigs;
		[SerializeField] private Material _sharedMaterial;
		public static Material SharedMaterial => Instance._sharedMaterial;
		public static Dictionary<EntityId, CharacterAnimationSO> CharAnimConfigDict { get; private set; }
		public static int CharAnimationCount => CharAnimConfigDict.Count;

		protected override void OnEnable()
		{
			base.OnEnable();
			CharAnimConfigDict = new(_charAnimConfigs.Length);
			foreach (var charAnim in _charAnimConfigs)
			{
				CharAnimConfigDict[charAnim.GetEntityId()] = charAnim;
			}
		}
	}
}
