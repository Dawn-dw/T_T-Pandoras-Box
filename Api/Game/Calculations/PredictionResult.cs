using System.Numerics;

namespace Api.Game.Calculations;

public struct PredictionResult
{
    public Vector3 Position { get; }
    public float HitChance { get; }
 
    public PredictionResult(Vector3 position, float hitChance)
    {
        Position = position;
        HitChance = hitChance;
    }
}