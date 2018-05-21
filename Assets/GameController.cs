using UnityEngine;
using UnityEngine.AI;

public class GameController : MonoBehaviour {

	public const int TICK_PER_SECOND = 20;
	private float _timeBetweenUpdates;
	private float _elapsedTimeFromLastUpdate;

	[SerializeField] private Snail _snail;
	[SerializeField] private NavMeshSurface _navMesh;
	
	void Start ()
	{
		_timeBetweenUpdates = 1f / TICK_PER_SECOND;
		_elapsedTimeFromLastUpdate = _timeBetweenUpdates;
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
		if (!_snail.HasTask() && _snail.IsHungry())
		{
			GameObject food = GameObject.FindWithTag("Food");
			if (food != null)
			{
				_snail.SetDestination(food.transform.position);
			}
		}
		
		_snail.TickUpdate();
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
}
