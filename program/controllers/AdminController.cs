using System;
using GosuslugiWinForms.Models;
using GosuslugiWinForms.Services;

namespace GosuslugiWinForms.Controllers
{
    public class AdminController
    {
        private readonly AdminService _adminService;

        public AdminController(AdminService adminService)
        {
            _adminService = adminService ?? throw new ArgumentNullException(nameof(adminService));
        }

        public void CreateService(Service service)
        {
            _adminService.CreateService(service);
        }

        public void UpdateService(Guid serviceId, Service newServiceData)
        {
            _adminService.UpdateService(serviceId, newServiceData);
        }

        public void CreateRule(Rule rule)
        {
            _adminService.CreateRule(rule);
        }

        public void EditRule(Guid ruleId, Rule newRuleData)
        {
            _adminService.UpdateRule(ruleId, newRuleData);
        }

        public void CreateStaffAccount(Account account)
        {
            _adminService.CreateStaffAccount(account);
        }

        public void CreateParameterType(ParameterType parameterType)
        {
            _adminService.CreateParameterType(parameterType);
        }

        public void DeleteRule(Guid ruleId)
        {
            _adminService.DeleteRule(ruleId);
        }

        public void DeleteService(Guid serviceId)
        {
            _adminService.DeleteService(serviceId);
        }

        public void DeleteParameterType(Guid paramTypeId)
        {
            _adminService.DeleteParameterType(paramTypeId);
        }

        public List<Service> GetAllServices()
        {
            return _adminService.GetAllServices();
        }
        public List<Account> GetAllAccounts() => _adminService.GetAllAccounts();

        public List<ParameterType> GetParameterTypes() => _adminService.GetParameterTypes();

        public void UpdateRule(Rule rule) => _adminService.UpdateRule(rule);

        public Rule GetRule(Guid ruleId) => _adminService.GetRule(ruleId);

        public List<Rule> GetAllRules() => _adminService.GetAllRules();

        public Service GetService(Guid serviceId) => _adminService.GetService(serviceId);

    }
}