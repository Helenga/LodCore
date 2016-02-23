﻿using System;
using System.Collections.Generic;
using Common;
using Journalist;
using ProjectManagement.Domain;

namespace FrontendServices.Models
{
    public class Project
    {
        public Project(
            int projectId, 
            string name, 
            ProjectType[] projectType, 
            string info, 
            ProjectStatus projectStatus, 
            Uri landingImageUri, 
            Uri versionControlSystemUri,
            Uri projectManagementSystemUri, 
            HashSet<Issue> issues, 
            HashSet<ProjectMembership> projectMemberships, 
            HashSet<Uri> screenshots)
        {
            Require.Positive(projectId, nameof(projectId));
            Require.NotEmpty(name, nameof(name));
            Require.NotNull(info, nameof(info));
            Require.NotNull(versionControlSystemUri, nameof(versionControlSystemUri));
            Require.NotNull(projectManagementSystemUri, nameof(projectManagementSystemUri));

            ProjectId = projectId;
            Name = name;
            ProjectType = projectType;
            Info = info;
            ProjectStatus = projectStatus;
            LandingImageUri = landingImageUri;
            VersionControlSystemUri = versionControlSystemUri;
            ProjectManagementSystemUri = projectManagementSystemUri;
            Issues = issues;
            ProjectMemberships = projectMemberships;
            Screenshots = screenshots;
        }

        public int ProjectId { get; private set; }

        public string Name { get; private set; }

        public ProjectType[] ProjectType { get; private set; }

        public string Info { get; private set; }

        public ProjectStatus ProjectStatus { get; private set; }

        public Uri LandingImageUri { get; private set; }

        public Uri VersionControlSystemUri { get; private set; }

        public Uri ProjectManagementSystemUri { get; private set; }

        public HashSet<Issue> Issues { get; private set; }

        public HashSet<ProjectMembership> ProjectMemberships { get; private set; }

        public HashSet<Uri> Screenshots { get; private set; }
    }
}