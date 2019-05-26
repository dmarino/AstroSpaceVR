using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroids : MonoBehaviour {

	public Rigidbody rb;
	public bool tieneAgua;

	public TextMesh cometas;
	public TextMesh asteroides;
	public TextMesh meteoroides;
	public TextMesh planetoides;

	private Vector3 origin;
	private bool move;
	private bool contar;

	private bool contarCometaAsteroide;

	private void Start()
	{
		origin = this.transform.position;
		move = false;
		contar = false;
		contarCometaAsteroide = true;


		if (tieneAgua) {
			this.GetComponentInChildren<TrailRenderer> ().enabled = false;
		}
	}


	void FixedUpdate(){
		if (move) {
			
			Sun sun = FindObjectOfType<Sun> ();
			Vector3 direction1 = rb.position - sun.rb.position;
			float distance1 = direction1.magnitude;

			Plano plano = FindObjectOfType<Plano> ();
			Vector3 direction2 = rb.position - plano.rb.position;
			float distance2 = direction2.magnitude;

			//aqui se supone necesita la colita
			if (distance1 > 12 && move && tieneAgua && distance2 > 3.5) {

				this.GetComponentInChildren<TrailRenderer> ().enabled = true;
				if (contar) {
					int com = int.Parse (cometas.text);
					com += 1;
					cometas.text = com + "";
					contar = false;
				}
			} else if (tieneAgua && distance1 <= 12){
				this.GetComponentInChildren<TrailRenderer> ().enabled = false;
				if (contarCometaAsteroide) {
					Debug.Log ("yo");
					contar = true;
					contarCometaAsteroide = false;
				}
			}

			//aqui se evalua si no es cometa 
			if (contar) {
				if (this.rb.mass >= 10) {

					int plan = int.Parse (planetoides.text);
					plan += 1;
					planetoides.text = plan + "";
					contar = false;

				} else if (this.rb.mass < 10 && this.rb.mass > 5) {

					int ast = int.Parse (asteroides.text);
					ast += 1;
					asteroides.text = ast + "";
					contar = false;

				} else {

					int meteo = int.Parse (meteoroides.text);
					meteo += 1;
					meteoroides.text = meteo + "";
					contar = false;
				}
			}
				
			//aqui se reinicia la pos cuando ya esta muy lejos
			if (distance2 > 100) {
				StartCoroutine (gotToTheEnd ());
			}
		}
	}
		

	public void letsMove(){
		this.move = true;
		contar = true;
	}
		
	IEnumerator gotToTheEnd(){
		doNotMove ();

		if (tieneAgua) {
			this.GetComponentInChildren<TrailRenderer> ().enabled = false;
		}

		rb.velocity = Vector3.zero;
		rb.angularVelocity = Vector3.zero;

		contar = false;
		contarCometaAsteroide = true;

		yield return new WaitForSeconds (3);
		this.transform.position = origin;
	}

	public void doNotMove(){
		this.move = false;
	}
}
