namespace DiffApi.Models
{
    /// <summary>
    /// Class used in the final JSON result. Shows up the actual difference from the comparison.
    /// </summary>
    public class Difference
    {
        public int Offset { get; }
        public int Length { get; }

        public Difference(int offset, int length)
        {
            Offset = offset;
            Length = length;
        }
    }
}