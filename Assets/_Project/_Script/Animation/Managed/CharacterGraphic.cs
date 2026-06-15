using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Zoomer.Animation
{
    public sealed class CharacterGraphic : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
		[NonSerialized] public new Transform transform;

		void Awake()
		{
			transform = base.transform;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void SetSprite(Sprite sprite)
		{
			_spriteRenderer.sprite = sprite;
		}
    }
}
