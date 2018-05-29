using UnityEngine;

// TODO - Make abstract? Should be pickable
public class Food : MonoBehaviour 
{

	[SerializeField] protected float totalFoodValue = 30f;
	[SerializeField] private ParticleSystem _particleSystem;

	private float _foodValueLeft;

	private Vector3 _currentScale;
	
	void Start ()
	{
		_foodValueLeft = totalFoodValue;
		_currentScale = transform.localScale;
	}
	
	void Update () {
		
	}

	public float RemovePart(float percentage = 1f)
	{
		float eatenValue = CalculateEatenValue(percentage);
		
		SubtractFoodValue(eatenValue);
		
		ApplyNewScale();

		PlayVfx();

		return eatenValue;
	}

	public void PlayVfx()
	{
		_particleSystem.Emit(1);
	}

	private void SubtractFoodValue(float eatenValue)
	{
		_foodValueLeft -= eatenValue;
		_foodValueLeft = Mathf.Max(0, _foodValueLeft);
	}

	private float CalculateEatenValue(float percentage)
	{
		percentage = Mathf.Clamp(percentage, Mathf.Epsilon, 1f);
		return totalFoodValue * percentage;
	}

	protected void ApplyNewScale()
	{
		transform.localScale = _currentScale * _foodValueLeft / totalFoodValue;
	}

	public bool IsEmpty()
	{
		return Mathf.Approximately(0, _foodValueLeft);
	}

	public void Delete()
	{
		WorldController.Instance.RemoveFood(this);
		Invoke("RemoveFromWorld", _particleSystem.main.duration);
	}

	private void RemoveFromWorld()
	{
		Destroy(gameObject);
	}
}
