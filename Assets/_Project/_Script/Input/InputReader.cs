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

		private UnityInput _unityInput;
		private static InputReader instance;
		
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
		private static void Initialize()
		{
			instance = new InputReader();
		}

		private InputReader()
		{
			_unityInput = new UnityInput();
			_unityInput.Player.SetCallbacks(this);
			_unityInput.Player.Enable();

			_inputEntity = World.DefaultGameObjectInjectionWorld.EntityManager.CreateEntity(typeof(InputData));
		}

		public void OnMove(InputAction.CallbackContext context)
		{
			_inputData.MoveDirection = context.ReadValue<Vector2>();
			World.DefaultGameObjectInjectionWorld.EntityManager.SetComponentData(_inputEntity, _inputData);
		}

		public void OnAttack(InputAction.CallbackContext context)
		{
			throw new System.NotImplementedException();
		}

	}
}
