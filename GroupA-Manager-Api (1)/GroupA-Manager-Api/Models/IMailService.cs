namespace GroupA_Manager_Api.Models
{
    public interface IMailService
    {
        Task TestEmail();
        Task SendEmailAsync(int EnquiryId);
        Task SendRejectedMaiAsync(int EnquiryId, string feedback);
    }
}
