using System;
using Microsoft.Extensions.Logging;

namespace SaeedRezayi.LogModule.Utilities
{
    public static class RuntimeExtensions
    {

        /// <summary>
        /// get currently grabage collector information/state
        /// </summary>
        /// <param name="logger">logger instance (if you have a logger instance), otherwise write to console</param>
        /// <param name="ForceGcBefore">Do GC collect before get GC information</param>
        /// <param name="generation">max generation if you want do do GC, default is 2(all)</param>
        public static void GetGCInfo(this ILogger logger, bool ForceGcBefore = false, int generation = 2)
        {
            var GcInfo = GC.GetGCMemoryInfo();

            if (logger != null)
            {
                logger.LogWarning($"GC HighMemoryLoad Threshold(byte): {GcInfo.HighMemoryLoadThresholdBytes}");
                logger.LogWarning($"GC Memory Frags(byte): {GcInfo.FragmentedBytes}");
                logger.LogWarning($"GC Heap Size(byte): {GcInfo.HeapSizeBytes}");
                logger.LogWarning($"GC Total Allocated Memory(byte): {GC.GetTotalMemory(ForceGcBefore)}");
            }
            else
            {
                Console.WriteLine($"GC HighMemoryLoad Threshold(byte): {GcInfo.HighMemoryLoadThresholdBytes}");
                Console.WriteLine($"GC Memory Frags(byte): {GcInfo.FragmentedBytes}");
                Console.WriteLine($"GC Heap Size(byte): {GcInfo.HeapSizeBytes}");
                Console.WriteLine($"GC Total Allocated Memory(byte): {GC.GetTotalMemory(ForceGcBefore)}");
            }
        }
    }
}
