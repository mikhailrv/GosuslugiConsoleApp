using System;
using System.Collections.Generic;
using GosuslugiWinForms.Models;
using GosuslugiWinForms.Services;

namespace GosuslugiWinForms.Controllers
{
    public class UserController
    {
        private readonly UserService _userService;
        private readonly ApplicationService _applicationService;

        public UserController(UserService userService, ApplicationService applicationService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _applicationService = applicationService ?? throw new ArgumentNullException(nameof(applicationService));
        }

        public void EditParameter(Guid userId, Guid parameterId, string newValue)
        {
            _userService.EditParameter(userId, parameterId, newValue);
        }

        public void AddParameter(Parameter parameter)
        {
            _userService.AddParameter(parameter);
        }

        public void SubmitApplication(Guid serviceId, Guid userId)
        {
            _userService.SubmitApplication(serviceId, userId);
        }

        public void CancelApplication(Guid applicationId)
        {
            _userService.CancelApplication(applicationId);
        }

        public ApplicationStatus? GetStatus(Guid applicationId)
        {
            return _applicationService.GetStatus(applicationId);
        }

        public Models.Application? GetById(Guid applicationId)
        {
            return _applicationService.GetById(applicationId);
        }

        public List<Models.Application> GetApplicationsByUser(Guid userId)
        {
            return _applicationService.GetByUser(userId);
        }

        public List<Service> GetAllActiveServices()
        {
            return _userService.GetAllActiveServices();
        }

        public List<User> GetPendingUsers()
        {
            return _userService.GetPendingUsers();
        }

        public List<Parameter> GetUserParameters(Guid userId)
        {
            return _userService.GetUserParameters(userId);
        }

        public List<ParameterType> GetParameterTypes()
        {
            return _userService.GetParameterTypes();
        }

        public User GetUser(Guid userId)
        {
            return _userService.GetUser(userId);
        }

        public void UpdateUser(Guid userId, string fullName)
        {
            _userService.UpdateUser(userId, fullName);
        }
    }
}