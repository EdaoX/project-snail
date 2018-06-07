using UnityEngine;
using UnityEngine.AI;

public class Snail : MonoBehaviour
{
	// TODO - Remove from here, put into ITargetable
	[Tooltip("In meters")] [SerializeField] private float _reach = 1f;
	
	// TODO - Remove from inspector
	[SerializeField] [Range(0, 100)] private float _fullness = 100f;
	
	[SerializeField] [Range(0,100)] protected int hungerThreshold = 30;
	[Tooltip("% per second")] [SerializeField] private float _hungerSpeed = 0.1f;
	[Tooltip("In units")] [SerializeField] private float _maxWanderDistance = 5f;
	[SerializeField] protected Transform handAnchorPoint;
	
	protected Food target; // TODO - Temporary, shouldn't be food but something like ITargetable
	protected Food held; // TODO - Temporary, shouldn't be food but something like IHoldable
	protected NavMeshAgent agent;

	private bool _hasTask;
	private float _hungerFactor = 1f / GameController.TICK_PER_SECOND; // Pre-calculated for perfomance

	public bool HasTask
	{
		get { return _hasTask; }
		set
		{
			IsWandering = false;
			_hasTask = value;
		}
	}


	public bool IsWandering { get; protected set; }
	
	public float Fullness
	{
		get { return _fullness; }
		set { _fullness = Mathf.Clamp(value, 0, 100f); }
	}

	void Start()
	{
		agent = GetComponent<NavMeshAgent>();
	}
	
	
	// TODO - Make Properties
	public void SetDestination(Vector3 destination)
	{
		agent.isStopped = false;
		agent.destination = destination;
	}

	public void ClearDestination()
	{
		agent.isStopped = true;
	}

	public void TickUpdate()
	{
		ProgressHunger();
		
		if(!HasTask)
			HandleNeeds();
		
		CheckNearTarget();

		CheckReachedWanderingDestination();

	}

	private void CheckReachedWanderingDestination()
	{
		if (IsWandering && IsAtDestination())
			IsWandering = false;
	}

	protected void CheckNearTarget()
	{
		if (target != null)
		{
			if (Vector3.Distance(transform.position, target.transform.position) < _reach)
			{
				InteractWith(target);
				ClearTarget();
			}
		}
	}

	// TODO - Temporary
	// TODO - Make Properties
	public void SetTarget(Food food)
	{
		SetDestination(food.transform.position);
		target = food;
	}

	public void ClearTarget()
	{
		target = null;
		ClearDestination();
	}

	private void ProgressHunger()
	{
		Fullness -= _hungerSpeed * _hungerFactor;
	}

	public bool IsHungry()
	{
		return Fullness <= hungerThreshold;
	}

	public bool IsAtDestination()
	{
		return agent.remainingDistance <= agent.stoppingDistance;
	}

	// TODO - Temporary, should be generalized
	private void InteractWith(Food food)
	{
		if (food != null)
		{
			PickUp(food);
			HasTask = false;
		}
	}

	private void PickUp(Food food)
	{
		// Remove from ground objects to avoid other snails picking it up
		WorldController.Instance.PickFood(food);
		food.transform.parent = transform;
		food.transform.localPosition = handAnchorPoint.localPosition;
		held = food; // TODO - property so that above lines are implicit
	}

	protected void HandleNeeds()
	{
		// TODO - Temporary
		// Should loop over possible tasks untill it finds a suitable one
		// LookForFood and WanderAround could be Task Class/Interface and
		// methods return true or false depending if it can be accomplished
		// Then if(Task.canAccomplish){ Task.accomplish(); _hasTask = true}
		if (IsHungry())
		{
			if (held != null)
			{
				Fullness += held.RemovePart(.5f);
				if (held.IsEmpty())
				{
					held.Delete();
					held = null;
				}
			}
			else
			{
				LookForFood();
			}
		}
		else if (!IsWandering)
		{
			WanderAround();
		}
	}

	public void WanderAround()
	{
		SetDestination(WorldController.GetNearbyWanderLocation(transform.position, _maxWanderDistance));
		IsWandering = true;
	}

	public void LookForFood()
	{
		Food nearbyFood = WorldController.Instance.GetFoodNear(transform.position);
		if (nearbyFood != null)
		{
			SetTarget(nearbyFood);
			HasTask = true;
		}
	}
}
