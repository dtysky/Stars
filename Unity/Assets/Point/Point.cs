using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Point : MonoBehaviour {

	private SocketInstance Skt;

	public int resolution = 30;

	BallList Bl; 
	
	private int currentResolution;
	private ParticleSystem.Particle[] points;
	
	private float[] PointPosition={0,0,0};
	private float[] PointForce={0,0,0};
	private float PointMass=0;

	private Vector3 last_point=new Vector3();	
	private Vector3 now_point = new Vector3();
	Vector3 velocity = new Vector3();

	private void Init(){
		for (int i=0; i<3; i++) {
			PointPosition[i]=0;
			PointForce[i]=0;
		}
	}
	
	private void CreatePoint () {
		if (resolution < 10 || resolution > 100) {
			Debug.LogWarning("Grapher resolution out of bounds, resetting to minimum.", this);
			resolution = 10;
		}
		Init ();
		Skt = SocketInstance.GetInstance ();
		currentResolution = resolution;
		points = new ParticleSystem.Particle[resolution];
		points[0].position = new Vector3(PointPosition[0], PointPosition[1], PointPosition[2]);
		points[0].color = new Color(255f, 255f, 255f);
		points[0].size = 0.5f;
		}

	void Start () {
		CreatePoint ();
	}

	void FixedUpdate(){
//		now_point.Set(this.GetPosition ()[0],this.GetPosition ()[1],this.GetPosition ()[2]);
//		velocity = now_point - last_point;
//		Debug.Log (velocity.magnitude.ToString());
//		last_point = now_point;
		var locVel = transform.InverseTransformDirection(rigidbody.velocity);
		locVel = 10 * locVel.normalized;
		velocity = rigidbody.velocity = transform.TransformDirection(locVel);

		//velocity = rigidbody.velocity = velocity.normalized * 10;
		Debug.Log (velocity.ToString());
	}

	void Update () {
		if (currentResolution != resolution || points == null) {
			CreatePoint();
		}

		float[] user_force_float = Skt.Get ();
		Vector3 user_force_vector = new Vector3 (user_force_float [0], user_force_float [1], user_force_float [2]);
		user_force_vector = user_force_vector - user_force_vector.magnitude * velocity.normalized ;

		RefForce();
		Vector3 force = new Vector3 (PointForce [0], PointForce [1], PointForce [2]);
		force = force + user_force_vector;	
		rigidbody.AddForce(force);
		particleSystem.SetParticles(points, points.Length);


	}

	public float[] GetPosition(){
		return PointPosition;
	}
	
	private float[] V32Float(Vector3 V3){
		float[] Fl={0,0,0};
		Fl [0] = V3.x;
		Fl [1] = V3.y;
		Fl [2] = V3.z;
		return Fl;
	}

	float[] GetGravity(float M, float m, float[] v1, float[] v2){
		float[] z = {v2[0]-v1[0], v2[1]-v1[1], v2[2]-v1[2]};
		float norm = Mathf.Pow (z[0], 2) + Mathf.Pow (z[1], 2) + Mathf.Pow (z[2], 2);
		float n1 = Mathf.Floor (norm);
		float n2 = norm - n1;
		n1 = 1 / n1;
		float r = n1 - Mathf.Pow (n1, 2) * n2 + Mathf.Pow (n1, 3) * Mathf.Pow (n2, 2);
		float F = M * m * r * r;
		z [0] /= norm; z [1] /= norm; z [2] /= norm;
		return z;
	}

	public void RefForce(){			
		Bl = BallList.GetInstance ();
		PointMass = this.rigidbody.mass;
		PointPosition = V32Float(this.transform.position);
		float[] BallPosition = {0,0,0};
		float[] BallForce = {0,0,0};
		float BallMass = 0;
		foreach (Ball bl in Bl.balllist) {
			BallMass=bl.rigidbody.mass;
			BallPosition=V32Float(bl.transform.position);
			BallForce=GetGravity(PointMass,BallMass,PointPosition,BallPosition);
			for (int i=0;i<3;i++)
				PointForce[i]+=BallForce[i];
		}
		for (int i=0; i<3; i++) {
			PointForce [i] = PointForce [i] * 0.8f;
		}
		return;
	}
}