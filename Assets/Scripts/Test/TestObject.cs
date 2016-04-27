using UnityEngine;
using System.Collections;

public class TestObject : PoolObject
{
    TrailRenderer trail;
    float trailTime;

    void Awake()
    {
        trail = GetComponent<TrailRenderer>();
        trailTime = trail.time;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale += Vector3.one * Time.deltaTime * 3;
        transform.Translate(Vector3.forward * Time.deltaTime * 25);
    }

    public override void OnObjectReuse()
    {
        trail.time = -1;                //giving negative numbers to stop it form rendering any trail.
        Invoke("ResetTrail", 0.1f);
        transform.localScale = Vector3.one;

    }

    void ResetTrail()
    {
        trail.time = trailTime;
    }
}
