namespace BablTest
{
    public class BaseParityAttribute : BaseAttribute
    {
        public BaseParityAttribute(int order = 2)
            : base(order, "Parity") { }
    }
}
