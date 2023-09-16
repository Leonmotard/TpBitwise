namespace TPBitwiseTraining.Models
{
    public class ResponseApi
    {
        public ResponseApi()
        {
            ErrorMenssages = new List<string>();

        }

        public System.Net.HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; }
        public List<string> ErrorMenssages { get; set; }
        public object Result { get; set; }

    }

}
