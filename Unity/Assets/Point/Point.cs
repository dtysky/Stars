using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Point : MonoBehaviour {

	private SocketInstance Skt;

	public int resolution = 30;
	
	private int currentResolution;
	private ParticleSystem.Particle[] points;
	
	Dictionary<string,float> PointPosition=new Dictionary<string, float>();
	Dictionary<string,float> PointForce=new Dictionary<string, float>();

	private void InitDict(Dictionary<string,float> Dict){
		Dict.Add ("x", 0f);
		Dict.Add ("y", 0f);
		Dict.Add ("z", 0f);
	}

	private void ResetDict(Dictionary<string,float> Dict){
		Dict.Clear ();
		InitDict(Dict);
	}

	private void CreatePoint () {
		if (resolution < 10 || resolution > 100) {
			Debug.LogWarning("Grapher resolution out of bounds, resetting to minimum.", this);
			resolution = 10;
		}
		currentResolution = resolution;
		points = new ParticleSystem.Particle[resolution];
		InitDict (PointForce);
		InitDict (PointPosition);
		points[0].position = new Vector3(PointPosition["x"], PointPosition["y"], PointPosition["z"]);
		points[0].color = new Color(10f, 10f, 10f);
		points[0].size = 0.5f;
	}

	void Start () {
		Skt = SocketInstance.GetInstance ();
	}

	Random ra=new Random();
	int i=0;
	void Update () {
		if (currentResolution != resolution || points == null) {
			CreatePoint();
		}
		RefForce (Skt.Get ());
		rigidbody.AddForce(new Vector3(PointForce["x"], PointForce["y"], PointForce["z"]));
		particleSystem.SetParticles(points, points.Length);
	}

	public Dictionary<string,float> GetPosition(){
		return PointPosition;
	}

	public void RefForce(Dictionary<string,float> UserForce){
		foreach (string key in UserForce.Keys){
			PointForce[key]=UserForce[key];
		}
	}

}