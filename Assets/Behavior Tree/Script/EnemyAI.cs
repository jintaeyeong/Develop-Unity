using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float startingHealth;
    [SerializeField] private float lowHealthThreshold;
    [SerializeField] private float healthRestoreRate;

    [SerializeField] private float chasingRange;
    [SerializeField] private float shootingRange;

    [SerializeField] private Transform playerTransform;


    [SerializeField] private Cover[] availableCovers;

    private Material material;
    private Transform bestCoverSpot;
    private NavMeshAgent agent;

    private Node topNode;

    private float _currentHealth;

    public float currentHealth
    {
        get { return _currentHealth; }
        set { _currentHealth = Mathf.Clamp(value, 0, startingHealth); }
    }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        material = GetComponent<MeshRenderer>().material;

    }

    private void Start()
    {
        _currentHealth = startingHealth;
        ConstructBehaviourTree();
    }

    private void ConstructBehaviourTree()
    {
        IsCoveredAvailableNode coveredAvailableNode = new IsCoveredAvailableNode(availableCovers, playerTransform, this);
        GotoCoverNode gotoCoverNode = new GotoCoverNode(agent, this);
        HealthNode healthNode = new HealthNode(this, lowHealthThreshold);
        IsConveredNode isCoveredNode = new IsConveredNode(playerTransform, transform);
        ChaseNode chaseNode = new ChaseNode(playerTransform, agent, this);
        RangeNode chasingRangeNode = new RangeNode(chasingRange, playerTransform, transform);
        RangeNode shootingRangeNode = new RangeNode(shootingRange, playerTransform, transform);
        ShootNode shootNode = new ShootNode(agent, this);

        Sequence chaseSequence = new Sequence(new List<Node> { chasingRangeNode, chaseNode });
        Sequence shootSequence = new Sequence(new List<Node> { shootingRangeNode, shootNode });

        Sequence goToCoverSequence = new Sequence(new List<Node> { coveredAvailableNode, gotoCoverNode});
        selector findCoverSelector = new selector(new List<Node> { goToCoverSequence, chaseSequence });
        selector tryToTakeCoverSelector = new selector(new List<Node> { isCoveredNode, findCoverSelector });
        Sequence mainCoverSequence = new Sequence(new List<Node> { healthNode, tryToTakeCoverSelector });

        topNode = new selector(new List<Node> { mainCoverSequence, shootSequence, chaseSequence });

    }


    private void Update()
    {
        topNode.Evaluate();
        if(topNode.nodeState == NodeState.Failure)
        {
            SetColor(Color.red);
        }

        _currentHealth += Time.deltaTime * healthRestoreRate;
    }

    private void OnMouseDown()
    {
        currentHealth -= 10f;
    }



    public void SetColor(Color color)
    {
        material.color = color;
    }

    public void SetBestCoverSpot(Transform bestCoverSpot)
    {
        this.bestCoverSpot = bestCoverSpot;
    }

    public Transform GetBestCover()
    {
        return bestCoverSpot;
    }
}
