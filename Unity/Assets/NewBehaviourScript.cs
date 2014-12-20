using UnityEngine;
using System.Collections;


public class NewBehaviourScript : MonoBehaviour {
	public SocketInstance m;

	// Use this for initialization
	void Start () {
		m = SocketInstance.GetInstance ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
