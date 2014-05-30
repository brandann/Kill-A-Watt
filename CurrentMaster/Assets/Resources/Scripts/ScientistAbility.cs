using UnityEngine;
using System.Collections;


namespace Global {
	public class ScientistAbility : MonoBehaviour {

		private bool active = false;
		private enum ability{
			none,
			ability1
		};
		ability currentAbility = ability.none;
		// Use this for initialization
		void Start () {
		
		}
		
		// Update is called once per frame
		void Update () {


			switch (currentAbility) {
			case(ability.none):
				break;
			case(ability.ability1):
				ability1();
				break;
			}
		}

		private void ability1() {
			print ("ability1");
			if (!active) {

			}
		}
	}
}
