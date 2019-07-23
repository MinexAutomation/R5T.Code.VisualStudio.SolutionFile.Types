using System;


namespace R5T.Code.VisualStudio.Model
{
    public class ProjectBuildConfigurationMapping
    {
        public Guid ProjectGUID { get; set; }
        public SolutionBuildConfiguration SolutionBuildConfiguration { get; set; }
        public ProjectConfigurationIndicator ProjectConfigurationIndicator { get; set; }
        public SolutionBuildConfiguration MappedSolutionBuildConfiguration { get; set; }
    }
}
