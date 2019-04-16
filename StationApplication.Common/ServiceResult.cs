namespace StationApplication.Common
{
    public class ServiceResult
    {
        public bool Result { get; set; }
        public string Message { get; set; }
        public int Id { get; set; }
        public string UniqueCode { get; set; }
        public double Pay { get; set; }
        public double Refund { get; set; }
    }
}
