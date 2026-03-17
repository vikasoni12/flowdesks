namespace Flowdesks.Application.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class IgnoreField : Attribute
    {
        public bool Ignore { get; }

        public IgnoreField(bool ignore)
        {
            Ignore = ignore;
        }
    }
}
