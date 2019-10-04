using UnityEngine;
using System.Collections.Generic;

public class DestroyOnStart : MonoBehaviour
{
	void Start()
	{
        Destroy(gameObject);
	}
}
