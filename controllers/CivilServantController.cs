 using System;
using GosuslugiWinForms.Models;
using GosuslugiWinForms.Services;

namespace GosuslugiWinForms.Controllers
{
    public class CivilServantController
    {
        private readonly CivilServantService _civilServantService;

        public CivilServantController(CivilServantService civilServantService)
        {
            _civilServantService = civilServantService ?? throw new ArgumentNullException(nameof(civilServantService));
        }

        public void ProcessRegistrationRequest(Guid userId, bool approved)
        {
            _civilServantService.ProcessRegistrationRequest(userId, approved);
        }

        public void UpdateStatus(Guid applicationId, ApplicationStatus status)
        {
            _civilServantService.UpdateStatus(applicationId, status);
        }

        public void AddResult(Guid applicationId, string result)
        {
            _civilServantService.AddResult(applicationId, result);
        }
        public List<Models.Application> GetAllApplications()
        {
            return _civilServantService.GetAllApplications();
        }

        public List<User> GetPendingRegistrations()
        {
            return _civilServantService.GetPendingRegistrations();
        }

        public void UpdateApplication(Guid id, ApplicationStatus status, string result)
        {
            _civilServantService.UpdateApplication(id, status, result);
        }

    }
}