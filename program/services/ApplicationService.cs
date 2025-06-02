using GosuslugiWinForms.Models;
using GosuslugiWinForms.Repositories;

namespace GosuslugiWinForms.Services
{
    public class ApplicationService
    {
        private readonly ApplicationRepository _applicationRepository;

        public ApplicationService(ApplicationRepository applicationRepository)
        {
            _applicationRepository = applicationRepository ?? throw new ArgumentNullException(nameof(applicationRepository));
        }

        public ApplicationStatus? GetStatus(Guid applicationId)
        {
            var application = _applicationRepository.FindById(applicationId);
            return application?.Status;
        }

        public Models.Application? GetById(Guid applicationId)
        {
            return _applicationRepository.FindById(applicationId);
        }

        public List<Models.Application> GetByUser(Guid userId)
        {
            return _applicationRepository.GetByUser(userId);
        }
    }
}