using System;
using System.Collections.Generic;
using System.Text;

namespace babl
{
    internal class BablComponent : Babl
    {
        private readonly static BablDb db = new BablDb();
        internal bool HasLuma { get; set; }
        internal bool HasChroma { get; set; }
        internal bool HasAlpha { get; set; }
        internal override BablClassType ClassType => BablClassType.Component;

        private BablComponent(string name,
                              int id,
                              bool hasLuma,
                              bool hasChroma,
                              bool hasAlpha,
                              string doc)
        {
            Name = name;
            Id = id;
            Doc = doc;
            HasLuma = hasLuma;
            HasChroma = hasChroma;
            HasAlpha = hasAlpha;
        }
        internal static Babl Create(string name,
                                    int id,
                                    bool hasLuma = false,
                                    bool hasChroma = false,
                                    bool hasAlpha = false,
                                    string doc = "")
        {
            var value = db.Exists(id, name);
            if (id is not 0 && value is null && db.Exists(name) is not null)
                Fatal.AlreadyRegistered(name, nameof(BablComponent));

            if (value is not null)
            {
                // There is an instance already registerd by the required id/name,
                // returning the preexistent one instead if it doesn't differ.
                if (!((BablComponent)value).Equals(hasLuma, hasChroma, hasAlpha))
                    Fatal.ExistsAsDifferentValue(name, nameof(BablComponent));
                return value;
            }
            value = new BablComponent(name, id, hasLuma, hasChroma, hasAlpha, doc);

            // Since there is not an already registered instance by the required
            // id/name, inserting newly created class into database.
            db.Insert(value);
            return value;
        }

        public bool Equals(bool hasLuma,
                           bool hasChroma,
                           bool hasAlpha) =>
            HasLuma == hasLuma &&
            HasChroma == hasChroma &&
            HasAlpha == hasAlpha;

        public override bool Equals(object? obj) =>
            obj is BablComponent babl &&
            babl.Equals(HasLuma, HasChroma, HasAlpha);
        public override int GetHashCode() =>
            HashCode.Combine(HasLuma, HasChroma, HasAlpha);

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
    }
}
