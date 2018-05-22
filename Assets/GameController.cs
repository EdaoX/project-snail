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

	void ExecuteLogic()
	{
		_worldController.TickUpdate();
	}

	void ComputeTick()
	{
		_elapsedTimeFromLastUpdate = Mathf.Min(_elapsedTimeFromLastUpdate + Time.deltaTime, _timeBetweenUpdates);
	}

	bool ShouldUpdate()
	{
		return (_timeBetweenUpdates - _elapsedTimeFromLastUpdate) < Mathf.Epsilon;
	}

	void ResetTick()
	{
		_elapsedTimeFromLastUpdate = 0;
	}

	public static GameObject GetNearestWithTag( Vector3 position, string tag )
	{
		GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
		
		float minDistance = Mathf.Infinity;
		GameObject closestObject = null;
		
		foreach (GameObject currentObject in objects)
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
