using InputSystem;
using InputSystem.InputProfiles;
using UnityEngine;
using Zenject;


public class Test : MonoBehaviour
{
	private float _speed = 5f;
	
	[Inject]
	private void Construct(InputProvider inputProvider)
	{
		JoyStickInput input = inputProvider.GetProfile(typeof(JoyStickInput)) as JoyStickInput;

		input.OnMoveEvent += Move;
	}

	private void Move(Vector2 direction)
	{
		Debug.Log("move");
		
		Vector3 offSet = new Vector3(direction.x * Time.fixedDeltaTime * _speed, 0, direction.y * Time.fixedDeltaTime * _speed);
		Vector3 newPosition = transform.position + offSet;
		transform.position = newPosition;
	}
}