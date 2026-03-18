namespace HireMind.Application.Queries.JobApplication
{
    public record GetCvByApplicationIdQuery(int ApplicationId) : IRequest<GetCvByApplicationIdResult>;

    public record GetCvByApplicationIdResult(string CvFilePath);

    internal class GetCvByApplicationIdHandler : IRequestHandler<GetCvByApplicationIdQuery, GetCvByApplicationIdResult>
    {
        private readonly IJobApplicationRepository _jobApplicationRepository;

        public GetCvByApplicationIdHandler(IJobApplicationRepository jobApplicationRepository)
        {
            _jobApplicationRepository = jobApplicationRepository;
        }

        public async Task<GetCvByApplicationIdResult> Handle(GetCvByApplicationIdQuery request, CancellationToken cancellationToken)
        {
            var application = await _jobApplicationRepository.GetJobApplicationByIdWithAnalyzedCVAsync(request.ApplicationId, cancellationToken);

            if (application == null || string.IsNullOrEmpty(application.AnalyzeCv.CvFilePath))
                return new GetCvByApplicationIdResult(string.Empty);

             return new GetCvByApplicationIdResult(application.AnalyzeCv.CvFilePath);
        }
    }
}
