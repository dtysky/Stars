using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
		float x, y, z, si, kg;
		x = Random.Range (-50, 50)*1f;
		y = Random.Range (-50, 50)*1f;
		z = Random.Range (-50, 50)*1f;
		si = Random.Range (1, 3);
		kg = si * 5;
		this.transform.position = new Vector3 (x, y, z);
		this.rigidbody.mass = kg;
		//this.transform.localScale.Set (si, si, si);
		BallList m;
		m = BallList.GetInstance ();
		m.balllist.Add (this);
	}
	// Update is called once per frame
	void Update () {
	
	}
}
