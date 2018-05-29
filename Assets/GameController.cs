using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameController : MonoBehaviour {

	public const int TICK_PER_SECOND = 20;
	private float _timeBetweenUpdates;
	private float _elapsedTimeFromLastUpdate;

	[SerializeField] private GameObject _snailPrefab;
	
	void Start ()
	{
		_timeBetweenUpdates = 1f / TICK_PER_SECOND;
		_elapsedTimeFromLastUpdate = _timeBetweenUpdates;

		GameObject snail = Instantiate(_snailPrefab);
		snail.transform.position = WorldController.GetNearbyWanderLocation(Vector3.zero, 10f);
		WorldController.Instance.AddSnail(snail.GetComponent<Snail>());
	}
	
	void Update ()
	{
		ComputeTick();
		
		if (!ShouldUpdate()) return;
		
		ExecuteLogic();
		ResetTick();
	}

	private void ExecuteLogic()
	{
		WorldController.Instance.TickUpdate();
	}

	private void ComputeTick()
	{
		_elapsedTimeFromLastUpdate = Mathf.Min(_elapsedTimeFromLastUpdate + Time.deltaTime, _timeBetweenUpdates);
	}

	private bool ShouldUpdate()
	{
		return (_timeBetweenUpdates - _elapsedTimeFromLastUpdate) < Mathf.Epsilon;
	}

	private void ResetTick()
	{
		_elapsedTimeFromLastUpdate = 0;
	}

	public static T GetNearest<T>(Vector3 position, IEnumerable<T> objects) where T : Component 
	{
		float minDistance = Mathf.Infinity;
		T closestObject = default(T);
		
		foreach (T currentObject in objects)
		{
			// Using sqrMagnitude for slightly better performance
			float approxDistance = (position - currentObject.transform.position).sqrMagnitude;
			
			if (approxDistance < minDistance)
			{
				minDistance = approxDistance;
				closestObject = currentObject;
			}
		}

		return closestObject;
	}
}
