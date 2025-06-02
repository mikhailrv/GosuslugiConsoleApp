using GosuslugiWinForms.Models;
using GosuslugiWinForms.Repositories;

namespace GosuslugiWinForms.Services
{
    public class UserService
    {
        private readonly UserRepository _userRepository;
        private readonly ApplicationRepository _applicationRepository;
        private readonly ParameterRepository _parameterRepository;
        private readonly ServiceRepository _serviceRepository;
        private readonly ParameterTypeRepository _parameterTypeRepository;

        public UserService(
            UserRepository userRepository,
            ApplicationRepository applicationRepository,
            ParameterRepository parameterRepository,
            ServiceRepository serviceRepository,
            ParameterTypeRepository parameterTypeRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _applicationRepository = applicationRepository ?? throw new ArgumentNullException(nameof(applicationRepository));
            _parameterRepository = parameterRepository ?? throw new ArgumentNullException(nameof(parameterRepository));
            _serviceRepository = serviceRepository ?? throw new ArgumentNullException(nameof(serviceRepository));
            _parameterTypeRepository = parameterTypeRepository ?? throw new ArgumentNullException(nameof(parameterTypeRepository));
        }

        public void EditParameter(Guid userId, Guid parameterId, string newValue)
        {
            var user = _userRepository.FindById(userId) as User
                ?? throw new ArgumentException("Пользователь не найден или не является гражданином.");
            var parameter = user.Parameters.FirstOrDefault(p => p.Id == parameterId)
                ?? throw new ArgumentException("Параметр не найден.");
            if (parameter.UserId != userId)
                throw new ArgumentException("Параметр не принадлежит этому пользователю.");
            parameter.Value = newValue ?? throw new ArgumentNullException(nameof(newValue));
            _parameterRepository.Update(parameter);
        }

        public void AddParameter(Parameter parameter)
        {
            var user = _userRepository.FindById(parameter.UserId) as User
                ?? throw new ArgumentException("Пользователь не найден или не является гражданином.");
            var parameterType = _parameterTypeRepository.FindById(parameter.ParameterTypeId)
                ?? throw new ArgumentException("Тип параметра не найден.");
            parameter.Id = Guid.NewGuid();
            _parameterRepository.Save(parameter);
        }

        public void SubmitApplication(Guid serviceId, Guid userId)
        {
            var user = _userRepository.FindById(userId) as User
                ?? throw new ArgumentException("Пользователь не найден или не является гражданином.");
            if (!user.IsActive)
                throw new InvalidOperationException("Учетная запись не активирована.");

            var service = _serviceRepository.FindById(serviceId)
                ?? throw new ArgumentException("Услуга не найдена.");

            // Проверка правил
            bool canApply = false;
            foreach (var rule in service.Rules ?? Enumerable.Empty<Rule>())
            {
                var parameter = user.Parameters.FirstOrDefault(p => p.ParameterTypeId == rule.ParameterTypeId);
                if (parameter != null && EvaluateRule(parameter.Value, rule))
                {
                    canApply = true;
                    break;
                }
            }

            if (!canApply)
                throw new InvalidOperationException("Вы не можете получить эту услугу.");

            var application = new Models.Application
            {
                ServiceId = serviceId,
                UserId = userId,
                SubmissionDate = DateTimeOffset.UtcNow.UtcDateTime,
                Status = ApplicationStatus.PENDING,
                Deadline = CalculateExecutionDeadline(service, user)
            };
            _applicationRepository.Save(application);
        }

        public void CancelApplication(Guid applicationId)
        {
            var application = _applicationRepository.FindById(applicationId)
                ?? throw new ArgumentException("Заявка не найдена.");
            if (application.Status != ApplicationStatus.PENDING)
                throw new InvalidOperationException("Можно отменить только заявки в статусе 'Ожидание'.");
            application.Status = ApplicationStatus.CANCELLED;
            _applicationRepository.Update(application);
        }

        private DateTime CalculateExecutionDeadline(Service service, User user)
        {
            var baseDeadline = DateTimeOffset.UtcNow.AddDays(1); 
            foreach (var rule in service.Rules ?? Enumerable.Empty<Rule>())
            {
                var parameter = user.Parameters.FirstOrDefault(p => p.ParameterTypeId == rule.ParameterTypeId);
                if (parameter != null && EvaluateRule(parameter.Value, rule))
                {
                    baseDeadline = baseDeadline.Add(rule.DeadlineInterval);
                }
            }
            return baseDeadline.UtcDateTime; 
        }

        private bool EvaluateRule(string? parameterValue, Rule rule)
        {
            if (string.IsNullOrEmpty(parameterValue) || string.IsNullOrEmpty(rule.Value))
                return false;

            switch (rule.Operator)
            {
                case Operator.EQUAL:
                    return parameterValue == rule.Value;
                case Operator.NOT_EQUALS:
                    return parameterValue != rule.Value;
                case Operator.GREATER_THAN:
                    return int.TryParse(parameterValue, out var paramInt) && int.TryParse(rule.Value, out var ruleInt) && paramInt > ruleInt;
                case Operator.LESS_THAN:
                    return int.TryParse(parameterValue, out paramInt) && int.TryParse(rule.Value, out ruleInt) && paramInt < ruleInt;
                case Operator.BETWEEN:
                    var values = rule.Value.Split(',');
                    if (values.Length == 2 && int.TryParse(parameterValue, out paramInt) &&
                        int.TryParse(values[0], out var min) && int.TryParse(values[1], out var max))
                    {
                        return paramInt >= min && paramInt <= max;
                    }
                    return false;
                default:
                    return false;
            }
        }

        public List<Service> GetAllActiveServices()
        {
            return _serviceRepository.FindAllActive();
        }

        public List<Parameter> GetUserParameters(Guid userId)
        {
            return _userRepository.FindParameters(userId);
        }

        public List<ParameterType> GetParameterTypes()
        {
            return _parameterTypeRepository.FindAll();
        }

        public List<User> GetPendingUsers()
        {
            return _userRepository.FindPendingUsers();
        }

        public User GetUser(Guid userId)
        {
            var user = _userRepository.FindById(userId) as User
                ?? throw new InvalidOperationException("Пользователь не найден.");
            return user;
        }

        public void UpdateUser(Guid userId, string fullName)
        {
            var user = _userRepository.FindById(userId) as User
                ?? throw new InvalidOperationException("Пользователь не найден.");
            user.FullName = fullName;
            _userRepository.Update(user);
        }

        public List<Models.Application> GetUserApplications(Guid userId)
        {
            return _applicationRepository.GetByUser(userId);
        }
    }
}