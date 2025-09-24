using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UIElements;

namespace GDD3400.Project01
{

    [SelectionBase]
    [RequireComponent(typeof(Rigidbody))]

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


        //Added variables to make the RigidBody stuff work


         // Movement Settings
        [NonSerialized] private float _stoppingDistance = 1.5f;
        [NonSerialized] private float _flockingDistance = 3.5f;
        [NonSerialized] private float _wanderSpeed = .5f;
        [NonSerialized] private float _walkSpeed = 2.5f;
        [NonSerialized] private float _runSpeed = 5f;
        [NonSerialized] private float _turnRate = 5f;

     

        // Dynamic Movement Variables
        private Vector3 _velocity;
        private float _targetSpeed;
        private Vector3 _target;
        private Vector3 _floatingTarget;
        private Collider[] _tmpTargets = new Collider[16]; // Maximum of 16 targets in each perception check







        public void Awake()
        {
            // Find the layers in the project settings
            _targetsLayer = LayerMask.GetMask("Targets");
            _obstaclesLayer = LayerMask.GetMask("Obstacles");

            //_rb = GetComponent<Rigidbody>();

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

            
                //copy and pasted sheep rigid body code

             if (_floatingTarget != Vector3.zero && Vector3.Distance(transform.position, _floatingTarget) > _stoppingDistance)
            {
                // Calculate the direction to the target position
                Vector3 direction = (_floatingTarget - transform.position).normalized;

                // Calculate the movement vector
                _velocity = direction * Mathf.Min(_targetSpeed, Vector3.Distance(transform.position, _floatingTarget));
            }

            // Calculate the desired rotation towards the movement vector
            if (_velocity != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(_velocity);
                
                // Smoothly rotate towards the target rotation based on the turn rate
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _turnRate);
            }

            //_rb.linearVelocity = _velocity;


            
        }
    }
}
