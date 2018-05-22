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
	
}
