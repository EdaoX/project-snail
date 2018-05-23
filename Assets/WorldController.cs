using UnityEngine;
using UnityEngine.AI;

public class WorldController : MonoBehaviour
{

	[SerializeField] private NavMeshSurface _navMesh;
	[SerializeField] private Snail[] _snails;
	
	public void TickUpdate () 
	{
		foreach (Snail snail in _snails)
		{
			snail.TickUpdate();
		}
	}

	public static Food GetFoodNear(Vector3 position)
	{
		// TODO - Don't query: keep an internal array of food items
		Food[] foods = FindObjectsOfType<Food>();
		return GameController.GetNearest<Food>(position, foods);
	}

	public static Vector3 GetNearbyWanderLocation(Vector3 position, float maxDistance = 1f)
	{
		Vector3 randomDirection = Random.insideUnitCircle * maxDistance;
		NavMeshHit hit;
		return NavMesh.SamplePosition(position + randomDirection, out hit, maxDistance, 1) ? hit.position : Vector3.zero;
	}
}
