using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static babl.Babl;
using static babl.Ids;
namespace babl.Init
{
    internal static partial class Core
    {
        private static (ushort min, ushort max) U15Ushort= (0, 32768);
        private static (double min, double max) U15Double = (0.0, 1.0);
        private static (float min, float max) U15Float = (0.0f, 1.0f);
        private static double rint(double value) =>
            Math.Floor(value + 0.5);

        private static void ConvertU15Double(Babl _1, object src, object dst, int srcPitch, int dstPitch, 
                                             long num, object? _2) =>
                Convert<ushort, double>(src, dst, srcPitch, dstPitch, num, ScaleU15Double);

        private static void ConvertDoubleU15(Babl _1, object src, object dst, int srcPitch, int dstPitch,
                                             long num, object? _2) =>
            Convert<double, ushort>(src, dst, srcPitch, dstPitch, num, ScaleDoubleU15);

        private static void ConvertU15Float(Babl _1, object src, object dst, int srcPitch, int dstPitch,
                                            long num, object? _2) =>
                Convert<ushort, float>(src, dst, srcPitch, dstPitch, num, ScaleU15Float);

        private static void ConvertFloatU15(Babl _1, object src, object dst, int srcPitch, int dstPitch,
                                            long num,object? _2) =>
            Convert<float, ushort>(src, dst, srcPitch, dstPitch, num, ScaleFloatU15);

        private static double ScaleU15Double(ushort value) =>
            Scale(value, U15Ushort, U15Double, v => v / (double)U15Ushort.max);
        private static ushort ScaleDoubleU15(double value) =>
            Scale(value, U15Double, U15Ushort, v => (ushort)rint(v * U15Ushort.max));

        private static float ScaleU15Float(ushort value) =>
            Scale(value, U15Ushort, U15Float, v => v / (float)U15Ushort.max);
        private static ushort ScaleFloatU15(float value) =>
            Scale(value, U15Float, U15Ushort, v => (ushort)rint(v * U15Ushort.max));

        private static void TypeU15Init()
        {
            logOnNameLookups = false;

            CreateType("u15", bits: 16);

            CreateConversion(Type("u15"), Type(Ids.Double), plane: ConvertU15Double);
            CreateConversion(Type(Ids.Double), Type("u15"), plane: ConvertDoubleU15);
            CreateConversion(Type("u15"), Type(Float), plane: ConvertU15Float);
            CreateConversion(Type(Float), Type("u15"), plane: ConvertFloatU15);

            logOnNameLookups = true;
        }
    }
}
