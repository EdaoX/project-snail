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
	
	[Tooltip("% per second")] [SerializeField] private float _hungerSpeed = 0.1f;
	[Tooltip("In units")] [SerializeField] private float _maxWanderDistance = 5f;

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
		
		if(!HasTask())
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
		Food food = other.gameObject.GetComponent<Food>();
		if (food != null)
		{
			float newFullness = _fullness + food.Eat();
			_fullness = Mathf.Min(100, newFullness);
			if(food.IsEmpty())
				food.Delete();
		}
	}

	protected void HandleNeeds()
	{
		// TODO - Temporary, as it stops doing anything if it can't find food.
		// Should loop over possible tasks untill it finds a suitable one
		// LookForFood and WanderAround could be Task Class/Interface and
		// methods return true or false depending if it can be accomplished
		// Then if(Task.canAccomplish){ Task.accomplish(); _hasTask = true}
		if (IsHungry())
			LookForFood();
		else
			WanderAround();
	}

	public void WanderAround()
	{
		SetDestination(WorldController.GetNearbyWanderLocation(transform.position, _maxWanderDistance));
	}

	public void LookForFood()
	{
		Food nearbyFood = WorldController.GetFoodNear(transform.position);
		if (nearbyFood != null)
			SetDestination(nearbyFood.transform.position);
	}
}
