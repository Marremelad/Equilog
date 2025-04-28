namespace equilog_backend.Common;

public struct Unit
{
    public static readonly Unit Value = new Unit();
    
    public override bool Equals(object? obj) => obj is Unit;
    public override int GetHashCode() => 0;
    
    public override string ToString() => "()";
    
    public static bool operator ==(Unit left, Unit right) => true;
    public static bool operator !=(Unit left, Unit right) => false;
}