using GosuslugiWinForms.Models;
using GosuslugiWinForms.Repositories;

namespace GosuslugiWinForms.Services
{
    public class AdminService
    {
        private readonly ServiceRepository _serviceRepository;
        private readonly RuleRepository _ruleRepository;
        private readonly UserRepository _userRepository;
        private readonly ParameterTypeRepository _parameterTypeRepository;

        public AdminService(ServiceRepository serviceRepository, RuleRepository ruleRepository, UserRepository userRepository, ParameterTypeRepository parameterTypeRepository)
        {
            _serviceRepository = serviceRepository ?? throw new ArgumentNullException(nameof(serviceRepository));
            _ruleRepository = ruleRepository ?? throw new ArgumentNullException(nameof(ruleRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _parameterTypeRepository = parameterTypeRepository ?? throw new ArgumentNullException(nameof(parameterTypeRepository));
        }

        public void CreateService(Service service)
        {
            if (string.IsNullOrEmpty(service.Title))
                throw new ArgumentException("Название услуги не может быть пустым.");
            _serviceRepository.Save(service);
        }

        public void UpdateService(Guid serviceId, Service newServiceData)
        {
            var service = _serviceRepository.FindById(serviceId)
                ?? throw new ArgumentException("Услуга не найдена.");
            service.EndDate = newServiceData.EndDate;
            _serviceRepository.Update(service);
        }

        public void CreateRule(Rule rule)
        {
            if (rule == null || _serviceRepository.FindById(rule.ServiceId) == null)
                throw new ArgumentException("Услуга не найдена или правило не задано.");
            _ruleRepository.Save(rule);
        }

        public void UpdateRule(Guid ruleId, Rule newRuleData)
        {
            var rule = _ruleRepository.FindById(ruleId)
                ?? throw new ArgumentException("Правило не найдено.");
            rule.ServiceId = newRuleData.ServiceId != Guid.Empty ? newRuleData.ServiceId : rule.ServiceId;
            rule.ParameterTypeId = newRuleData.ParameterTypeId != Guid.Empty ? newRuleData.ParameterTypeId : rule.ParameterTypeId;
            rule.Description = newRuleData.Description ?? rule.Description;
            rule.Value = newRuleData.Value ?? rule.Value;
            rule.Operator = newRuleData.Operator;
            rule.DeadlineInterval = newRuleData.DeadlineInterval;
            _ruleRepository.Update(rule);
        }

        public void CreateStaffAccount(Account account)
        {
            if (account.Role != Role.ADMIN && account.Role != Role.CIVIL_SERVANT)
                throw new ArgumentException("Можно создать только учетные записи администратора или госслужащих.");
            _userRepository.Save(new User
            {
                Login = account.Login,
                Password = account.Password,
                FullName = account.FullName,
                Role = account.Role,
                IsActive = true
            });
        }

        public void CreateParameterType(ParameterType parameterType)
        {
            if (string.IsNullOrEmpty(parameterType.Name))
                throw new ArgumentException("Название типа параметра не может быть пустым.");
            _parameterTypeRepository.Save(parameterType);
        }

        public void DeleteRule(Guid ruleId)
        {
            _ruleRepository.Delete(ruleId);
        }

        public Rule GetRule(Guid ruleId)
        {
            return _ruleRepository.FindById(ruleId)
                ?? throw new ArgumentException("Правило не найдено.");
        }
        public Service GetService(Guid serviceId)
        {
            return _serviceRepository.FindById(serviceId)
                ?? throw new ArgumentException("Услуга не найдена.");
        }

        public void DeleteService(Guid serviceId)
        {
            _serviceRepository.Delete(serviceId);
        }
        public void UpdateService(Service service)
        {
            _serviceRepository.Update(service);
        }

        public void UpdateRule(Rule rule)
        {
            _ruleRepository.Update(rule);
        }

        public void DeleteParameterType(Guid paramTypeId)
        {
            _parameterTypeRepository.Delete(paramTypeId);
        }

        public List<Service> GetAllServices()
        {
            return _serviceRepository.FindAll();
        }

        public List<Rule> GetAllRules()
        {
            return _ruleRepository.FindAll();
        }

        public List<ParameterType> GetParameterTypes()
        {
            return _parameterTypeRepository.FindAll();
        }

        public List<Account> GetAllAccounts()
        {
            return _userRepository.FindAllAccounts();
        }
    }
}