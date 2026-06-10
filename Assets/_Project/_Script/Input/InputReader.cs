using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Zoomer.Input
{
	public class InputReader : InputSystem.IPlayerActions
	{		
		private Entity inputEntity;
		private InputData inputData;

		private InputSystem inputSystem;
		private static InputReader instance;
		
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
		private static void Initialize()
		{
			instance = new InputReader();
		}

		private InputReader()
		{
			inputSystem = new InputSystem();
			inputSystem.Player.SetCallbacks(this);
			inputSystem.Player.Enable();

			inputEntity = World.DefaultGameObjectInjectionWorld.EntityManager.CreateEntity(typeof(InputData));
		}

		public void OnMove(InputAction.CallbackContext context)
		{
			inputData.MoveDirection = context.ReadValue<Vector2>();
			World.DefaultGameObjectInjectionWorld.EntityManager.SetComponentData(inputEntity, inputData);
		}

		public void OnAttack(InputAction.CallbackContext context)
		{
			throw new System.NotImplementedException();
		}

	}
}
