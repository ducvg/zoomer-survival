using DVG.Pool;
using UnityEngine;

namespace Zoomer.Animation
{
    public sealed class CharacterGraphicFactory : Singleton<CharacterGraphicFactory>
	{
		[SerializeField] private CharacterGraphic _characterGraphicPrefab;
		private ComponentPool<CharacterGraphic> _pool;

		protected override void Awake()
		{
			base.Awake();
			_pool = new ComponentPool<CharacterGraphic>(_characterGraphicPrefab, parent: transform, maxSize:-1);
		}

		public static CharacterGraphic Create()
		{
			var instance = Instance._pool.Rent();
			return instance;
		}

		public static void Return(CharacterGraphic instance)
		{
			Instance._pool.Return(instance);
		}
	}
}
