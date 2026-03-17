namespace Flowdesks.Application.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class IsRequiredField : Attribute
    {
        public bool IsDefault { get; }

        public IsRequiredField(bool isDefault)
        {
            IsDefault = isDefault;
        }
    }
}
