namespace AccountsApiService.Models
{
    public class Beneficiaries
    {
        public int BenefID { get; set; }
        public long Source_Acc { get; set; }
        public long Dest_Acc { get; set; }
        public bool isActive { get; set; }
    }
}
