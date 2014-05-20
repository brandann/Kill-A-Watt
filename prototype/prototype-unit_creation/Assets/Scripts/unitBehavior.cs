using UnityEngine;
using System.Collections;

public class unitBehavior : MonoBehaviour {

	public float speed = .01f;
	private int damage = 1;

	public Vector3 target;
	NavMeshAgent agent;
	
	public Sprite play1;
	public Sprite play2;

	// Use this for initialization
	void Start () {
		agent = this.GetComponent<NavMeshAgent> ();
	}
	
	// Update is called once per frame
	void Update () {
		//agent.SetDestination (target);
		transform.position += (speed * Time.smoothDeltaTime) * transform.up;
	}
	
	void OnTriggerEnter2D(Collider2D other)
	{
		string name = other.name;
		if (name.Contains("unit(")) {
			GameObject unit = GameObject.Find(other.name);
			unitBehavior otherscript = (unitBehavior) unit.GetComponent(typeof(unitBehavior));
			if(this.getDamage() == otherscript.getDamage()) {return;}
			Destroy(other.gameObject);
			Destroy(this.gameObject);
		}
	}

	public int getDamage() {
		return damage;
	}

	public void setDamage(int d) {
		damage = d;
		SpriteRenderer r = GetComponent<SpriteRenderer>();
		if(damage < 0){
			r.sprite = play1;
		}
		else {
			r.sprite = play2;
		}
	}
}
