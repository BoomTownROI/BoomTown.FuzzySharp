namespace BoomTown.FuzzySharp.InternalDiffUtils.Models
{
    internal class EditOperations
    {
        public EditType Type { get; set; }
        /// <summary>
        /// Source Block Position
        /// </summary>
        public int Spos { get; set; }
        /// <summary>
        /// Destination Block Position
        /// </summary>
        public int Dpos { get; set; } 
    }
    
    internal enum EditType {
        Delete,
        Equal,
        Insert,
        Replace,
        Keep
    }
}