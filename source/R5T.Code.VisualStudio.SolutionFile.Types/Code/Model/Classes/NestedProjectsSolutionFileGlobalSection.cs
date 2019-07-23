using System;
using System.Collections.Generic;


namespace R5T.Code.VisualStudio.Model
{
    public class NestedProjectsSolutionFileGlobalSection : SolutionFileGlobalSectionBase
    {
        public const string SolutionFileGlobalSectionName = "NestedProjects";


        #region Static

        /// <summary>
        /// Creates a new <see cref="NestedProjectsSolutionFileGlobalSection"/> with the <see cref="NestedProjectsSolutionFileGlobalSection.SolutionFileGlobalSectionName"/> and <see cref="PreOrPostSolution.PreSolution"/>.
        /// </summary>
        public static NestedProjectsSolutionFileGlobalSection New()
        {
            var output = new NestedProjectsSolutionFileGlobalSection
            {
                Name = NestedProjectsSolutionFileGlobalSection.SolutionFileGlobalSectionName,
                PreOrPostSolution = PreOrPostSolution.PreSolution,
            };
            return output;
        }

        #endregion



        public List<ProjectNesting> ProjectNestings { get; } = new List<ProjectNesting>();

        public override IEnumerable<string> ContentLines
        {
            get
            {
                foreach (var projectNesting in this.ProjectNestings)
                {
                    var line = ProjectNesting.Serialize(projectNesting);
                    yield return line;
                }
            }
        }
    }
}
