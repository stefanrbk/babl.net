using System;
using System.Collections.Generic;

namespace babl
{
    internal class BablType : Babl
    {
        private readonly static BablDb db = new BablDb();
        internal const double Tolerance = 1e-9;

        internal int Bits { get; set; }
        internal double MinValue { get; set; }
        internal double MaxValue { get; set; }
        internal override BablClassType ClassType => BablClassType.Type;

        internal static Babl Create(string name,
                                    int id,
                                    int bits,
                                    bool integer,
                                    bool unsigned,
                                    long min,
                                    long max,
                                    double minVal,
                                    double maxVal,
                                    string doc = "")
        {
            var value = db.Exists(id, name);
            if (id is not 0 && value is null && db.Exists(name) is not null)
                Fatal.AlreadyRegistered(name, nameof(BablType));

            if (value is not null)
            {
                // There is an instance already registerd by the required id/name,
                // returning the preexistent one instead if it doesn't differ.
                if (!((BablType)value).Equals(bits))
                    Fatal.ExistsAsDifferentValue(name, nameof(BablType));
                return value;
            }
            value = integer
                ? new BablTypeInteger()
                {
                    Name = name,
                    Id = id,
                    Bits = bits,
                    Doc = doc,
                    IsSigned = !unsigned,
                    Max = max,
                    Min = min,
                    MaxValue = maxVal,
                    MinValue = minVal
                }
                : new BablType()
                {
                    Name = name,
                    Id = id,
                    Bits = bits,
                    Doc = doc,
                    MinValue = minVal,
                    MaxValue = maxVal
                };
            db.Insert(value);
            return value;
        }

        public bool Equals(int bits) =>
            Bits == bits;

        public override bool Equals(object? obj) =>
            obj is BablType babl &&
            babl.Equals(Bits);

        public override int GetHashCode() =>
            HashCode.Combine(Bits);

        internal static Babl Find(string name)
        {
            if (logOnNameLookups)
                Logging.LookingUp(name);
            var babl = db.Exists(name);

            if (babl is null)
                Fatal.NotFound(name);

            return babl;
        }

        internal static Babl Find(int id)
        {
            var babl = db.Exists(id);
            if (babl is null)
                Fatal.NotFound(id.ToString());
            return babl;
        }

        public static void ForEach(Action<Babl> action)
        {
            foreach (var entry in db)
                action(entry);
        }
    }

    internal class BablTypeInteger : BablType
    {
        internal bool IsSigned { get; set; }
        internal long Max { get; set; }
        internal long Min { get; set; }
    }

    internal class BablTypeFloat : BablType
    {

    }
}
