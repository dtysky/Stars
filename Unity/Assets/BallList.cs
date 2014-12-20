using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BallList{
	public static BallList instance;
	public List<Ball> balllist;
	
	public static BallList GetInstance(){
		if (instance == null)
			instance = new BallList ();
		return instance;
	}

	BallList(){
		balllist = new List<Ball>();
	}
	
}