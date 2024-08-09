namespace CircuitSim.Core.Annotations
{
    /// <summary>
    /// Hides the class from being a selectable component.
    /// </summary>
    [System.AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public sealed class HiddenAttribute : Attribute
    {
        public HiddenAttribute()
        {
        }
    }
}
