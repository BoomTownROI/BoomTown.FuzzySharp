namespace BoomTown.FuzzySharp.InternalDiffUtils.Models
{
    internal class MatchingBlock
    {
        /// <summary>
        /// // Source Block Position
        /// </summary>
        public int Spos { get; set; } 
        /// <summary>
        /// Destination Block Position
        /// </summary>
        public int Dpos { get; set; } 
        /// <summary>
        /// Block Length
        /// </summary>
        public int Length { get; set; }
    }
}