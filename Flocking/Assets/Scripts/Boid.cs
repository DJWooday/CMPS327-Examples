using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A singular boid propeled by 3 flocking functions
/// </summary>
public class Boid : MonoBehaviour
{
    [SerializeField] private List<Boid> neighbors;
    [SerializeField] private LayerMask boidLayer;
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private float avoidanceNeighborDist = 5;

    [Header("Physics")]
    [SerializeField] private float mass = 1;
    [Range(0, .99f)] [SerializeField] private float massVariance = 0;
    [SerializeField] private Vector3 acceleration;
    [SerializeField] private Vector3 velocity;
    [SerializeField] private Vector3 position;

    [Header("Maxes")]
    [SerializeField] private float maxSpeed = 5;
    [SerializeField] private float maxForce = 5;

    [Header("Debug")]
    [SerializeField] private bool debugCohesion = true;
    [SerializeField] private bool debugAvoidance = true;

    // Start is called before the first frame update
    void Start()
    {
        mass += Random.Range(-massVariance, massVariance);
        position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate() {
        // Reset acceleration every frame, we don't want it to accumulate
        acceleration = Vector3.zero;

        // Forces are applied to acceleration vector in these funcs
        CalculateFlockingForces();

        AvoidObstacles();

        // Apply acceleration and set pos
        velocity = Vector3.ClampMagnitude(velocity + acceleration * Time.fixedDeltaTime, maxSpeed);
        position += velocity * Time.fixedDeltaTime;
        transform.position = position;

        // Reorient
        transform.forward = velocity.normalized;
        //transform.right = Vector3.Cross(Vector3.up, transform.forward);
    }

    private void CalculateFlockingForces() {
        FindNeighbors();
        ApplyCohesion();
        ApplyAvoidance();
        ApplyAlignment();
    }

    private void FindNeighbors() {
        // Simply get all boids within a radius, don't worry about sight lines.
        neighbors.Clear();
        Collider[] boidCols = Physics.OverlapSphere(transform.position, FlockingWeights.neighborRadius, boidLayer);
        foreach (var c in boidCols)
            neighbors.Add(c.GetComponent<Boid>());
        neighbors.Remove(this);
    }

    // Make global to help with debugging
    Vector3 avgPos;
    private void ApplyCohesion() {
        if (neighbors.Count == 0) return;
        // Find average position of all local flockmates
        avgPos = Vector3.zero;
        foreach (Boid b in neighbors) {
            avgPos += b.position;
        } avgPos /= neighbors.Count;

        // Calculate steering force to go towards that
        Vector3 desiredVelocity = avgPos - transform.position;
        Vector3 steeringForce = Vector3.ClampMagnitude(desiredVelocity - velocity, maxForce) * FlockingWeights.cohesionWeight;
        acceleration += steeringForce / mass;
    }

    Vector3 avoidanceVelocity;
    private void ApplyAvoidance() {
        if (neighbors.Count == 0) return;
        // For each bird, compute direction away then multiply the direction by some influence level to calculate desired velocity
        avoidanceVelocity = Vector3.zero;
        foreach (Boid b in neighbors) {
            //if (Vector3.Distance(transform.position, b.position) < avoidanceNeighborDist) {
                Vector3 birdToMe = transform.position - b.position;
                float length = birdToMe.magnitude;
                Vector3 avoidanceDir = birdToMe.normalized;
                float influence = 1 / length;
                avoidanceVelocity += avoidanceDir * influence;
            //}
        }

        Vector3 steeringForce = Vector3.ClampMagnitude(avoidanceVelocity - velocity, maxForce) * FlockingWeights.avoidanceWeight;
        acceleration += steeringForce / mass;
    }

    Vector3 avgNeighborVel;
    private void ApplyAlignment() {
        // Find the average velocity of all neighbors and steer towards that
        if (neighbors.Count == 0) return;

        avgNeighborVel = Vector3.zero;
        foreach (Boid b in neighbors) {
            avgNeighborVel += b.velocity;
        } avgNeighborVel /= neighbors.Count;

        Vector3 steeringForce = Vector3.ClampMagnitude(avgNeighborVel, maxForce) * FlockingWeights.alignmentWeight;
        acceleration += steeringForce / mass;
    }

    private void AvoidObstacles() {
        // Raycast out to hit obstacles, steer to avoid them
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 10, obstacleLayer)) {
            Vector3 steeringForce = ((transform.position + velocity.normalized * 10) - hit.point).normalized * maxForce;
            Debug.Log("Steer with " + steeringForce);
            acceleration += steeringForce;
        }

    }

    private void OnDrawGizmosSelected() {
        // Draw neighbor sphere
        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(transform.position, FlockingWeights.neighborRadius);

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, velocity);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, acceleration);

        if (debugCohesion) {
            foreach (Boid b in neighbors) {
                Debug.DrawLine(b.position, avgPos);
            }
        }

        if (debugAvoidance) {
            foreach (Boid b in neighbors) {
                Debug.DrawLine(b.position, transform.position);
            } Debug.DrawRay(transform.position, avoidanceVelocity, Color.yellow);
        }

        Debug.DrawRay(transform.position, transform.position - avgPos, Color.green);
    }
}
