using System;

namespace babl
{
#if DEBUG
    public
#endif
        class BablComponent : Babl
    {
#if DEBUG
        internal
#endif
        static readonly BablDb db = new();

        internal BablComponent(string name, int id, bool luma = false, bool chroma = false, bool alpha = false, string docs = "") :
            base(name, id, docs)
        {
            Luma = luma;
            Chroma = chroma;
            Alpha = alpha;
        }

        internal static Babl New(BablId id, string name = "", bool luma = false, bool chroma = false, bool alpha = false, string docs = "") =>
            New(name, (int)id, luma, chroma, alpha, docs);

        internal static Babl New(string name = "", int id = 0, bool luma = false, bool chroma = false, bool alpha = false, string docs = "")
        {
            return (id != 0
                ? Find(id)
                : Find(name))
                ?? db.Insert(new BablComponent(name, id, luma, chroma, alpha, docs));
        }

#if DEBUG
        public
#else
        internal
#endif
        bool Luma { get; }

#if DEBUG
        public
#else
        internal
#endif
        bool Chroma { get; }

#if DEBUG
        public
#else
        internal
#endif
        bool Alpha { get; }

        internal static Babl? Find(int id) =>
            db.Find(id);

        internal static Babl? Find(string name) =>
            db.Find(name);

        internal static void InitBase()
        {
            db.Insert(new BablComponent("R", (int)BablId.Red, luma: true, chroma: true));
            db.Insert(new BablComponent("G", (int)BablId.Green, luma: true, chroma: true));
            db.Insert(new BablComponent("B", (int)BablId.Blue, luma: true, chroma: true));
            db.Insert(new BablComponent("A", (int)BablId.Alpha, alpha: true));
            db.Insert(new BablComponent("PAD", (int)BablId.Padding));
        }


#if DEBUG
        public
#else
        internal
#endif
        static void ForEach(BablEachFunc action) =>
            db.ForEach(action);

        internal bool Equals(BablComponent other) =>
            Luma == other.Luma && Chroma == other.Chroma && Alpha == other.Alpha;

        public override bool Equals(object? obj) =>
            obj is BablComponent babl && Equals(babl);

        public override int GetHashCode() =>
            HashCode.Combine(Luma, Chroma, Alpha);
    }
}
