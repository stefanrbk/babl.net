using System;
using System.Collections.Generic;

namespace babl
{
    class BablDb
    {
        readonly Dictionary<string, Babl> names = new();
        readonly Dictionary<int, Babl> ids = new();
        readonly List<Babl> babls = new();
        readonly object mutex = new();

        public Babl? Find(string name) =>
            names.TryGetValue(name, out var value) ? value : null;

        public Babl? Find(int id) =>
            ids.TryGetValue(id, out var value) ? value : null;

        public int Count => babls.Count;

        public Babl Insert(Babl babl)
        {
            lock(mutex)
            {
                if (babl.Id != 0)
                    ids.Add((int)babl.Id, babl);
                names.Add(babl.Name, babl);
                babls.Add(babl);
            }
            return babl;
        }

        public void ForEach(BablEachFunc action)
        {
            List<Babl> babls;
            lock(mutex)
            {
                babls = new(this.babls);
            }

            foreach (var babl in babls)
                action(babl);
        }
    }
}
