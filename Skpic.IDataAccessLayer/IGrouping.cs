namespace Skpic.IDataAccessLayer
{
    public interface IGrouping<TKey, TSource> : IQueryator<TSource> where TSource : class
    {
        TKey Key { get; set; }

        new int Count { get; set; }
    }
}