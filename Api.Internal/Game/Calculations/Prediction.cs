using System.Numerics;
using Api.Game.Calculations;
using Api.Game.Objects;

namespace Api.Internal.Game.Calculations;

public class Prediction : IPrediction
{
    public PredictionResult PredictPosition(IHero target, Vector3 sourcePosition, float delay, float speed, float radius)
    {
        var predictedPosition = PredictPositionInternal(target, sourcePosition, delay, speed, radius);
        var hitChance = CalculateHitChance(target, predictedPosition, sourcePosition, delay, speed, radius);
        return new PredictionResult(predictedPosition, hitChance);
    }
    
    private Vector3 PredictPositionInternal(IHero target, Vector3 sourcePosition, float delay, float speed, float radius)
    {
        var waypoints = target.AiManager.RemainingPath.ToArray();
        if (waypoints.Length == 0) return target.AiManager.CurrentPosition;
    
        var timeElapsed = 0f;
        var currentPosition = target.AiManager.CurrentPosition;
    
        for (var i = 0; i < waypoints.Length - 1; i++)
        {
            var waypointEnd = waypoints[i + 1];
    
            var distanceToNextWaypoint = Vector3.Distance(currentPosition, waypointEnd);
            var timeToReachNextWaypoint = distanceToNextWaypoint / target.AiManager.MovementSpeed;

            var remainingTravelTime = (timeElapsed + timeToReachNextWaypoint) - delay;
            if (timeElapsed + timeToReachNextWaypoint > delay)
            {
                var predictedPositionAfterDelay = currentPosition + Vector3.Normalize(waypointEnd - currentPosition) * (target.AiManager.MovementSpeed * remainingTravelTime);
    
                var missileTravelTime = Vector3.Distance(sourcePosition, predictedPositionAfterDelay) / speed;
    
                if (missileTravelTime < remainingTravelTime)
                {
                    return predictedPositionAfterDelay;
                }

                sourcePosition = predictedPositionAfterDelay;
            }
    
            timeElapsed += timeToReachNextWaypoint;
            currentPosition = waypointEnd;
        }

        return target.AiManager.TargetPosition;
    }

    private float CalculateHitChance(IHero target, Vector3 predictedPosition, Vector3 sourcePosition, float delay, float speed, float radius)
    {
        var distanceToPredictedPosition = Vector3.Distance(predictedPosition, sourcePosition);
        var travelTime = distanceToPredictedPosition / speed + delay;
        var maxDistanceTargetCanMove = target.AiManager.MovementSpeed * travelTime;
        var effectiveHitDistance = radius + maxDistanceTargetCanMove;
        var hitChance = (radius / effectiveHitDistance) * 100f;

        return Math.Clamp(hitChance, 0f, 100f);
    }
}