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

    // 水面の法線を返す
    public RaycastHit2D WaterNormalVec(RaycastHit2D ray)
    {
        // 周囲に水の粒があるか
        Collider2D[] col = Physics2D.OverlapCircleAll(transform.position, 0.2f
            , LayerMask.GetMask("PostProcessing"));
        for(int i = 0; i < col.Length; i++)
        {
            // 自分自身なら処理を飛ばす
            if (col[i].transform.position == transform.position)
                continue;

            //WaterDir((col[i].transform.position - transform.position).normalized);

            // rayの当たっている点から
            // 水面の線分への垂線が線分のどの部分に当たっているか
            Vector2 point = nearPoint(transform.position,
                col[i].transform.position, ray.point);
            // 垂線が線分と交わっていないとき処理を飛ばす
            if (point == Vector2.zero)
                continue;
            // 線分からrayの当たっているところへのベクトルを法線とする
            //normal.Add((ray.point - point).normalized);
            ray.normal= (ray.point - point).normalized;

        }
        ray.normal = WaterDir(ray.normal);
        Debug.DrawRay(ray.point, ray.normal, Color.green);
        return ray;
    }

    // 線分と点から線分への垂線がどこで交わっているか
    public static Vector2 nearPoint(Vector2 A,Vector2 B, Vector2 P,bool mode = true)
    {
        Vector2 a, b;
        float r;

        a.x = B.x - A.x;
        a.y = B.y - A.y;
        b.x = P.x - A.x;
        b.y = P.y - A.y;

        // 内積 ÷ |a|^2
        r = (a.x * b.x + a.y * b.y) / (a.x * a.x + a.y * a.y);

        if(mode)
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
        else        
            if (r <= 0)
            {
                return A;
            }
            else if (r >= 1)
            {
                return B;
            }
            else
            {
                Vector2 result;
                result.x = A.x + r * a.x;
                result.y = A.y + r * a.y;
                return result;
            }
        
    }

    public static Vector2 WaterDir(Vector2 PVec)
    {
        float answer = -2;
        int index = -1;
        for(int i = 0; i < 16; i++)
        {
            Vector2 vec2 = normalVec(i);
            float a = (PVec.x * vec2.x + PVec.y * vec2.y)
                / (PVec.magnitude * vec2.magnitude);
            if (a > answer)
            {
                answer = a;
                index = i;
            }
        }

        return normalVec(index);
    }

    static Vector2 normalVec(int i)
    {
        Vector2 vec = Vector2.zero;
        switch (i)
        {
            case 0:
                vec = Vector2.up;
                break;
            case 1:
                vec = new Vector2(1, 1).normalized;
                break;
            case 2:
                vec = Vector2.right;
                break;
            case 3:
                vec = new Vector2(1, -1).normalized;
                break;
            case 4:
                vec = Vector2.down;
                break;
            case 5:
                vec = new Vector2(-1, -1).normalized;
                break;
            case 6:
                vec = Vector2.left;
                break;
            case 7:
                vec = new Vector2(-1, 1).normalized;
                break;

         /*   case 8:
                vec = Vector2.Lerp(Vector2.up, new Vector2(1, 1),0.5f).normalized;
                break;
            case 9:
                vec= Vector2.Lerp(Vector2.right, new Vector2(1, 1), 0.5f).normalized;
                break;
            case 10:
                vec= Vector2.Lerp(Vector2.right, new Vector2(1, -1), 0.5f).normalized;
                break;
            case 11:
                vec= Vector2.Lerp(Vector2.down, new Vector2(1, -1), 0.5f).normalized;
                break;
            case 12:
                vec= Vector2.Lerp(Vector2.down, new Vector2(-1, -1), 0.5f).normalized;
                break;
            case 13:
                vec= Vector2.Lerp(Vector2.left, new Vector2(-1, -1), 0.5f).normalized;
                break;
            case 14:
                vec= Vector2.Lerp(Vector2.left, new Vector2(-1, 1), 0.5f).normalized;
                break;
            case 15:
                vec = Vector2.Lerp(Vector2.up, new Vector2(-1, 1), 0.5f).normalized;
                break;
         */
        }
        return vec;
    }
}
