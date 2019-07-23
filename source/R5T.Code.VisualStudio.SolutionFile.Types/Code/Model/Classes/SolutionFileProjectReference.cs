using System;

using PathUtilities = R5T.NetStandard.IO.Paths.Utilities;
using SolutionFileConstants = R5T.Code.VisualStudio.Model.SolutionFileSpecific.Constants;
using VsPathUtilities = R5T.Code.VisualStudio.IO.Utilities;


namespace R5T.Code.VisualStudio.Model
{
    /// <summary>
    /// Example:
    /// Project("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}") = "R5T.Private.SimpleConsoleAndLib", "R5T.Private.SimpleConsoleAndLib\R5T.Private.SimpleConsoleAndLib.csproj", "{9DAD5F24-3C22-47C9-8D69-3C7D72C62DAD}"
    /// EndProject
    /// </summary>
    public class SolutionFileProjectReference
    {
        #region Static

        public static SolutionFileProjectReference NewNetCoreOrStandardFromProjectFileRelativePath(string projectName, string projectFileRelativePathValue)
        {
            var solutionFileProjectReference = new SolutionFileProjectReference
            {
                ProjectGUID = Guid.NewGuid(),
                ProjectName = projectName,
                ProjectFileRelativePathValue = projectFileRelativePathValue,
                ProjectTypeGUID = SolutionFileConstants.NetStandardLibraryProjectTypeGUID
            };
            return solutionFileProjectReference;
        }

        public static SolutionFileProjectReference NewNetCoreOrStandard(string solutionFilePath, string projectFilePath)
        {
            var solutionDirectoryPath = PathUtilities.GetDirectoryPath(solutionFilePath);

            var solutionDirectoryToDependencyProjectRelativeFilePath = PathUtilities.GetRelativePathDirectoryToFile(solutionDirectoryPath, projectFilePath);

            var projectName = VsPathUtilities.GetProjectName(projectFilePath);

            var solutionFileProjectReference = SolutionFileProjectReference.NewNetCoreOrStandardFromProjectFileRelativePath(projectName, solutionDirectoryToDependencyProjectRelativeFilePath);
            return solutionFileProjectReference;
        }

        #endregion


        public Guid ProjectTypeGUID { get; set; }
        public string ProjectName { get; set; }
        public string ProjectFileRelativePathValue { get; set; }
        public Guid ProjectGUID { get; set; }
    }
}
