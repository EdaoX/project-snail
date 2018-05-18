using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ClickMove : MonoBehaviour
{

	[SerializeField] Camera _camera;
	[SerializeField] NavMeshAgent _agent;
	
	void Update () {
		if (Input.GetMouseButtonDown(0))
		{
			Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;

			if (Physics.Raycast(ray, out hit))
			{
				_agent.SetDestination(hit.point);
			}
			
		}
	}
}
