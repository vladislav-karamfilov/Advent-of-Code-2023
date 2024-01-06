namespace Day24NeverTellMeTheOdds;

public readonly record struct Vector3D(long X, long Y, long Z)
{
    public static Vector3D operator +(Vector3D left, Vector3D right)
        => new(left.X + right.X, left.Y + right.Y, left.Z + right.Z);

    public static Vector3D operator -(Vector3D left, Vector3D right)
        => new(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
}
