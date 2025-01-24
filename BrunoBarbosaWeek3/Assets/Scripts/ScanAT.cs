using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System.Linq;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions {

	public class ScanAT : ActionTask {
		public Color scanColour;
		public int numberOfScanCirclePoints;
		public BBParameter <float> detectionRadius;
		public LayerMask LightMachineLayerMask;
		public Transform towerlocation;
        public BBParameter<float> speed;
        public BBParameter<float> arrivalDistance;

		public BBParameter<Transform> targetTransform;

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
			DrawCircle(agent.transform.position, detectionRadius.value, scanColour, numberOfScanCirclePoints);

            Collider[] detectColliders = Physics.OverlapSphere(agent.transform.position, detectionRadius.value, LightMachineLayerMask);

			//Debug.Log(detectColliders.Count());
            
			foreach (Collider detectCollider in detectColliders)
			{
				Blackboard lightMachineBlackboard = detectCollider.GetComponentInParent<Blackboard>();

				if(lightMachineBlackboard == null)
				{
					Debug.LogError("Scan AT Error - could not find light machine's blackboard");

					return;
				}

				float repairValue = lightMachineBlackboard.GetVariableValue<float>("repairValue");
				if(repairValue ==0)
				{
					/*
					Debug.Log("found uncharged tower");
					Vector3 moveDiretction = (towerlocation.position - agent.transform.position).normalized;
					agent.transform.position += moveDiretction * speed.value * Time.deltaTime;
					float distanceToTarget = Vector3.Distance(towerlocation.position, agent.transform.position);

                    if (distanceToTarget < arrivalDistance.value)
                    {
                        EndAction(true);
                    }
					*/
					targetTransform.value = lightMachineBlackboard.GetVariableValue<Transform>("workpad");
                    EndAction(true);
                }
			}

        }

		private void DrawCircle(Vector3 center, float radius, Color colour, int numberOfPoints)
		{
			Vector3 startPoint, endPoint;
			int anglePerPoint = 360 / numberOfPoints;
			for (int i = 1; i <= numberOfPoints; i++)
			{
				startPoint = new Vector3(Mathf.Cos(Mathf.Deg2Rad * anglePerPoint * (i-1)), 0, Mathf.Sin(Mathf.Deg2Rad * anglePerPoint * (i-1)));
				startPoint = center + startPoint * radius;
				endPoint = new Vector3(Mathf.Cos(Mathf.Deg2Rad * anglePerPoint * i), 0, Mathf.Sin(Mathf.Deg2Rad * anglePerPoint * i));
				endPoint = center + endPoint * radius;
				Debug.DrawLine(startPoint, endPoint, colour);
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