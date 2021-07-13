namespace BablTest
{
    public abstract class BaseAttribute : BablBaseAttribute
    {
        protected BaseAttribute(int order, string category)
            : base(order, "Base", category) { }
    }
}
