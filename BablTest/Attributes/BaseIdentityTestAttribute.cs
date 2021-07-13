namespace BablTest
{
    public class BaseIdentityAttribute : BaseAttribute
    {
        public BaseIdentityAttribute(int order = 3)
            : base(order, "Identity") { }
    }
}
