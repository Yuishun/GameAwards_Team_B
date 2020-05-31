using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetaballParticleClass : MonoBehaviour {


	public GameObject MObject;
	public float LifeTime;
	public bool Active{
		get{ return _active;}
		set{ _active = value;
			if (MObject) {
				MObject.SetActive (value);

				if (tr)
					tr.Clear ();

			}
		}
	}
    [SerializeField,Header("屈折開始水粒の数"),Range(1,4)]
    public int Count = 4;
	public bool witinTarget; // si esta dentro de la zona de vaso de vidrio en la meta



	bool _active;
	float delta;
	public Rigidbody2D rb;
	TrailRenderer tr;
    public SpriteRenderer spRend;

	void Start () {
        //MObject = gameObject;
        Init();
	}

    public void Init()
    {
        rb = GetComponent<Rigidbody2D>();
        tr = GetComponent<TrailRenderer>();
        spRend = GetComponent<SpriteRenderer>();
        Active = false;
    }

    void Update () {

		if (Active == true) {

			VelocityLimiter ();

			//if (LifeTime < 0)
				return;
            /*
			if (delta > LifeTime) {
				delta *= 0;
				Active = false;
			} else {
				delta += Time.deltaTime;
			}
            */

		}

	}



	void VelocityLimiter()
	{
		
		
		Vector2 _vel = rb.velocity;
		if (_vel.y < -8f) {
			_vel.y = -8f;
		}
		rb.velocity = _vel;
	}

    public bool CountWater()
    {
        Collider2D[] col = new Collider2D[4];
            Physics2D.OverlapCircleNonAlloc(transform.position, 0.5f, col,
            LayerMask.GetMask("PostProcessing"));
        for(int i = 0; i < Count; i++)
        {
            if (!col[i])
                return false;
        }
                
            return true;
    }

    public RaycastHit2D WaterNormalVec(RaycastHit2D ray)
    {
        Collider2D[] col = Physics2D.OverlapCircleAll(transform.position, 0.2f
            , LayerMask.GetMask("PostProcessing"));
        for(int i = 0; i < col.Length; i++)
        {
            if (col[i].transform.position == transform.position)
                continue;

            Vector2 point = nearPoint(transform.position,
                col[i].transform.position, ray.point);
            if (point == Vector2.zero)
                continue;
            ray.normal = (ray.point - point).normalized;

        }
        return ray;
    }

    Vector2 nearPoint(Vector2 A,Vector2 B, Vector2 P)
    {
        Vector2 a, b;
        float r;

        a.x = B.x - A.x;
        a.y = B.y - A.y;
        b.x = P.x - A.x;
        b.y = P.y - A.y;

        // 内積 ÷ |a|^2
        r = (a.x * b.x + a.y * b.y) / (a.x * a.x + a.y * a.y);

        if (r == 0)
        {
            return A;
        }
        else if (r == 1)
        {
            return B;
        }
        else if(r < 0 || r > 1)
        {
            return Vector2.zero;
        }
        else
        {
            Vector2 result;
            result.x = A.x + r * a.x;
            result.y = A.y + r * a.y;
            return result;
        }

    }
}
