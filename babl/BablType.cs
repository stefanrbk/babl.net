using System;

namespace babl
{
    internal class BablType : Babl
    {
        static readonly BablDb db = new();

        public BablType(string name, int id, int bits, string docs = "") :
            base(ClassType.Type, name, id, docs)
        {
            Bits = bits;
        }
        public BablType(string name, BablId id, int bits, string docs = "") :
            this(name, (int)id, bits, docs) { }

        public int Bits { get; }

        public static Babl? Find(int id) =>
            db.Find(id);

        public static Babl? Find(string name) => 
            db.Find(name);

        internal static void InitBase()
        {
            db.Insert(new BablType("double", BablId.Double, bits: 64, docs: "IEEE 754 double precision."));
        }

        public override int GetHashCode() =>
            HashCode.Combine(Bits);

        public bool Equals(BablType other) => 
            Bits == other.Bits;

        public override bool Equals(object? obj) =>
            obj is BablType babl && Equals(babl);
    }
}
