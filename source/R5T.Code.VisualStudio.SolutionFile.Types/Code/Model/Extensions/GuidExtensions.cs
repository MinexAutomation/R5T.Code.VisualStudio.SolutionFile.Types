using System;


namespace R5T.Code.VisualStudio.Model
{
    public static class GuidExtensions
    {
        public static string ToStringSolutionFileFormat(this Guid guid)
        {
            var output = guid.ToString("B").ToUpperInvariant();
            return output;
        }
    }
}
