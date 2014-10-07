namespace Skpic.Common
{
    public interface IGrouping<TKey>
    {
        TKey Key { get; set; }

        string Count { get; set; }

    }
}