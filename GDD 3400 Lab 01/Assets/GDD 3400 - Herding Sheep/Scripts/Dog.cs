using UnityEngine;

namespace GDD3400.Project01
{
    public class Dog : MonoBehaviour
    {

        //private RigidBody _rb;
        
        private bool _isActive = true;
        public bool IsActive 
        {
            get => _isActive;
            set => _isActive = value;
        }

        // Required Variables (Do not edit!)
        private float _maxSpeed = 5f;
        private float _sightRadius = 7.5f;

        // Layers - Set In Project Settings
        public LayerMask _targetsLayer;
        public LayerMask _obstaclesLayer;

        // Tags - Set In Project Settings
        private string friendTag = "Friend";
        private string threatTag = "Thread";
        private string safeZoneTag = "SafeZone";



        public void Awake()
        {
            // Find the layers in the project settings
            _targetsLayer = LayerMask.GetMask("Targets");
            _obstaclesLayer = LayerMask.GetMask("Obstacles");

        }

        private void Update()
        {
            if (!_isActive) return;
            
            Perception();
            DecisionMaking();
        }

        private void Perception()
        {

            //this code is similar to the sheep perception except the threat possibilites are not needed

           // _friendTargets.Clear();
           // _safeZoneTarget = null;

            // Collect all target colliders within the sight radius
            //int t = Physics.OverlapSphereNonAlloc(transform.position, _sightRadius, _tmpTargets, _targetsLayer);
            //for (int i = 0; i < t; i++)
            //{
               // var c = _tmpTargets[i];
                //if (c==null || c.gameObject == gameObject) continue;

                // Store the friends and safe zone targets
                //switch (c.tag)
                //{
                   // case _friendTag:
                        //_friendTargets.Add(c);
                        //break;
                   
                    //case _safeZoneTag:
                       // _safeZoneTarget = c;
                       // break;
                //}
            //}
            
        }

        private void DecisionMaking()
        {


            
            //if sheep are seen, go near to the sheep

            //This is simialr to the sheep behavior that goes toward the do if it is a freind except speed is defaukt for the dog.


            // foreach (var friend in _friendTargets)
           // {
               // if (friend.GetComponentInParent<Dog>() != null)
               // {
                    //_target = friend.transform.position;
                   // return;
               // }
            //}


            //if certain distance from sheep go back toward safezone at a walk 

            //check if dog is close enough
           // if(var friend.position - var dog.postion < 5 )
            //{

                //walk toward safezone

                //_target = _safeZoneTarget;



           // }

            
            //if no sheep seen that are not in safe zone, go wander

            //if(_friendTargets = null or freindTargets == InSafeZone)

            //{

                // go back to wander behavior


            //}

            //if no sheep within sight, rotate 360 degrees




        }

        /// <summary>
        /// Make sure to use FixedUpdate for movement with physics based Rigidbody
        /// You can optionally use FixedDeltaTime for movement calculations, but it is not required since fixedupdate is called at a fixed rate
        /// </summary>
        private void FixedUpdate()
        {
            if (!_isActive) return;

            
            
        }
    }
}
