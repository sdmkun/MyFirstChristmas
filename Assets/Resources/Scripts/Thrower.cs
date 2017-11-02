using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thrower : MonoBehaviour {

	private Vector3 screenPoint;
	private Vector3 offset;
	private Vector3 startPoint;															// 投げる動きをはじめた座標
	private float startThrowingTime;												// 投げる動きをはじめた時間
	private Vector3 previousPoint;													// 投げるだけの動きがないかどうか確かめる始点
	private float previousThrowingTime;											// 投げるだけの動きがないかどうか確かめはじめる時間
	private Rigidbody rigidbody;

	[SerializeField] private float distanceToResetThrowing;	// 一定時間内に正のy方向にこの距離だけ動きがないときに投げる勢いをリセットする
	[SerializeField] private float timeToResetThrowing;			// この時間だけ殆ど動きが無いときに投げる勢いをリセットする
	[SerializeField] private float throwingHeightScale;			// 投げる時の勢いから計算する高さ方向のスケール
	[SerializeField] private float throwingVelocityRequired;// 投げるのに必要な勢い

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

		// 投げる動きを始めた座標と時間
		startPoint = transform.position;
		startThrowingTime = Time.time;
  }

	void OnMouseDrag () {
		// コピペ
		// カーソルの位置に移動させてずれを足し合わせる
		Vector3 currentScreenPoint = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
		Vector3 currentPosition = Camera.main.ScreenToWorldPoint (currentScreenPoint) + offset;
		transform.position = currentPosition;

		// 投げる勢いがあるかどうか計算する
		if(transform.position.y - previousPoint.y > distanceToResetThrowing){
			previousPoint = transform.position;
			previousThrowingTime = Time.time;
		}
		// 一定時間に投げる勢いがないときは勢いをリセットする
		if(Time.time - previousThrowingTime > timeToResetThrowing &&
					transform.position.y - previousPoint.y < distanceToResetThrowing){
			previousPoint = transform.position;
			previousThrowingTime = Time.time;
			startPoint = transform.position;
			startThrowingTime = Time.time;
		}
	}

	void OnMouseUp () {
		float vx = -(startPoint.x - transform.position.x) / (Time.time - startThrowingTime);
		float vy =  (startPoint.y - transform.position.y) / (Time.time - startThrowingTime) * (startPoint.y - transform.position.y) / throwingHeightScale;
		float vz = -(startPoint.y - transform.position.y) / (Time.time - startThrowingTime);
		Vector3 throwingVelocity = new Vector3(vx, vy, vz);
		if(throwingVelocity.magnitude < throwingVelocityRequired){
			return;
		}
		if(throwingVelocity.z < 0.0f){
			return;
		}
		rigidbody.constraints = RigidbodyConstraints.None;
		rigidbody.velocity = new Vector3(vx, vy, vz);
	}

}
