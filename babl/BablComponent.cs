using System;

namespace babl
{
    class BablComponent : Babl
    {
        static readonly BablDb db = new();

        public BablComponent(string name, int id, bool luma = false, bool chroma = false, bool alpha = false) :
            base(ClassType.Component, name, id)
        {
            Luma = luma;
            Chroma = chroma;
            Alpha = alpha;
        }

        public bool Luma { get; }
        public bool Chroma { get; }
        public bool Alpha { get; }

        public static Babl? FromId(int id) =>
            db.Find(id);

        internal static void InitBase()
        {
            db.Insert(new BablComponent("R", (int)BablId.Red, luma: true, chroma: true));
            db.Insert(new BablComponent("G", (int)BablId.Green, luma: true, chroma: true));
            db.Insert(new BablComponent("B", (int)BablId.Blue, luma: true, chroma: true));
            db.Insert(new BablComponent("A", (int)BablId.Alpha, alpha: true));
            db.Insert(new BablComponent("PAD", (int)BablId.Padding));
        }

        public static void ForEach(Action<Babl> action) =>
            db.ForEach(action);

        public bool Equals(BablComponent other) =>
            Luma == other.Luma && Chroma == other.Chroma && Alpha == other.Alpha;

        public override bool Equals(object? obj) =>
            obj is BablComponent babl && this.Equals(babl);

        public override int GetHashCode() =>
            HashCode.Combine(Luma, Chroma, Alpha);
    }
}
