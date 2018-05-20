using UnityEngine;

public class SnailController : MonoBehaviour {

	const int TICK_PER_SECOND = 20;
	private float _timeBetweenUpdates;
	private float _elapsedTimeFromLastUpdate;
	
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
		print("Logic");
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
