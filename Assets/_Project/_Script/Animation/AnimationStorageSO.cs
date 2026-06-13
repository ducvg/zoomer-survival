using System;
using UnityEngine;

namespace Zoomer.Animation
{
    [CreateAssetMenu(fileName = "AnimationStorageSO", menuName = "Scriptable Objects/AnimationStorageSO")]
    public class AnimationStorageSO : ScriptableObject
    {
		[SerializeField] private AnimationConfig[] _actionAnimations;
		public AnimationConfig this[ActionKind actionKind] => _actionAnimations[(byte)actionKind];

		#if UNITY_EDITOR
		private void OnValidate()
		{
			Array.Sort(_actionAnimations, (a, b) => a.ActionKind.CompareTo(b.ActionKind));
		}
		#endif

		
		[Serializable]
		public class AnimationConfig
		{
			#if UNITY_EDITOR
			[SerializeField] private string _actionName;
			[SerializeField] public ActionKind ActionKind;
			#endif
			
			public int Fps;
			public Sprite[] Sprites;
		} 
    }
}
