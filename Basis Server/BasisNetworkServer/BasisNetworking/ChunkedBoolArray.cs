using System;

public class ChunkedBoolArray
{
    private readonly object[] _chunkLocks;  // Locks for each chunk
    private readonly bool[][] _chunks;      // Array divided into chunks
    private readonly int _chunkSize;        // Number of elements in each chunk
    private readonly int _numChunks;        // Number of chunks
    public const int TotalSize = 1024;      // Total size of the array

    // Precomputed array size
    private readonly int _totalSize;

    public ChunkedBoolArray(int chunkSize = 256)
    {
        if (TotalSize <= 0)
            throw new ArgumentOutOfRangeException(nameof(TotalSize), "Total size must be greater than zero.");
        if (chunkSize <= 0)
            throw new ArgumentOutOfRangeException(nameof(chunkSize), "Chunk size must be greater than zero.");

        _chunkSize = chunkSize;
        _numChunks = (int)Math.Ceiling((double)TotalSize / chunkSize);
        _totalSize = _chunkSize * _numChunks;

        _chunks = new bool[_numChunks][];
        _chunkLocks = new object[_numChunks];

        for (int i = 0; i < _numChunks; i++)
        {
            _chunks[i] = new bool[chunkSize];
            _chunkLocks[i] = new object();
        }
    }

    public void SetBool(int index, bool value)
    {
        if (index < 0 || index >= _totalSize)
            throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range.");

        int chunkIndex = index / _chunkSize;
        int localIndex = index % _chunkSize;

        lock (_chunkLocks[chunkIndex])
        {
            _chunks[chunkIndex][localIndex] = value;
        }
    }

    public bool GetBool(int index)
    {
        if (index < 0 || index >= _totalSize)
            throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range.");

        int chunkIndex = index / _chunkSize;
        int localIndex = index % _chunkSize;

        lock (_chunkLocks[chunkIndex])
        {
            return _chunks[chunkIndex][localIndex];
        }
    }
}
