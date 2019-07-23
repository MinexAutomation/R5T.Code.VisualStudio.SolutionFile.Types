using System;
using System.Linq;

using R5T.Code.VisualStudio.Model.SolutionFileSpecific;

using PathUtilities = R5T.NetStandard.IO.Paths.Utilities;
using VsPathUtilities = R5T.Code.VisualStudio.IO.StringUtilities;


namespace R5T.Code.VisualStudio.Model
{
    public static class SolutionFileExtensions
    {
        public static void Save(this SolutionFile solutionFile, string filePath)
        {
            SolutionFile.Save(filePath, solutionFile);
        }

        public static bool HasDependenciesSolutionFolder(this SolutionFile solutionFile, out SolutionFileProjectReference dependenciesSolutionFolder)
        {
            dependenciesSolutionFolder = solutionFile.SolutionFileProjectReferences.Where(x => x.ProjectName == Constants.DependenciesSolutionFolderName).SingleOrDefault();

            var hasDependenciesSolutionFolder = dependenciesSolutionFolder != default;
            return hasDependenciesSolutionFolder;
        }

        public static bool HasDependenciesSolutionFolder(this SolutionFile solutionFile)
        {
            var hasDependenciesSolutionFolder = solutionFile.HasDependenciesSolutionFolder(out var _);
            return hasDependenciesSolutionFolder;
        }

        public static SolutionFileProjectReference GetDependenciesSolutionFolder(this SolutionFile solutionFile)
        {
            var hasDependenciesSolutionFolder = solutionFile.HasDependenciesSolutionFolder(out var dependenciesSolutionFolder);
            if(!hasDependenciesSolutionFolder)
            {
                throw new InvalidOperationException($"Solution file had no {Constants.DependenciesSolutionFolderName} solution folder.");
            }

            return dependenciesSolutionFolder;
        }

        public static SolutionFileProjectReference AddDependenciesSolutionFolder(this SolutionFile solutionFile)
        {
            var dependenciesSolutionFolder = new SolutionFileProjectReference
            {
                ProjectTypeGUID = Constants.SolutionFolderProjectTypeGUID,
                ProjectName = Constants.DependenciesSolutionFolderName,
                ProjectFileRelativePathValue = Constants.DependenciesSolutionFolderName,
                ProjectGUID = Guid.NewGuid()
            };

            solutionFile.SolutionFileProjectReferences.Add(dependenciesSolutionFolder);

            return dependenciesSolutionFolder;
        }

        public static SolutionFileProjectReference AcquireDependenciesSolutionFolder(this SolutionFile solutionFile)
        {
            if(!solutionFile.HasDependenciesSolutionFolder(out var dependenciesSolutionFolder))
            {
                dependenciesSolutionFolder = solutionFile.AddDependenciesSolutionFolder();
            }

            return dependenciesSolutionFolder;
        }

        public static void RemoveDependenciesSolutionFolder(this SolutionFile solutionFile)
        {
            var dependenciesSolutionFolder = solutionFile.GetDependenciesSolutionFolder();

            solutionFile.SolutionFileProjectReferences.Remove(dependenciesSolutionFolder);
        }

        public static void AddProjectNesting(this SolutionFile solutionFile, ProjectNesting projectNesting)
        {
            var nestedProjectsGlobalSection = solutionFile.GlobalSections.AcquireNestedProjectsGlobalSection();

            nestedProjectsGlobalSection.ProjectNestings.Add(projectNesting);
        }

        public static void AddProjectToDependenciesSolutionFolder(this SolutionFile solutionFile, SolutionFileProjectReference projectReference)
        {
            var dependenciesSolutionFolder = solutionFile.AcquireDependenciesSolutionFolder();

            var projectNesting = new ProjectNesting { ProjectGUID = projectReference.ProjectGUID, ParentProjectGUID = dependenciesSolutionFolder.ProjectGUID };

            solutionFile.AddProjectNesting(projectNesting);
        }

        public static void AddProjectReference(this SolutionFile solutionFile, string solutionFilePath, string projectFilePath)
        {
            var projectReference = SolutionFileProjectReference.NewNetCoreOrStandard(solutionFilePath, projectFilePath);

            solutionFile.AddProjectReference(projectReference);
        }

        public static void AddProjectReference(this SolutionFile solutionFile, SolutionFileProjectReference projectReference)
        {
            solutionFile.SolutionFileProjectReferences.Add(projectReference);

            var solutionConfigurationPlatforms = solutionFile.GlobalSections.AcquireSolutionConfigurationPlatformsGlobalSection();

            var projectConfigurationPlatforms = solutionFile.GlobalSections.AcquireProjectConfigurationPlatformsGlobalSection();

            projectConfigurationPlatforms.AddProjectConfigurations(projectReference.ProjectGUID, solutionConfigurationPlatforms);

            // Somehow, adding a project to a solution in VS adds the Extensibility Globals section. So ensure that the solution file has the Extensibility Globals section.
            solutionFile.EnsureHasExtensibilityGlobals();
        }

        public static void EnsureHasExtensibilityGlobals(this SolutionFile solutionFile)
        {
            var hasExtensiblityGlobals = solutionFile.GlobalSections.HasGlobalSectionByName<GeneralSolutionFileGlobalSection>(Constants.ExtensibilityGlobalsSolutionGlobalSectionName, out var extensibilityGlobals);
            if(!hasExtensiblityGlobals)
            {
                solutionFile.GlobalSections.AddGlobalSection(SolutionFile.CreateExtensibilityGlobals);
            }
        }

        public static void AddProjectReferenceAsDependency(this SolutionFile solutionFile, string solutionFilePath, string projectFilePath)
        {
            var projectReference = SolutionFileProjectReference.NewNetCoreOrStandard(solutionFilePath, projectFilePath);

            solutionFile.AddProjectReferenceAsDependency(projectReference);
        }

        public static void AddProjectReferenceAsDependency(this SolutionFile solutionFile, SolutionFileProjectReference projectReference)
        {
            solutionFile.AddProjectReference(projectReference);

            solutionFile.AddProjectToDependenciesSolutionFolder(projectReference);
        }
    }
}
