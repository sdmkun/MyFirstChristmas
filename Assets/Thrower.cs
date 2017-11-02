using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thrower : MonoBehaviour {

	private Vector3 screenPoint;
	private Vector3 offset;
	private Rigidbody rigidbody;

	// Use this for initialization
	void Start () {
		rigidbody = GetComponent<Rigidbody>();
		// 投げるまではZ座標（奥行き方向）を固定
		rigidbody.constraints = RigidbodyConstraints.FreezePositionZ;
	}

	// Update is called once per frame
	void Update () {

	}

	void OnMouseDown () {
		// この辺コピペ
		// オブジェクトのスクリーン座標を取得してカーソルとのずれを測る
		screenPoint = Camera.main.WorldToScreenPoint (transform.position);
		offset = transform.position - Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
  }

	void OnMouseDrag () {
		// コピペ
		// カーソルの位置に移動させてずれを足し合わせる
		Vector3 currentScreenPoint = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
		Vector3 currentPosition = Camera.main.ScreenToWorldPoint (currentScreenPoint) + offset;
		transform.position = currentPosition;
	}

	void OnMouseUp () {

	}

}
