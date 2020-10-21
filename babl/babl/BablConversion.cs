using Microsoft.VisualBasic.CompilerServices;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace babl
{
    internal class BablConversion : Babl
    {
        private const int FromListInitialSize = 5;

        private static int Collisions;
        private static readonly BablDb db = new BablDb();

        internal override BablClassType ClassType => type;
        private readonly BablClassType type = BablClassType.Conversion;

        internal Babl Source { get; set; }
        internal Babl Destination { get; set; }
        internal FuncDispatch Dispatch { get; set; } = null!;
        internal object? Data { get; set; }
        internal long Cost { get; set; }
        internal double ErrorMargin { get; set; }
        internal FuncPlane? Plane { get; set; }
        internal FuncLinear? Linear { get; set; }
        internal FuncPlanar? Planar { get; set; }
        internal long Pixels { get; set; }

        private BablConversion(string name, int id, Babl source, Babl destination, FuncLinear? linear = null, FuncPlane? plane = null, FuncPlanar? planar = null, object? data = null, bool /*allowCollision*/ _ = false)
        {
            Assert(source.ClassType == destination.ClassType);
            Name = name;

            if (linear is not null)
            {
                type = BablClassType.ConversionLinear;
                Linear = linear;
            }
            else if (plane is not null)
            {
                type = BablClassType.ConversionPlane;
                Plane = plane;
            }
            else if (planar is not null)
            {
                type = BablClassType.ConversionPlanar;
                Planar = planar;
            }
            switch (source.ClassType)
            {
                case BablClassType.Type:
                    if (linear is not null)
                        Error($"linear conversions not supported for {source.ClassType}");
                    else if (planar is not null)
                        Error($"planar conversions not supported for {source.ClassType}");
                    break;
                // TODO: Model and Format cases needed here
                default:
                    Error($"{source.ClassType} unexpected");
                    break;
            }

            Id = id;
            Source = source;
            Destination = destination;
            ErrorMargin = -1.0;
            Cost = 69;
            Pixels = 0;
            Data = data;

            // TODO: Fish needed to complete Conversion init.
        }

        private static string CreateName(Babl source, Babl destination, BablClassType type)
        {
            // TODO: Extender needs to be handled here
            /*if (Extender is not null)
            {

            }
            else*/
            {
                return $"{(type is BablClassType.ConversionLinear ? "" : type is BablClassType.ConversionPlane ? "plane " : type is BablClassType.ConversionPlanar ? "planar " : "Eeeek!")} {source.Name} to {destination.Name} #{Collisions}";
            }
        }

        public static string CreateName(Babl source, Babl destination, BablClassType type, bool allowCollisions)
        {
            var id = 0;
            Collisions = 0;
            var name = CreateName(source, destination, type);

            if (!allowCollisions)
            {
                var babl = db.Exists(id, name);
                while (babl is not null)
                {
                    // We allow multiple conversions to be registered per extender, each of them ending up with their own unique name
                    Collisions++;
                    name = CreateName(source, destination, type);
                    babl = db.Exists(id, name);
                }
            }

            return name;
        }

        public static Babl? Create(Babl source,
                                   Babl destination,
                                   int id = 0,
                                   object? data = null,
                                   bool allowCollisions = false,
                                   FuncLinear? linear = null,
                                   FuncPlane? plane = null,
                                   FuncPlanar? planar = null)
        {
            Assert(IsBabl(source));
            Assert(IsBabl(destination));

            var sourceType = source.AsType;
            var destinationType = destination.AsType;
            Assert(sourceType is not null);
            Assert(destinationType is not null);

            var type = linear is not null
                           ? BablClassType.ConversionLinear
                           : plane is not null
                               ? BablClassType.ConversionPlane
                               : planar is not null
                                   ? BablClassType.ConversionPlanar
                                   : BablClassType.Conversion;

            var name = CreateName(source, destination, type, allowCollisions);

            var babl = new BablConversion(name, id, source, destination, linear, plane, planar, data, allowCollisions);

            db.Insert(babl);

            if (sourceType.FromList is null)
                sourceType.FromList = new List<BablConversion>(FromListInitialSize);
            sourceType.FromList.Add(babl);
            return babl;
        }

        public void PlaneProcess(object source, object destination, int srcPitch, int dstPitch, long num, object userData)
        {
            if (Plane is not null)
                Plane(this, source, destination, srcPitch, dstPitch, num, userData);
        }

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
}
