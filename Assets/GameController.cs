using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(WorldController))]
public class GameController : MonoBehaviour {

	public const int TICK_PER_SECOND = 20;
	private float _timeBetweenUpdates;
	private float _elapsedTimeFromLastUpdate;
	
	private static WorldController _worldController;

	public static WorldController WorldController
	{
		get { return _worldController; }
	}


	void Start ()
	{
		_timeBetweenUpdates = 1f / TICK_PER_SECOND;
		_elapsedTimeFromLastUpdate = _timeBetweenUpdates;
		_worldController = GetComponent<WorldController>();
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
		_worldController.TickUpdate();
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
