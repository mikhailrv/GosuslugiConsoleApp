using GosuslugiWinForms.Models;
using GosuslugiWinForms.Repositories;

namespace GosuslugiWinForms.Services
{
    public class CivilServantService
    {
        private readonly ApplicationRepository _applicationRepository;
        private readonly UserRepository _userRepository;

        public CivilServantService(ApplicationRepository applicationRepository, UserRepository userRepository)
        {
            _applicationRepository = applicationRepository ?? throw new ArgumentNullException(nameof(applicationRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public void ProcessRegistrationRequest(Guid userId, bool approved)
        {
            var user = _userRepository.FindById(userId) as User
                ?? throw new ArgumentException("Пользователь не найден или не является гражданином.");
            user.IsActive = approved;
            _userRepository.Update(user);
        }

        public void UpdateStatus(Guid applicationId, ApplicationStatus status)
        {
            var application = _applicationRepository.FindById(applicationId)
                ?? throw new ArgumentException("Заявка не найдена.");
            if (!Enum.IsDefined(typeof(ApplicationStatus), status))
                throw new ArgumentException("Недопустимый статус.");
            application.Status = status;
            if (status == ApplicationStatus.COMPLETED || status == ApplicationStatus.REJECTED)
                application.CompletionDate = DateTime.Now;
            _applicationRepository.Update(application);
        }

        public void AddResult(Guid applicationId, string result)
        {
            var application = _applicationRepository.FindById(applicationId)
                ?? throw new ArgumentException("Заявка не найдена.");
            application.Result = result ?? throw new ArgumentNullException(nameof(result));
            _applicationRepository.Update(application);
        }

        public List<Models.Application> GetAllApplications()
        {
            return _applicationRepository.FindAll();
        }

        public void UpdateApplication(Guid id, ApplicationStatus status, string result)
        {
            var application = _applicationRepository.FindById(id)
                ?? throw new ArgumentException("Заявка не найдена.");
            application.Status = status;
            application.Result = result;
            _applicationRepository.Update(application);
        }

        public List<User> GetPendingRegistrations()
        {
            return _userRepository.FindPendingUsers();
        }
    }
}