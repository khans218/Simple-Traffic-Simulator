using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Driver : MonoBehaviour {

	public float distanceCovered = 0;
	public float laneLength = 660f;
    public float speed;

    Collider myCollider;
    //****do not use distance covered if it causes degrade in performance**** rather use a vector3 which tells the coordinates of end of the road and
    //read displacement b/w you're coordinate and that coordinate. if typecasted to int of the value is 0 destroy the vehicle

	// Update is called once per frame
	void Update () {
		transform.Translate (Vector3.forward * speed * Time.deltaTime);
		distanceCovered += speed * Time.deltaTime;
		if (distanceCovered > laneLength) {
			Destroy (this.gameObject);
		}
	}

    private void Awake()
    {
        myCollider = this.GetComponent<BoxCollider>();
        myCollider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "DrivingLane")
        {
            Destroy(this.gameObject);
        }
    }
}
