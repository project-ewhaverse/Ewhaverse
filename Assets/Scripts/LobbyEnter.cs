using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyEnter : MonoBehaviour
{
	public Launcher launcher;
    // Start is called before the first frame update
	void OnTriggerEnter(Collider other)
	{
		launcher.LeaveSquare();
	}
}
