using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Contains weights for the flocking forces as static vars that all birds can access.
/// </summary>
public class FlockingWeights : MonoBehaviour
{
    public Slider cohesionSlider;
    public Slider avoidanceSlider;
    public Slider alignmentSlider;
    public Slider neighborSlider;

    public float maxCohesion, maxAvoidance, maxAlignment;
    public float maxNeighborRadius;

    public static float cohesionWeight = 1;
    public static float avoidanceWeight = 1;
    public static float alignmentWeight = 1;
    public static float neighborRadius = 5;

    public void Start() {
        SetCohesion(cohesionSlider.value);
        SetAvoidance(avoidanceSlider.value);
        SetAlignment(alignmentSlider.value);
        SetNeighborRadius(neighborSlider.value);
    }

    public void SetCohesion(float c) {
        cohesionWeight = c * maxCohesion;
    }
    public void SetAvoidance(float a) {
        avoidanceWeight = a * maxAvoidance;
    }
    public void SetAlignment(float a) {
        alignmentWeight = a * maxAlignment;
    }
    public void SetNeighborRadius(float n) {
        neighborRadius = n * maxNeighborRadius;
    }
}
