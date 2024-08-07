namespace CircuitSim.Core.Annotations;

[System.AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
public sealed class PropertyEditableAttribute : Attribute
{
    public PropertyEditableAttribute()
    {

    }
}
