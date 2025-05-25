namespace equilog_backend.Common;


// Represents a void or empty return type for operations that don't return data.
// Similar to functional programming's Unit type - indicates successful completion without a value.
public struct Unit
{
    // Static instance to avoid repeated allocations - represents the single Unit value.
    public static readonly Unit Value = new();
    
    // All Unit instances are considered equal since they represent the same "empty" concept.
    public override bool Equals(object? obj) => obj is Unit;
    
    // All Unit instances have the same hash code since they're all equivalent.
    public override int GetHashCode() => 0;
    
    // String representation using functional programming convention for unit type.
    public override string ToString() => "()";
    
    // Equality operators - all Unit instances are equal to each other.
    public static bool operator ==(Unit left, Unit right) => true;
    public static bool operator !=(Unit left, Unit right) => false;
}