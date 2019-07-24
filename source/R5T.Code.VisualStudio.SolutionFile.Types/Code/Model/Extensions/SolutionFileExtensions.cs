using System;
using System.Collections.Generic;
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

        /// <summary>
        /// Adds a project reference to the solution's dependencies folder, but checks first to avoid adding duplicates.
        /// </summary>
        public static void AddProjectReferenceDependencyChecked(this SolutionFile solutionFile, SolutionFileProjectReference projectReference)
        {
            var hasProjectReference = solutionFile.HasProjectReference(projectReference);
            if(!hasProjectReference)
            {
                solutionFile.AddProjectReferenceAsDependency(projectReference);
            }
        }

        /// <summary>
        /// Adds a project reference to the solution's dependencies folder, but checks first to avoid adding duplicates.
        /// </summary>
        public static void AddProjectReferenceDependencyChecked(this SolutionFile solutionFile, string solutionFilePath, string projectFilePath)
        {
            var projectReference = SolutionFileProjectReference.NewNetCoreOrStandard(solutionFilePath, projectFilePath);

            solutionFile.AddProjectReferenceDependencyChecked(projectReference);
        }

        public static void AddProjectReferenceDependenciesChecked(this SolutionFile solutionFile, string solutionFilePath, IEnumerable<string> projectFilePaths)
        {
            foreach (var projectFilePath in projectFilePaths)
            {
                solutionFile.AddProjectReferenceDependencyChecked(solutionFilePath, projectFilePath);
            }
        }

        public static bool HasProjectReferenceByProjectFileRelativePath(this SolutionFile solutionFile, string projectFileRelativePath)
        {
            var hasProjectReference = solutionFile.SolutionFileProjectReferences.Where(x => x.ProjectFileRelativePathValue == projectFileRelativePath).Any();
            return hasProjectReference;
        }

        public static bool HasProjectReferenceByProjectFilePath(this SolutionFile solutionFile, string solutionFilePath, string projectFilePath)
        {
            var projectFileRelativePath = VsPathUtilities.GetProjectFileRelativeToSolutionDirectoryPath(solutionFilePath, projectFilePath);

            var hasProjectReference = solutionFile.HasProjectReferenceByProjectFileRelativePath(projectFileRelativePath);
            return hasProjectReference;
        }

        public static bool HasProjectReference(this SolutionFile solutionFile, SolutionFileProjectReference projectReference)
        {
            var hasProjectReference = solutionFile.HasProjectReferenceByProjectFileRelativePath(projectReference.ProjectFileRelativePathValue);
            return hasProjectReference;
        }

        public static bool HasProjectReference(this SolutionFile solutionFile, string projectRelativeFilePath, out SolutionFileProjectReference projectReference)
        {
            projectReference = solutionFile.SolutionFileProjectReferences.Where(x => x.ProjectFileRelativePathValue == projectRelativeFilePath).SingleOrDefault();

            var hasProjectReference = projectReference != default;
            return hasProjectReference;
        }

        public static void AddProjectReferences(this SolutionFile solutionFile, IEnumerable<SolutionFileProjectReference> projectReferences)
        {
            foreach (var projectReference in projectReferences)
            {
                solutionFile.AddProjectReferenceDependencyChecked(projectReference);
            }
        }

        public static void AddProjectReferences(this SolutionFile solutionFile, string solutionFilePath, IEnumerable<string> projectFilePaths)
        {
            var projectReferences = projectFilePaths.Select(projectFilePath => SolutionFileProjectReference.NewNetCoreOrStandard(solutionFilePath, projectFilePath));

            solutionFile.AddProjectReferences(projectReferences);
        }

        /// <summary>
        /// Must also specify dependency project file paths because the solution file types assembly does not reference the project file types assembly.
        /// </summary>
        public static void AddProjectReferenceDependencyAndAllDependenciesChecked(this SolutionFile solutionFile, string solutionFilePath, string projectFilePath, IEnumerable<string> dependencyProjectFilePaths)
        {
            solutionFile.AddProjectReferenceDependencyChecked(solutionFilePath, projectFilePath);

            solutionFile.AddProjectReferenceDependenciesChecked(solutionFilePath, dependencyProjectFilePaths);
        }

        /// <summary>
        /// Gets all project reference file paths.
        /// </summary>
        public static IEnumerable<string> GetProjectReferenceFilePaths(this SolutionFile solutionFile, string solutionFilePath)
        {
            var projectReferenceFilePaths = solutionFile.SolutionFileProjectReferences.Select(projectReference => VsPathUtilities.GetProjectFilePath(solutionFilePath, projectReference.ProjectFileRelativePathValue));
            return projectReferenceFilePaths;
        }

        /// <summary>
        /// Gets all project reference file paths for project in the dependencies solution folder.
        /// </summary>
        public static IEnumerable<string> GetProjectReferenceDependencyFilePaths(this SolutionFile solutionFile, string solutionFilePath)
        {
            var dependencies = solutionFile.GetDependencyProjectReferences().Select(projectReference => VsPathUtilities.GetProjectFilePath(solutionFilePath, projectReference.ProjectFileRelativePathValue));
            return dependencies;
        }

        public static IEnumerable<Guid> GetDependencyProjectGUIDs(this SolutionFile solutionFile)
        {
            var hasDependenciesSolutionFolder = solutionFile.HasDependenciesSolutionFolder(out var dependenciesSolutionFolder);
            var hasNestedProjectsGlobalSection = solutionFile.GlobalSections.HasNestedProjectsGlobalSection(out var nestedProjectsGlobalSection);

            if(!hasDependenciesSolutionFolder || !hasNestedProjectsGlobalSection)
            {
                return Enumerable.Empty<Guid>();
            }

            var dependencyProjectGUIDs = nestedProjectsGlobalSection.ProjectNestings.Where(x => x.ParentProjectGUID == dependenciesSolutionFolder.ProjectGUID).Select(x => x.ProjectGUID);
            return dependencyProjectGUIDs;
        }

        public static IEnumerable<SolutionFileProjectReference> GetDependencyProjectReferences(this SolutionFile solutionFile)
        {
            var dependencyProjectGUIDs = solutionFile.GetDependencyProjectGUIDs();

            var dependencyProjectReferences = solutionFile.SolutionFileProjectReferences.Where(x => dependencyProjectGUIDs.Contains(x.ProjectGUID));
            return dependencyProjectReferences;
        }

        public static void RemoveProjectReference(this SolutionFile solutionFile, string solutionFilePath, string projectFilePath)
        {
            var projectRelativePath = VsPathUtilities.GetProjectFileRelativeToSolutionDirectoryPath(solutionFilePath, projectFilePath);

            var hasProjectReference = solutionFile.HasProjectReference(projectRelativePath, out var projectReference);
            if(!hasProjectReference)
            {
                return;
            }

            // Is the project reference in a nested solution folder?
            var hasNestedProjectsGlobalSection = solutionFile.GlobalSections.HasNestedProjectsGlobalSection(out var nestedProjectsGlobalSection);
            if(hasNestedProjectsGlobalSection)
            {
                nestedProjectsGlobalSection.ProjectNestings.RemoveAll(x => x.ProjectGUID == projectReference.ProjectGUID);
            }

            // Remove the project configuration platform entries.
            var hasProjectConfigurationPlatformsGlobalSection = solutionFile.GlobalSections.HasProjectConfigurationPlatformsGlobalSection(out var projectConfigurationPlatformsGlobalSection);
            if(hasProjectConfigurationPlatformsGlobalSection)
            {
                projectConfigurationPlatformsGlobalSection.ProjectBuildConfigurationMappings.RemoveAll(x => x.ProjectGUID == projectReference.ProjectGUID);
            }

            // Remove the project reference.
            solutionFile.SolutionFileProjectReferences.Remove(projectReference);
        }
    }
}
