using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UIElements;

namespace GDD3400.Project01
{

    //not sure what selection base is
    [SelectionBase]

    //I dont understand why the required component thing is necessary, ive used rigidbodies in unity/c# many time without that before
    [RequireComponent(typeof(Rigidbody))]

    public class Dog : MonoBehaviour
    {   

        //michellanious variables proably dont need all of them

        private Rigidbody _rb; 
        private Level _level;
        private bool _inSafeZone = false;
        public bool InSafeZone => _inSafeZone;
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
        private const string _friendTag = "Friend";
        private const string _threatTag = "Threat";
        private const string _safeZoneTag = "SafeZone";

        //added obstacle tag for walls

        private const string _obstacleTag = "Obstacle";

        //Added variables to make the RigidBody stuff work proably dont need all of them

        // Movement Settings
        [NonSerialized] private float _stoppingDistance = 1.5f;
        [NonSerialized] private float _flockingDistance = 3.5f;
        [NonSerialized] private float _wanderSpeed = .5f;
        [NonSerialized] private float _walkSpeed = 2.5f;
        [NonSerialized] private float _runSpeed = 5f;
        [NonSerialized] private float _turnRate = 5f;


        //need some of these for target checkin? proably dont need all of them

        // Dynamic Movement Variables
        private Vector3 _velocity;
        private float _targetSpeed;
        private Vector3 _target;
        private Vector3 _floatingTarget;
        private Collider[] _tmpTargets = new Collider[16]; // Maximum of 16 targets in each perception check
        
        //This makes the perception work for knowing if sheep are following
        
        private List<Collider> _friendTargets = new List<Collider>();
        private Collider _safeZoneTarget;
        private Collider obstacleCollider;

        //Added variables

        //bool to see if sheep are following
        bool hasSheep;

        //step counter variable for when dog has already checked a certain direction for long enough
        int stepCounter = 0;


        public GameObject SafeZone;


        public void Awake()
        {
            // Find the layers in the project settings
            _targetsLayer = LayerMask.GetMask("Targets");
            _obstaclesLayer = LayerMask.GetMask("Obstacles");
            _rb = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            if (!_isActive) return;         
            Perception();
            DecisionMaking();
        }
        private void Perception()
        {
            //this code is similar to the sheep perception except the threat and safe zone aspects are not needed

            _friendTargets.Clear();

            // Collect all target colliders within the sight radius
            int t = Physics.OverlapSphereNonAlloc(transform.position, _sightRadius, _tmpTargets, _targetsLayer);
            for (int i = 0; i < t; i++)
            {
                var c = _tmpTargets[i];
                if (c==null || c.gameObject == gameObject) continue;

                // only freind tag is needed from sheep code
                switch (c.tag)
                {
                    case _friendTag:
                        _friendTargets.Add(c);
                        break;                                   
                }
            }      
        }

        private void DecisionMaking()
        { 

            //if have group of sheep that is a cetain threshold lead them to the safe zone

            if(hasSheep == true)
            {
                DeliverSheep(); 
            }

            else
            {
                Wander();
            }
      
        }
            
        //Wander is set on distance threshhold. At 30 fps, after 10 seconds, dog will change to a random direction then move forward.

         public void Wander()       
        {


            //add 1 to step if wandering
            stepCounter +=1;
            
            // if ceratain abmount of steps taken
            if(stepCounter > 190)
            {
              //claculate new direction
              transform.forward = new Vector3(UnityEngine.Random.Range(-1f, 1f), 0, UnityEngine.Random.Range(-1f, 1f));  

              //reset step counter
              stepCounter = 0;
            }
            
            // simply move forward
            transform.Translate(Vector3.forward * _maxSpeed * Time.deltaTime);

            //check if sheep are following, if so , hasSheep becomes true and dog will no longer wander

            if(_friendTargets.Count >= 3)
            {
                hasSheep = true;
            }

        }


        // deliver function that goes back to the start postion of the dog which should be the safe zone
        //since the dog will have a freind tag, sheep should flock to dog , so if he goes to the zone, sheep will follow suit

        public void DeliverSheep()
        {

            //avoid walls
            if (transform.position.x > 20 )
            {
                //turn west
                transform.forward = new Vector3 (-90,0,-45);
            }
            if(transform.position.x < -20)
            {
                //turn east
                transform.forward = new Vector3 (90,0,45);
            }
            if(transform.position.z > 20)
            {
                // turn  south
                transform.forward = new Vector3 (-45,0,-90);
            }
            if(transform.position.z < -20)
            {
                //turn north
                transform.forward = new Vector3 (45,0,90);
            }

            

            //simply move forward
            //walkspeed is important here because otherwise the sheep could not keep up

            transform.Translate(Vector3.forward * _walkSpeed * Time.deltaTime);

            

        }

        //if sheep are dropped of at safe zone, go back to wandering same as sheep code trigger except instead of disseappering into particles, 
        //becomes false and therfore should go back to wandering

        public void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(_safeZoneTag) && !_inSafeZone)
            {
                _inSafeZone = true;

                if (_level != null)
                {
                    hasSheep = false;
                }
            }


            //if hit a wall turn around 

            if(other.CompareTag(_obstacleTag))
            {
                     transform.forward *= -1;
            }

        }
         

        //copy and pasted sheep rigid body stuff for fixed update

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

            _rb.linearVelocity = _velocity;
            
        }
    }
}
