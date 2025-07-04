namespace FinalProject.Controllers
{
    public class Utility
{
        public static byte[] ConvertToBytes(IFormFile file)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }
   
    }
}
