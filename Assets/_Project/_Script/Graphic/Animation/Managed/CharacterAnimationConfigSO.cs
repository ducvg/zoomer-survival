using System;
using UnityEngine;

namespace Zoomer.Graphic.Animation
{
	[CreateAssetMenu(fileName = "Character Animation", menuName = "Zoomer SO/Animation/Character Animation")]
    public sealed class CharacterAnimationConfigSO : ScriptableObject
    {
		[field: SerializeField] public ActionAnimationConfig[] Actions;
		public ActionAnimationConfig this[ActionKind actionKind] => Actions[(byte)actionKind];

		#if UNITY_EDITOR
		private void OnValidate()
		{
			Array.Sort(Actions, (a, b) => a.ActionKind.CompareTo(b.ActionKind));
			foreach(var actionAnims in Actions) actionAnims.ActionName = actionAnims.ActionKind.ToString();
		}
		#endif

		
		[Serializable]
		public class ActionAnimationConfig
		{
			#if UNITY_EDITOR
			[HideInInspector] public string ActionName;
			#endif
			
			public ActionKind ActionKind;
			public int Fps;
			public Sprite[] Frames;
		} 
    }
}
