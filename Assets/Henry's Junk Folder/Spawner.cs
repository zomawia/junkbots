using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public GameObject objectToSpawn;
    public Vector3 forceDir;

    public float spawnInterval;
    private float spawnTimer;

    public Transform baseTransform;

	// Use this for initialization
	void Start () {
        spawnTimer = spawnInterval;
	}
	
	// Update is called once per frame
	void Update () {
        spawnTimer -= Time.deltaTime;
        if(spawnTimer <= 0)
        {
            GameObject obj = Instantiate(objectToSpawn, baseTransform.position, new Quaternion(Random.Range(0,360), Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360)));
            obj.GetComponent<Rigidbody>().AddForce(forceDir, ForceMode.Impulse);

            spawnTimer = spawnInterval;
        }
	}

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(this.gameObject);
    }
}
