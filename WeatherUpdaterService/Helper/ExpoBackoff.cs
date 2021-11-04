using System;
using System.Threading.Tasks;

public struct ExponentialBackoff
{
    private readonly int _delayMilliseconds;
    private readonly int _maxDelayMilliseconds;
    private int _retries;
    private int _pow;

    public ExponentialBackoff(int delayMilliseconds, int maxDelayMilliseconds)
    {
        _delayMilliseconds = delayMilliseconds;
        _maxDelayMilliseconds = maxDelayMilliseconds;
        _retries = 0;
        _pow = 1;
    }

    public Task Delay()
    {
        ++_retries;
        if (_retries < 31)
        {
            _pow = _pow << 1;
        }

        var delay = Math.Min(_delayMilliseconds * (_pow - 1) / 2, _maxDelayMilliseconds);
        return Task.Delay(delay);
    }
}