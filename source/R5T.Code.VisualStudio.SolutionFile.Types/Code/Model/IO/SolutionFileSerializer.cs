using System;

using R5T.NetStandard.IO.Serialization;

using R5T.Code.VisualStudio.Model;


namespace R5T.Code.VisualStudio.IO
{
    /// <summary>
    /// Special solution file serializer that properly adds byte-order-marks (BOM) to solution file.
    /// </summary>
    public class SolutionFileSerializer : IFileSerializer<SolutionFile>
    {
        public SolutionFile Deserialize(string solutionFilePath)
        {
            var solutionFile = SolutionFileSerialization.Deserialize(solutionFilePath);
            return solutionFile;
        }

        public void Serialize(string solutionFilePath, SolutionFile obj, bool overwrite = true)
        {
            SolutionFileSerialization.Serialize(solutionFilePath, obj, overwrite);
        }
    }
}
