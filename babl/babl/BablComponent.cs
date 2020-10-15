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

        private BablComponent(string name,
                              int id,
                              bool hasLuma,
                              bool hasChroma,
                              bool hasAlpha,
                              string doc)
        {
            Name = name;
            ClassType = BablClassType.Component;
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
            if (id != 0 && value is not null && db.Exists(name) is not null)
                throw new Exception($"Trying to reregister BablComponent '{name}' with different id!");

            if (value is not null)
            {
                // There is an instance already registerd by the required id/name,
                // returning the preexistent one instead if it doesn't differ.
                if (!((BablComponent)value).Equals(hasLuma, hasChroma, hasAlpha))
                    throw new Exception($"BablComponent '{name}' already registerd with different attributes!");
                return value;
            }
            value = new BablComponent(name, id, hasLuma, hasChroma, hasAlpha, doc);

            // Since there is not an already registered instance by the required
            // id/name, inserting newly created class into database.
            db.Insert(value);
            return value;
        }

        public bool Equals(bool hasLuma, bool hasChroma, bool hasAlpha) =>
            HasLuma == hasLuma &&
            HasChroma == hasChroma &&
            HasAlpha == hasAlpha;

        public override bool Equals(object? obj) =>
            obj is BablComponent babl &&
            babl.Equals(HasLuma, HasChroma, HasAlpha);
        public override int GetHashCode() =>
            HashCode.Combine(HasLuma, HasChroma, HasAlpha);

        public static Babl? Find(string name)
        {
            if (logOnNameLookups)
                Log($"\"{name}\": looking up");
            if (db is null)
                Error($"\"{name}\": you must call Babl.Init first");
            var babl = db.Exists(name);

            if (babl is null)
                Error($"\"{name}\": not found");

            return babl;
        }
    }
}
