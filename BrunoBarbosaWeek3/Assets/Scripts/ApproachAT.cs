using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions {

	public class ApproachAT : ActionTask {
		public BBParameter<Transform> target;
		public BBParameter<float> speed;
		public BBParameter<float> arrivalDistance;


		//Use for initialization. This is called only once in the lifetime of the task.
		//Return null if init was successfull. Return an error string otherwise
		protected override string OnInit() {
			return null;
		}

		//This is called once each time the task is enabled.
		//Call EndAction() to mark the action as finished, either in success or failure.
		//EndAction can be called from anywhere.
		protected override void OnExecute() {
			
		}

		//Called once per frame while the action is active.
		protected override void OnUpdate() {
			Vector3 moveDirection = (target.value.position - agent.transform.position).normalized;
			agent.transform.position += moveDirection * speed.value * Time.deltaTime;

			float distanceToTarget = Vector3.Distance(target.value.position, agent.transform.position);
			if (distanceToTarget < arrivalDistance.value)
			{
				EndAction(true);
			}
		}

		//Called when the task is disabled.
		protected override void OnStop() {
			
		}

		//Called when the task is paused.
		protected override void OnPause() {
			
		}
	}
}