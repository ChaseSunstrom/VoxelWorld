using System.Threading;

namespace Spark.Common;
public static class Cancellation
{
    private static readonly CancellationTokenSource _cts = new();

    public static CancellationToken Token => _cts.Token;
    
    public static CancellationTokenSource TokenSource => _cts;

    public static void Cancel() => _cts.Cancel();
}
