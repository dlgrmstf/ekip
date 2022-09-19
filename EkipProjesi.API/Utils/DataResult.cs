namespace EkipProjesi.API.Utils
{
    public class DataResult<T> : Result
    {
        public T Data { get; set; }
    }
}