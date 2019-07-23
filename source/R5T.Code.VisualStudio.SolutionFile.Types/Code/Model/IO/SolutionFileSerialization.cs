using System;

using R5T.NetStandard.IO;

using R5T.Code.VisualStudio.Model;


namespace R5T.Code.VisualStudio.IO
{
    public static class SolutionFileSerialization
    {
        public static SolutionFile Deserialize(string solutionFilePath)
        {
            using (var fileStream = FileStreamHelper.NewRead(solutionFilePath))
            using (var textReader = StreamReaderHelper.NewLeaveOpen(fileStream))
            {
                var solutionFileTextSerialializer = new SolutionFileTextSerializer();

                var solutionFile = solutionFileTextSerialializer.Deserialize(textReader);
                return solutionFile;
            }
        }

        public static void Serialize(string solutionFilePath, SolutionFile obj, bool overwrite = true)
        {
            using (var fileStream = FileStreamHelper.NewWrite(solutionFilePath, overwrite))
            using (var textWriter = StreamWriterHelper.NewLeaveOpenAddBOM(fileStream))
            {
                var solutionFileTextSerialializer = new SolutionFileTextSerializer();

                solutionFileTextSerialializer.Serialize(textWriter, obj);
            }
        }
    }
}
