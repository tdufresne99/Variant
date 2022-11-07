using System.Collections;
using System.Collections.Generic;
using PathCreation;
using UnityEngine;
using UnityEngine.AI;

public class PathManager : MonoBehaviour
{
    [SerializeField] AudioSource _asMonstre;
    [SerializeField] private Animator _monstreAnim;
    [SerializeField] private Transform _player;
    [SerializeField] private GameObject[] _waypointsEntry;
    [SerializeField] private GameObject[] _waypointsExit;
    [SerializeField] private PathCreator[] _pathsRoom;
    [SerializeField] private PathCreator[] _pathsCorridor_lab;
    [SerializeField] private PathCreator[] _pathsCorridor_breakroom;
    [SerializeField] private PathCreator[] _pathsCorridor_spawn;
    [SerializeField] private PathCreator[] _pathsCorridor_elevator;
    [SerializeField] private PathCreator[] _pathsCorridor_storage;
    [SerializeField] private EndOfPathInstruction _end;
    [SerializeField] private PathCreator _pathCreator;
    [SerializeField] private float _speed;
    private NavMeshAgent navMeshAgent;
    private Rigidbody _rb; 
    private GameObject _waypoint;
    private GameObject _spawnWaypoint;
    private Transform _transform;
    private Vector3 _startPosition = new Vector3(2.2f,0.1f,28.5f);
    float distanceToTarget = Mathf.Infinity;
    private float _angle;
    private float _dstTravelled;
    private float _distance;
    private bool _chase = false;
    public bool chase{
        get{ return _chase; }
        set{ _chase = value; }
    }
    private bool _stab = false;
    public bool stab{
        get{ return _stab; }
        set{ _stab = value; }
    }


    void Start() 
    {
        _monstreAnim = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();
        _transform = GetComponent<Transform>();
        navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
        _transform.position = _startPosition;
    }
    void Update() 
    {
        _distance = Vector3.Distance(_player.position, gameObject.transform.position);
        if(_distance < 10f) {
            _chase = true;
            if(_stab){
                _monstreAnim.SetBool("chase", false);

            } else{
                _monstreAnim.SetBool("chase", true);
            }
        }
        if(_chase){
            navMeshAgent.SetDestination(_player.position);
            distanceToTarget = Vector3.Distance(_player.position, transform.position);
            // Determine which direction to rotate towards
            Vector3 targetDirection = _player.position - transform.position;

            // The step size is equal to speed times frame time.
            float singleStep = _speed * Time.deltaTime;

            // Rotate the forward vector towards the target direction by one step
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);

            // Draw a ray pointing at our target in
            Debug.DrawRay(transform.position, newDirection, Color.red);

            // Calculate a rotation a step closer to the target and applies rotation to this object
            transform.rotation = Quaternion.LookRotation(newDirection);
        } else {
            MoveIdle();
        }
    }
    private void MoveIdle()
    {
        _dstTravelled += _speed * Time.deltaTime;
        _transform.position = _pathCreator.path.GetPointAtDistance(_dstTravelled, _end);
        _transform.rotation = _pathCreator.path.GetRotationAtDistance(_dstTravelled, _end);
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject == _waypointsEntry[0]){ // Lab Entry
            // Debug.Log("the monster just entered the lab...");
            _pathCreator = _pathsRoom[0];
            _dstTravelled = 0;
            _waypointsEntry[0].SetActive(false);
            _waypoint = _waypointsExit[0];
            Invoke("ActivateWaypoint", 1f);
        }
        else if (other.gameObject == _waypointsExit[0]){ // Lab Exit
            // Debug.Log("the monster just left the lab...");
            int rPath = Random.Range(0,_pathsCorridor_lab.Length);
            _pathCreator = _pathsCorridor_lab[rPath];
            _dstTravelled = 0;
            _waypointsExit[0].SetActive(false);
            _waypoint = _waypointsEntry[0];
            Invoke("ActivateWaypoint", 1f);
        }

        if (other.gameObject == _waypointsEntry[1]){ // Breakroom Entry
            // Debug.Log("the monster just entered the breakroom...");
            _pathCreator = _pathsRoom[1];
            _dstTravelled = 0;
            _waypointsEntry[1].SetActive(false);
            _waypoint = _waypointsExit[1];
            Invoke("ActivateWaypoint", 1f);
        }
        else if (other.gameObject == _waypointsExit[1]){ // Breakroom Exit
            // Debug.Log("the monster just left the breakroom...");
            int rPath = Random.Range(0,_pathsCorridor_breakroom.Length);
            _pathCreator = _pathsCorridor_breakroom[rPath];
            _dstTravelled = 0;
            _waypointsExit[1].SetActive(false);
            _waypoint = _waypointsEntry[1];
            Invoke("ActivateWaypoint", 1f);
        }

        if (other.gameObject == _waypointsEntry[2]){ // Spawn Entry
            // Debug.Log("the monster just entered the spawn room...");
            _pathCreator = _pathsRoom[2];
            _dstTravelled = 0;
            _waypointsEntry[2].SetActive(false);
            _spawnWaypoint = _waypointsExit[2];
            Invoke("ActivateSpawnWaypoint", 1f);
        }
        else if (other.gameObject == _waypointsExit[2]){ // Spawn Exit
            // Debug.Log("the monster just left the spawn room...");
            int rPath = Random.Range(0,_pathsCorridor_spawn.Length);
            _pathCreator = _pathsCorridor_spawn[rPath];
            _dstTravelled = 0;
            _waypointsExit[2].SetActive(false);
            _spawnWaypoint = _waypointsEntry[2];
            Invoke("ActivateSpawnWaypoint", 1f);
        }

        if (other.gameObject == _waypointsEntry[3]){ // Elevator Entry
            // Debug.Log("the monster just entered the elevator...");
            _pathCreator = _pathsRoom[3];
            _dstTravelled = 0;
            _waypointsEntry[3].SetActive(false);
            _waypoint = _waypointsExit[3];
            Invoke("ActivateWaypoint", 1f);
        }
        else if (other.gameObject == _waypointsExit[3]){ // Elevator Exit
            // Debug.Log("the monster just left the elevator...");
            int rPath = Random.Range(0,_pathsCorridor_elevator.Length);
            _pathCreator = _pathsCorridor_elevator[rPath];
            _dstTravelled = 0;
            _waypointsExit[3].SetActive(false);
            _waypoint = _waypointsEntry[3];
            Invoke("ActivateWaypoint", 1f);
        }

        if (other.gameObject == _waypointsEntry[4]){ // Storage Entry
            // Debug.Log("the monster just entered the storage room...");
            _pathCreator = _pathsRoom[4];
            _dstTravelled = 0;
            _waypointsEntry[4].SetActive(false);
            _waypoint = _waypointsExit[4];
            Invoke("ActivateWaypoint", 1f);
        }
        else if (other.gameObject == _waypointsExit[4]){ // Storage Exit
            // Debug.Log("the monster just left the storage room...");
            int rPath = Random.Range(0,_pathsCorridor_storage.Length);
            _pathCreator = _pathsCorridor_storage[rPath];
            _dstTravelled = 0;
            _waypointsExit[4].SetActive(false);
            _waypoint = _waypointsEntry[4];
            Invoke("ActivateWaypoint", 1f);
        }
    }
    private void ActivateWaypoint()
    {
        _waypoint.SetActive(true);
    }
    private void ActivateSpawnWaypoint()
    {
        _spawnWaypoint.SetActive(true);
    }
    private void ResetMonstre()
    {
        gameObject.SetActive(false);
    }
}
