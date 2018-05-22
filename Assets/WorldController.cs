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
		// TODO - Should return closest food to position
		GameObject food = GameObject.FindWithTag("Food"); // TODO - Food should be parameterized (static class Tags?)
		return food;
	}
}
