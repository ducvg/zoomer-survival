using DVG.Pool;
using UnityEngine;

namespace Zoomer.Graphic
{
    public sealed class SpriteGraphicFactory : Singleton<SpriteGraphicFactory>
	{
		[SerializeField] private SpriteGraphic _characterGraphicPrefab;
		private ComponentPool<SpriteGraphic> _pool;

		protected override void Awake()
		{
			base.Awake();
			_pool = new ComponentPool<SpriteGraphic>(_characterGraphicPrefab, parent: transform, maxSize:-1);
		}

		public static SpriteGraphic Create()
		{
			var instance = Instance._pool.Rent();
			return instance;
		}

		public static void Return(SpriteGraphic instance)
		{
			Instance._pool.Return(instance);
		}
	}
}
