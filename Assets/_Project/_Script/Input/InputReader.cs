using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Zoomer.Input
{
	public class InputReader : UnityInput.IPlayerActions
	{		
		private Entity _inputEntity;
		private InputData _inputData;

		private EntityManager _entityManager;
		private UnityInput _unityInput;
		private static InputReader instance;
		
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
		private static void Initialize()
		{
			instance = new InputReader();
			SetWorld(World.DefaultGameObjectInjectionWorld);
		}

		private InputReader()
		{
			_unityInput = new UnityInput();
			_unityInput.Player.SetCallbacks(this);
			_unityInput.Player.Enable();
		}

		public static void SetWorld(World world)
		{
			ref var inputEntity = ref instance._inputEntity;
			ref var em = ref instance._entityManager;
			if(inputEntity != Entity.Null)
			{
				em.DestroyEntity(inputEntity);
			}

			em = world.EntityManager;
			inputEntity = em.CreateSingleton<InputData>();
		}

		public void OnMove(InputAction.CallbackContext context)
		{
			_inputData.MoveDirection = context.ReadValue<Vector2>();
			_entityManager.SetComponentData(_inputEntity, _inputData);
		}

		public void OnAttack(InputAction.CallbackContext context)
		{
			throw new System.NotImplementedException();
		}

	}
}
