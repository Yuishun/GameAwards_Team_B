using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainDropParticle : MonoBehaviour
{
    public GameObject MObject;
    public bool Active
    {
        get { return _active; }
        set
        {
            _active = value;
            if (MObject)
            {
                MObject.SetActive(value);

                if (tr)
                    tr.Clear();

            }
        }
    }
    bool _active;

    public Rigidbody2D rb;
    TrailRenderer tr;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    public void Init()
    {
        rb = GetComponent<Rigidbody2D>();
        tr = GetComponent<TrailRenderer>();
        Active = false;
    }

    // Update is called once per frame
    void Update()
    {
        //VelocityLimiter();
    }

    void VelocityLimiter()
    {


        Vector2 _vel = rb.velocity;
        if (_vel.y < -10f)
        {
            _vel.y = -10f;
        }
        rb.velocity = _vel;
    }

    public IEnumerator FallGravity(Vector2 vec)
    {
        yield return null;

        rb.velocity = vec;
    }
}
