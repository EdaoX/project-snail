using UnityEngine;
using UnityEngine.AI;

public class WorldController : MonoBehaviour
{

	[SerializeField] private NavMeshSurface _navMesh;
	[SerializeField] private Snail _snail;
	
	void Start()
	{
		
	}
	
	public void TickUpdate () 
	{
		if (!_snail.HasTask() && _snail.IsHungry())
		{
			GameObject food = GameObject.FindWithTag("Food");
			if (food != null)
			{
				_snail.SetDestination(food.transform.position);
			}
		}
		
		_snail.TickUpdate();
	}
}
