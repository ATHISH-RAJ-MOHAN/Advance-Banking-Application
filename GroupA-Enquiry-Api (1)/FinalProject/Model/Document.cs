namespace FinalProject.Model
{
    public class Emailing
    {
        private string _email = "";

        public Emailing(string email)
        {
            _email = email;

        }

        public string? Email
        {
            get { return _email; }
            set { _email = value; }
        }
    }
    public class Document
    {
        private string _email = "";
        private IFormFile? _formPhoto;
        private IFormFile? _formAadhar;
        private IFormFile? _formPanCard;
        private string _basePhoto = null;
        private string _baseAadhar = null;
        private string _basePanCard = null;


        public Document()
        {
        }

        public Document(string email, IFormFile? formPhoto, IFormFile? formAadhar,
                       IFormFile? formPanCard, string basePhoto, string baseAadhar, string basePanCard)
        {
            _email = email;
            _formPhoto = formPhoto;
            _formAadhar = formAadhar;
            _formPanCard = formPanCard;
            _basePhoto = basePhoto;
            _baseAadhar = baseAadhar;
            _basePanCard = basePanCard;
        }

        public string? Email
        {
            get { return _email; }
            set { _email = value; }
        }
        public IFormFile? Photo
        {
            get { return _formPhoto; }
            set { _formPhoto = value; }
        }
        public IFormFile? Aadhar
        {
            get { return _formAadhar; }
            set { _formAadhar = value; }
        }
        public IFormFile? PanCard
        {
            get { return _formPanCard; }
            set { _formPanCard = value; }
        }
        public string? basePhoto
        {
            get { return _basePhoto; }
            set { _basePhoto = value; }
        }
        public string? baseAadhar
        {
            get { return _baseAadhar; }
            set { _baseAadhar = value; }
        }
        public string? basePanCard
        {
            get { return _basePanCard; }
            set { _basePanCard = value; }
        }



    }


}

