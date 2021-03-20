using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringBody : MonoBehaviour
{
	public Transform top_part;
	public Transform top_stabilizer;
	public Transform root_stabilizer;
	Vector3 stabPOS = new Vector3();
	Vector3 stabVEL = new Vector3();
	public float mass = 1;


	public Vector3 localEulerAngles
	{
		get { return top_part.localEulerAngles; }
	}
	void OnEnable()
	{
		stabPOS = transform.position;
	}

	void Update()
	{
		
		stabPOS = Vector3.Lerp(stabPOS, root_stabilizer.position, Time.deltaTime * 10 * mass) + stabVEL * 10 * mass * Time.deltaTime;
		top_stabilizer.position = stabPOS;
		top_part.rotation = Quaternion.Euler( new Vector3((top_stabilizer.localPosition.z - root_stabilizer.localPosition.z), -(top_stabilizer.localPosition.x - root_stabilizer.localPosition.x), -(top_stabilizer.localPosition.x - root_stabilizer.localPosition.x)) * 20 * mass);
		stabVEL = Vector3.Lerp(stabVEL, root_stabilizer.position - stabPOS, Time.deltaTime * 15);
		
	}
}