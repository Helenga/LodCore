﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NotificationService;
using Ploeh.AutoFixture;
using ProjectManagement.Application;
using ProjectManagement.Domain;
using ProjectManagement.Domain.Events;
using ProjectManagement.Infrastructure;

namespace ProjectManagementTests
{
    [TestClass]
    public class ProjectProviderTests
    {
        private Mock<IEventSink> _eventSinkMock;
        private Fixture _fixture;
        private Mock<IProjectManagerGateway> _pmGateway;
        private ProjectProvider _projectProvider;
        private Mock<IProjectRepository> _projectRepository;
        private Mock<IUserRepository> _userRepository;
        private Mock<IVersionControlSystemGateway> _vcsGateway;

        [TestInitialize]
        public void Setup()
        {
            _fixture = new Fixture();
            _pmGateway = new Mock<IProjectManagerGateway>();
            _projectRepository = new Mock<IProjectRepository>();
            _vcsGateway = new Mock<IVersionControlSystemGateway>();
            _eventSinkMock = new Mock<IEventSink>();
            _userRepository = new Mock<IUserRepository>();
            var paginationSettings = new PaginationSettings(10);

            _pmGateway
                .Setup(pm => pm.CreateProject(It.IsAny<CreateProjectRequest>()))
                .Returns(_fixture.Create<int>());
            _vcsGateway
                .Setup(vcs => vcs.CreateRepositoryForProject(It.IsAny<CreateProjectRequest>()))
                .Returns(_fixture.Create<int>());
            _projectRepository
                .Setup(repo => repo.SaveProject(It.IsAny<Project>()))
                .Returns(1);
            _userRepository.Setup(repo => repo.GetUserRedmineId(It.IsAny<int>())).Returns(1);
            _userRepository.Setup(repo => repo.GetUserGitlabId(It.IsAny<int>())).Returns(1);
            _projectProvider = new ProjectProvider(
                _pmGateway.Object,
                _vcsGateway.Object,
                _projectRepository.Object,
                _eventSinkMock.Object,
                _userRepository.Object,
                paginationSettings);
        }

        [TestMethod]
        public void ProjectHasToBeAddedSuccessfully()
        {
            var createRequest = _fixture.Create<CreateProjectRequest>();

            _projectProvider.CreateProject(createRequest);

            _projectRepository.Verify(
                repo => repo.SaveProject(It.Is<Project>(
                    project => project.Name == createRequest.Name
                               || project.Info == createRequest.Info
                               || project.AccessLevel == createRequest.AccessLevel
                               || project.LandingImageUri == createRequest.LandingImageUri)),
                Times.Once);
            _vcsGateway.Verify(
                vsc => vsc.CreateRepositoryForProject(It.Is<CreateProjectRequest>(
                    request => request.Equals(createRequest))),
                Times.Once);
            _pmGateway.Verify(pm => pm.CreateProject(It.Is<CreateProjectRequest>(
                request => request.Equals(createRequest))),
                Times.Once);
            _eventSinkMock.Verify(
                sink => sink.ConsumeEvent(
                    It.Is<IEventInfo>(
                        eventInfo => eventInfo.GetEventType() == typeof (NewProjectCreated).Name)),
                Times.Once);
        }
    }
}