using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    [RequireComponent(typeof(TubeMesh))]
    public class JacobTempPlayerController : MonoBehaviour
    {
        private const float SegmentLength = 0.6f;

        public GameObject BodyPointPrefab;
        public BoxCollider GroundedTrigger;
        public Material BodyMaterial;
        public int MaxBodyPoints = 4;
        public float TurnRate = 15;
        public float MaxBodyLengh => (MaxBodyPoints - 1) * SegmentLength;

        [Header("Config")]
        public AnimationCurve InputDistanceMovementCurve;
        public AnimationCurve EndOfLengthMovementCurve;
        public AnimationCurve RetractionCurve;

        private MovementState _movementState;
        private List<GameObject> BodyPoints = new List<GameObject>();
        private Rigidbody _rigidbody;
        private float _cachedBodyLength;
        private TubeMesh _tubeMesh;
        private GameObject _tubeMeshObj;

        bool _xformPushedToTube;
        
        void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _tubeMeshObj = new GameObject();
            _tubeMesh = _tubeMeshObj.AddComponent<TubeMesh>();
            _tubeMeshObj.transform.position = Vector3.zero;
            _tubeMeshObj.GetComponent<MeshRenderer>().material = BodyMaterial;
            BodyPoints.Add(Object.Instantiate(BodyPointPrefab, transform.position, transform.rotation));
            _tubeMesh.PushNode(BodyPoints[BodyPoints.Count - 1].transform);
            _xformPushedToTube = false;
        }

        void PushXformToTube()
        {
            if (!_xformPushedToTube)
            {
                _xformPushedToTube = true;
                _tubeMesh.PushNode(transform);
            }
        }

        void PopXformFromTube()
        {
            if (_xformPushedToTube)
            {
                _xformPushedToTube = false;
                _tubeMesh.PopNode();
            }
        }

        void Update()
        {
            PopXformFromTube();

            _cachedBodyLength = (BodyPoints.Count - 1) * SegmentLength;
            _cachedBodyLength += (transform.position - BodyPoints[BodyPoints.Count - 1].transform.position).magnitude;

            DetectExtents();

            if (Input.GetMouseButtonDown(0))
            {
                _movementState = MovementState.EXPANDING;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                _movementState = MovementState.IDLE;
                HaltMovement();
            }

            if(_movementState!= MovementState.EXPANDING)
            {
                if (Input.GetMouseButtonDown(1))
                    _movementState = MovementState.RETRACTING;
                else if (Input.GetMouseButtonUp(1))
                    _movementState = MovementState.IDLE;
            }

            switch (_movementState)
            {
                case MovementState.IDLE:
                    break;
                case MovementState.EXPANDING:
                    if (BodyPoints.Count < MaxBodyPoints)
                    {
                        // Input in screenspace 
                        Plane playerGround = new Plane(Vector3.up, transform.position);
                        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                        if (playerGround.Raycast(inputRay, out float dist)) // Mouse is pointed at ground
                        {
                            Vector3 direction = (inputRay.GetPoint(dist) - transform.position);
                            TryMove(direction.normalized 
                                * InputDistanceMovementCurve.Evaluate(direction.magnitude)
                                * EndOfLengthMovementCurve.Evaluate(MaxBodyLengh - _cachedBodyLength));
                        }
                    }
                    break;
                case MovementState.RETRACTING:
                    HaltMovement(); // Don't let velocity contribute
                    float newDist = _cachedBodyLength - Time.deltaTime * RetractionCurve.Evaluate(MaxBodyLengh- _cachedBodyLength);
                    newDist = Mathf.Max(0, newDist); // No negative distances
                    int newFinalPoint = Mathf.FloorToInt(newDist / SegmentLength); // +1 because it starts with a point, but -1 because we want the index
                    float progressOnNewSeg = (newDist % SegmentLength) / SegmentLength;

                    if(newFinalPoint == BodyPoints.Count - 1) // Just move back on current trajectory
                    {
                        Vector3 projectedDirection = transform.position - BodyPoints[BodyPoints.Count - 1].transform.position;
                        transform.position = BodyPoints[BodyPoints.Count-1].transform.position + (projectedDirection.normalized) * SegmentLength * progressOnNewSeg;
                        break;
                    }
                    // Otherwise: We need to cull points
                    transform.position = Vector3.Lerp(BodyPoints[newFinalPoint].transform.position, BodyPoints[newFinalPoint + 1].transform.position, progressOnNewSeg);
                    transform.rotation = BodyPoints[newFinalPoint].transform.rotation;

                    for (int i = BodyPoints.Count-1; i>newFinalPoint; i--)
                    {
                        Object.Destroy(BodyPoints[i]);
                        BodyPoints.RemoveAt(i);
                        _tubeMesh.PopNode();

                    }
                    break;
            }

            

            // We don't jump
            if (_rigidbody.velocity.y>0 &&
                Physics.OverlapBox(GroundedTrigger.bounds.center, GroundedTrigger.bounds.extents, transform.rotation, LayerMask.GetMask("Ground")).Length == 0)
            {
                _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, 0, _rigidbody.velocity.z);
            }

            PushXformToTube();
        }

        private void HaltMovement()
        {
            _rigidbody.velocity = new Vector3(0, _rigidbody.velocity.y, 0); // Cease intentional movement
        }

        private void TryMove(Vector3 direction)
        {
            // Make sure we have room
            if (BodyPoints.Count >= MaxBodyPoints)
                return;

            // Insert a turn radius
            float originalMagn = direction.magnitude;
            if (Vector3.Angle(BodyPoints[BodyPoints.Count - 1].transform.forward, direction) > TurnRate)
            {
                direction = Vector3.RotateTowards(BodyPoints[BodyPoints.Count - 1].transform.forward*originalMagn, direction, Mathf.Deg2Rad* TurnRate, 0f);
            }
            //if (Vector3.Angle(transform.forward, direction) > TurnRate)
            //{
                //direction = Vector3.RotateTowards(transform.forward, direction, Time.deltaTime*Mathf.Deg2Rad* TurnRate, 0f)*originalMagn;
            //}
            // TODO
            //if(Vector3.Angle(BodyPoints[BodyPoints.Count-1].transform.forward, direction) > TurnRate) // Clamp within overall angle
            //{
            //   direction = Vector3.RotateTowards(BodyPoints[BodyPoints.Count-1].transform.forward, direction, Mathf.Deg2Rad * TurnRate, 0f)*originalMagn;
            //}

            Vector3 newVel = new Vector3(direction.x, _rigidbody.velocity.y, direction.z);
            _rigidbody.velocity = newVel;
            transform.forward = newVel;
            return; 
        }

        private void UpdateTube()
        {
            List<Transform> xforms = new List<Transform>();
            foreach (var point in BodyPoints)
            {
                xforms.Add(point.transform);
            }
            xforms.Add(transform);
           // _tubeMesh.SetNodes(xforms);
        }

        private void DetectExtents()
        {
            // Todo: place more than 1 segment theoretically I guess (lag spike abuse prevention)
            Vector3 offset = transform.position - BodyPoints[BodyPoints.Count - 1].transform.position;
            Vector3 normalizedOffset = offset.normalized;
            if (offset.magnitude > SegmentLength)
            {
                Vector3 segmentEndpoint = BodyPoints[BodyPoints.Count-1].transform.position + normalizedOffset * SegmentLength;
                BodyPoints.Add(Object.Instantiate(BodyPointPrefab, segmentEndpoint, transform.rotation));
                _tubeMesh.PushNode(BodyPoints[BodyPoints.Count - 1].transform);
            }

            if (BodyPoints.Count >= MaxBodyPoints)
                HaltMovement();
        }

        public enum MovementState
        {
            IDLE = 0,
            EXPANDING = 1,
            RETRACTING = 2
        }
    }
}