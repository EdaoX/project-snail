﻿using UnityEngine;
using UnityEngine.AI;

public class Snail : MonoBehaviour
{
	protected const int HUNGER_THRESHOLD = 30;
	
	protected Food target; // TODO - Temporary, shouldn't be food but something like ITargetable
	protected NavMeshAgent agent;

	private bool _hasTask;

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

	// TODO - Remove from here, put into ITargetable
	[Tooltip("In meters")] [SerializeField] private float _reach = 1f;
	
	// TODO - Remove from inspector
	[SerializeField] [Range(0, 100)] private float _fullness = 100f;
	
	[Tooltip("% per second")] [SerializeField] private float _hungerSpeed = 0.1f;
	[Tooltip("In units")] [SerializeField] private float _maxWanderDistance = 5f;

	// Pre-calculated for perfomance
	private float _hungerFactor = 1f / GameController.TICK_PER_SECOND;

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
		_fullness = Mathf.Max(0, _fullness - _hungerSpeed * _hungerFactor);
	}

	public bool IsHungry()
	{
		return _fullness <= HUNGER_THRESHOLD;
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
			float newFullness = _fullness + food.Eat(.5f);
			_fullness = Mathf.Min(100, newFullness);
			if(food.IsEmpty())
				food.Delete();
			HasTask = false;
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
		{
			LookForFood();
			HasTask = true;
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
		Food nearbyFood = WorldController.GetFoodNear(transform.position);
		if (nearbyFood != null)
		{
			SetTarget(nearbyFood);
		}
	}
}
