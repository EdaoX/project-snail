using UnityEngine;
using UnityEngine.AI;

public class Snail : MonoBehaviour
{
	protected const int HUNGER_THRESHOLD = 30;
	
	protected Vector3 currentDestination;
	protected NavMeshAgent agent;
	private bool _hasTask = false;
	
	// TODO - Remove from inspector
	[SerializeField] [Range(0, 100)] private float _fullness = 100f;
	[Tooltip("% per second")] [SerializeField] private float _hungerSpeed = 0.1f; // Hunger per second

	// Pre-calculated for perfomance
	private float _hungerFactor = 1f / GameController.TICK_PER_SECOND;

	void Start()
	{
		currentDestination = transform.position;
		agent = GetComponent<NavMeshAgent>();
	}
	
	public void SetDestination(Vector3 destination)
	{
		currentDestination = destination;
	}

	public void Update()
	{
	}

	public void TickUpdate()
	{
		ProgressHunger();
		
		HandleNeeds();
		
		CheckNewDestination();

		if (IsAtDestination())
			_hasTask = false;
	}

	protected void CheckNewDestination()
	{
		if (currentDestination != agent.destination)
		{
			agent.SetDestination(currentDestination);
			_hasTask = true;
		}
	}

	private void ProgressHunger()
	{
		_fullness = Mathf.Max(0, _fullness - _hungerSpeed * _hungerFactor);
	}

	public bool HasTask()
	{
		return _hasTask;
	}

	public bool IsHungry()
	{
		return _fullness <= HUNGER_THRESHOLD;
	}

	public bool IsAtDestination()
	{
		return agent.remainingDistance <= agent.stoppingDistance;
	}

	private void OnCollisionEnter(Collision other)
	{
		switch (other.gameObject.tag)
		{
			case "Food" : // TODO - Food should be parameterized (static class Tags?)
				// TODO - fullness info should belong to a Food class
				_fullness = Mathf.Min(100, _fullness + 20);
				Destroy(other.gameObject);
				break;
		}
	}

	protected void HandleNeeds()
	{
		if (IsHungry())
		{
			GameObject nearbyFood = GameController.WorldController.GetFoodNear(transform.position);
			if (nearbyFood != null)
				SetDestination(nearbyFood.transform.position);
		}
	}
}
