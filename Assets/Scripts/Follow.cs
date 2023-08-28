using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public GameObject target;
    public float distance, height, speed;

    Vector3 pos;

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        pos = new Vector3(target.transform.position.x, height, target.transform.position.z-distance);

        this.gameObject.transform.position = Vector3.Lerp(this.gameObject.transform.position, pos, speed * Time.deltaTime);
    }
}
