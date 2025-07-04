using FinalProject.DataAccess;
using FinalProject.Model;

namespace FinalProject.Process
{
    public class EnquiryLoginProcess
    {

        private readonly EnquiryLoginDataAccess _elda;
        public EnquiryLoginProcess(EnquiryLoginDataAccess elda)
        {

            _elda = elda;
        }


        public string CreateEnquirer(string email, string password)
        {
            try
            {
                return _elda.CreateEnquirer(email, password);
            }
            catch
            {
                throw;
            }

        }

        public CreateEnquiry GetEnquirer(string email, string password)
        {
            try
            {
                return _elda.GetEnquirer(email, password);
            }
            catch
            {
                throw;
            }

        }

        public Document GetDocuments(string email)
        {
            try
            {
                return _elda.GetDocuments(email);
            }
            catch
            {
                throw;
            }

        }

        public void CreateEnquiry(string firstName, string lastName, string address1, string address2, string address3, string phoneNumber,
                         string email, DateTime dob, string city, string country, int status, int pincode, bool wantsCheque,
                         string feedback, bool isActive, int accountType, decimal balance)
        {
            try
            {


                _elda.CreateEnquiry(firstName, lastName, address1, address2, address3, phoneNumber, email, dob, city, country,
                                         status, pincode, wantsCheque, feedback, isActive, accountType, balance);


            }
            catch
            {
                throw;
            }
        }

        public void CreateDocuments(string email, byte[] formPhoto, byte[] formAadhar, byte[] formPanCard)
        {
            try
            {


                _elda.CreateDocuments(email, formPhoto, formAadhar, formPanCard);


            }
            catch
            {
                throw;
            }
        }

        public void SaveEnquiry(string firstName, string lastName, string address1, string address2, string address3, string phoneNumber,
                      string email, DateTime dob, string city, string country, int status, int pincode, bool wantsCheque,
                      string feedback, bool isActive, int accountType, decimal balance)
        {
            try
            {


                _elda.SaveEnquiry(firstName, lastName, address1, address2, address3, phoneNumber, email, dob, city, country,
                                         status, pincode, wantsCheque, feedback, isActive, accountType, balance);


            }
            catch
            {

                throw;
            }
        }


    }
}
