namespace GroupA_Manager_Api.Models
{
    public interface IManagerRepository<TEntity>
    {
        TEntity ApproveEnquiry(int endId);
        TEntity GetCredentials(int EnquiryId);
        string RejectEnquiry(int EnqId, string feedback);
        EnquiryModel DisplayEnquiry(int EnquiryId);
        List<RequestModel> PendingEnquiries(int ManagerId);
        List<RequestModel> ApprovedEnquiries(int ManagerId);
        List<RequestModel> RejectedEnquiries(int ManagerId);
        int? GetManagerIdFromUserId(int userId);
    }
}
