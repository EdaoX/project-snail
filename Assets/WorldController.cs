using UnityEngine;
using UnityEngine.AI;

public class WorldController : MonoBehaviour
{

	[SerializeField] private NavMeshSurface _navMesh;
	[SerializeField] private Snail[] _snails;
	
	void Start()
	{
		
	}
	
	public void TickUpdate () 
	{
		foreach (Snail snail in _snails)
		{
			snail.TickUpdate();
		}
	}

	public GameObject GetFoodNear(Vector3 position)
	{
		// TODO - Food should be parameterized (static class Tags?)
		return GameController.GetNearestWithTag(position, "Food");
	}

	public Vector3 GetNearbyWanderLocation(Vector3 position, float maxDistance = 1f)
	{
		Vector3 randomDirection = Random.insideUnitCircle * maxDistance;
		NavMeshHit hit;
		if (NavMesh.SamplePosition(position + randomDirection, out hit, maxDistance, 1))
		{
			return hit.position;
		}
		
		return Vector3.zero;
	}
}
