using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraS : MonoBehaviour {
	//音乐文件
	public AudioSource music;
	//音量
	public float musicVolume;	
	
	void Start() {
		//设置默认音量
		musicVolume = 1f;
		music.Play();
	}
}