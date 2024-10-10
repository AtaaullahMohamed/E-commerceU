namespace Api.RequestHelpers
{
    public class Pagination<T>(int pagIndex,int pageSize,int count,IReadOnlyList<T>data)
    {

        public int PageIndex { get; set; } = pagIndex;
        public int PageSize { get; set; }=pageSize;
        public int Count { get; set; }=count;
        public IReadOnlyList<T> Data { get; set; }=data;
    }
}
