using System;
using UnityEngine;

namespace Zoomer.Animation
{
    [CreateAssetMenu(fileName = "Animation Storage", menuName = "Scriptable Objects/Animation Storage")]
    public sealed class AnimationStorageSO : ScriptableObject
    {
		[SerializeField] private AnimationConfig[] _actionAnimations;
		public AnimationConfig this[ActionKind actionKind] => _actionAnimations[(byte)actionKind];

		#if UNITY_EDITOR
		private void OnValidate()
		{
			Array.Sort(_actionAnimations, (a, b) => a.ActionKind.CompareTo(b.ActionKind));
			foreach(var actionAnims in _actionAnimations) actionAnims.ActionName = actionAnims.ActionKind.ToString();
		}
		#endif

		
		[Serializable]
		public class AnimationConfig
		{
			#if UNITY_EDITOR
			[HideInInspector] public string ActionName;
			public ActionKind ActionKind;
			#endif
			
			public int Fps;
			public Sprite[] Sprites;
		} 
    }
}
