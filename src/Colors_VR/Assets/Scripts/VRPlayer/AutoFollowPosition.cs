using System.Collections.Generic;
using UnityEngine;

public class AutoFollowPosition : MonoBehaviour
{
	private List<Transform> autoFollowPositions;

	private void Awake()
	{
		autoFollowPositions = new List<Transform>();

		for (int i = 0; i < transform.childCount; ++i)
			autoFollowPositions.Add(transform.GetChild(i));
	}

	public Transform[] GetAutoFollowPositions()
	{
		return autoFollowPositions.ToArray();
	}
}
