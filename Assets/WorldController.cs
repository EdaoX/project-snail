using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WorldController
{

	protected static WorldController instance;

	public static WorldController Instance
	{
		get { return instance ?? (instance = new WorldController()); }
	}

	protected NavMeshSurface navMesh;
	protected List<Snail> snails;
	protected List<Food> foods;

	private WorldController()
	{
		snails = new List<Snail>();
		foods = new List<Food>();
		navMesh = Object.FindObjectOfType<NavMeshSurface>(); // TODO - Change to some other way of associating navmesh
	}
	
	public void TickUpdate () 
	{
		foreach (Snail snail in snails)
		{
			snail.TickUpdate();
		}
	}

	public void AddSnail(Snail snail)
	{
		snails.Add(snail);
	}

	public void AddFood(Food food)
	{
		foods.Add(food);
	}

	public void RemoveFood(Food food)
	{
		foods.Remove(food);
	}
	
	public Food GetFoodNear(Vector3 position)
	{
		return GameController.GetNearest<Food>(position, foods);
	}

	public static Vector3 GetNearbyWanderLocation(Vector3 position, float maxDistance = 1f)
	{
		Vector3 randomDirection = Random.insideUnitCircle * maxDistance;
		NavMeshHit hit;
		return NavMesh.SamplePosition(position + randomDirection, out hit, maxDistance, 1) ? hit.position : Vector3.zero;
	}
}
