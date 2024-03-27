namespace Guid_Inator_9000;

internal class MaxNumberOfGuidsGenerator
{
    private long MaxNumberOfGuids = (long)Math.Pow(2, 128);

    public void Start()
    {
        Parallel.For(0, MaxNumberOfGuids, i =>
        {
            var guid = Guid.NewGuid();
            Console.WriteLine(guid);
        });
    }
}
