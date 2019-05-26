using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attractor : MonoBehaviour {

	const float G = 6.774f;
	public Rigidbody rb;
	private Vector3 origin;
	private bool move;

	private void Start()
	{
		origin = this.transform.position;
		if (this.name == "BlackHole") {
			move = true;
		} else {
			move = false;
		}
	}


	void FixedUpdate(){
		if(this.name == "BlackHole"){
			Attractor[] attractors = FindObjectsOfType<Attractor> ();
			foreach(Attractor at in attractors){
				if (at != this && at.move) {
					Attract (at);
				}
			}
		}
	}

	void Attract (Attractor obToAttract){

		Rigidbody rbToAttract = obToAttract.rb;
		Vector3 direction = rb.position - rbToAttract.position;
		float distance = direction.magnitude;

		float forceMag = G*(rb.mass * rbToAttract.mass) / Mathf.Pow (distance, 2);
		Vector3 force = direction.normalized * forceMag;

		rbToAttract.AddForce (force);
	}

	private void OnTriggerEnter(Collider collider)
	{
		if (collider.name == "BlackHole") 
		{
			collider.attachedRigidbody.velocity = Vector3.zero;
			StartCoroutine (gotEaten());
		}
	}

	public void letsMove(){
		this.move = true;
	}

	IEnumerator gotEaten(){
		doNotMove ();
		rb.velocity = Vector3.zero;
		rb.angularVelocity = Vector3.zero;
		yield return new WaitForSeconds (3);
		this.transform.position = origin;
	}

	public void doNotMove(){
		this.move = false;
	}
}
