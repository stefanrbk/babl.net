
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace BablTest
{
    public abstract class BablBaseAttribute : OrderAttribute, IApplyToTest
    {
        readonly CategoryAttribute cat1;
        readonly CategoryAttribute cat2;

        protected BablBaseAttribute(int order, string category1, string category2)
            : base(order)
        {
            cat1 = new(category1);
            cat2 = new(category2);
        }

        public new void ApplyToTest(Test test)
        {
            base.ApplyToTest(test);
            cat1.ApplyToTest(test);
            cat2.ApplyToTest(test);
        }
    }
}
