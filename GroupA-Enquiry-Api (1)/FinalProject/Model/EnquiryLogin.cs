namespace FinalProject.Model
{
    public class EnquiryLogin
    {

        private string _email = "";
        private string _password = "";

        private EnquiryLogin() { }
        public EnquiryLogin(string email, string password)
        {

            _email = email;
            _password = password;
        }

        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }


    }
}
