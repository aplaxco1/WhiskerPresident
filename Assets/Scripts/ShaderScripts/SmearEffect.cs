using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SmearEffect : MonoBehaviour
{
	Queue<Vector3> _recentPositions = new Queue<Vector3>();
	Queue<Vector3> _recentRotations = new Queue<Vector3>();

	[SerializeField]
	int _frameLag = 0;

	Material _smearMat = null;
	public Material smearMat
	{
		get
		{
			if (!_smearMat)
				_smearMat = GetComponent<Renderer>().material;

			if (!_smearMat.HasProperty("_PrevPosition"))
				_smearMat.shader = Shader.Find("Custom/Smear");

			return _smearMat;
		}
	}

	void Update()
	{
		_frameLag = (int) Mathf.Floor(1/(Time.deltaTime*12));
		//Debug.Log(1/Time.deltaTime);
		if(_recentPositions.Count > _frameLag)
			smearMat.SetVector("_PrevPosition", _recentPositions.Dequeue());
		if(_recentRotations.Count > _frameLag)
			smearMat.SetVector("_PrevRotation", _recentRotations.Dequeue());
		_recentPositions.Enqueue(transform.position);
		smearMat.SetVector("_Position", transform.position);
		Vector3 rot = transform.parent.eulerAngles;
		//rot = new Vector3(-fix(rot.y),fix(rot.x),-fix(rot.z));
		rot = new Vector3(0,fix(rot.x),0);
		smearMat.SetVector("_Rotation", rot);
		//Debug.Log(rot);
		_recentRotations.Enqueue(rot);
	}
	float fix(float n) {
		float a = n <= 180 ? n : (360-n)*-1;
		return a;
	}
}