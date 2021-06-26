using System;

namespace babl
{
    class BablComponent : Babl
    {
        bool luma;
        bool chroma;
        bool alpha;

        static readonly BablDb db = new();

        public BablComponent(string name, int id, bool luma, bool chroma, bool alpha) :
            base(ClassType.Component, name, id)
        {
            this.luma = luma;
            this.chroma = chroma;
            this.alpha = alpha;
        }

        public static Babl? FromId(int id) =>
            db.Find(id);

        public static void ForEach(Action<Babl> action) =>
            db.ForEach(action);

        public bool Equals(BablComponent other) =>
            luma == other.luma && chroma == other.chroma && alpha == other.alpha;

        public override bool Equals(object? obj) =>
            obj is BablComponent babl && this.Equals(babl);

        public override int GetHashCode() =>
            HashCode.Combine(luma, chroma, alpha);
    }
}
