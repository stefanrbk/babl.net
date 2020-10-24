using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace babl
{
    internal class BablDb: IEnumerable<Babl>
    {
        private static readonly IEqualityComparer<string> comparer = new StringComparer();

        private readonly Dictionary<string, Babl> byName = new Dictionary<string, Babl>(comparer);
        private readonly Dictionary<int, Babl> byId = new Dictionary<int, Babl>();
        private readonly List<Babl> babls = new List<Babl>();
        private readonly object mutex = new object();

        public Babl Find(string name) =>
            byName[name];
        public int Count =>
            babls.Count;

        public void Insert(Babl item)
        {
            lock(mutex)
            {
                if (item.Id is not 0)
                    byId.Add(item.Id, item);
                byName.Add(item.Name, item);
                babls.Add(item);
            }
        }

        public Babl? Exists(int id, string name) =>
            id is not 0 ?
                Exists(id) :
                Exists(name);

        public Babl? Exists(int id) =>
            byId.TryGetValue(id, out var value) ?
                value :
                null;
        public Babl? Exists(string name) =>
            byName.TryGetValue(name, out var value) ?
                value :
                null;

        public IEnumerator<Babl> GetEnumerator() =>
            babls.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() =>
            GetEnumerator();
    }
}
