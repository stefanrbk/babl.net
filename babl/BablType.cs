using System;

namespace babl
{
#if DEBUG
    public
#else
    internal
#endif
    class BablType : Babl
    {
        static readonly BablDb db = new();

        public BablType(string name, int id, int bits, string docs = "") :
            base(name, id, docs)
        {
            Bits = bits;
        }
        public BablType(string name, BablId id, int bits, string docs = "") :
            this(name, (int)id, bits, docs)
        { }

        public static Babl New(string name = "", BablId id = 0, int bits = 0, string docs = "") =>
            New(name, (int)id, bits, docs);

        public static Babl New(string name = "", int id = 0, int bits = 0, string docs = "")
        {
            return (id != 0
                ? Find(id)
                : Find(name))
                ?? db.Insert(new BablType(name, id, bits, docs));
        }

        public int Bits { get; }

        public static Babl? Find(int id) =>
            db.Find(id);

        public static Babl? Find(string name) =>
            db.Find(name);

        internal static void InitBase()
        {
            db.Insert(new BablType("double", BablId.Double, bits: 64, docs: "IEEE 754 double precision."));
        }

        public static void ForEach(BablEachFunc action) =>
            db.ForEach(action);

        public override int GetHashCode() =>
            HashCode.Combine(Bits);

        public bool Equals(BablType other) =>
            Bits == other.Bits;

        public override bool Equals(object? obj) =>
            obj is BablType babl && Equals(babl);

#if DEBUG
        public static void Remove(BablType type) =>
            db.Remove(type);
#endif
    }
}
