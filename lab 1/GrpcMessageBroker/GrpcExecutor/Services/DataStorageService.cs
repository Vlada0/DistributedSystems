using System.Collections.Concurrent;

namespace GrpcExecutor.Services
{
    public class DataStorageService
    {
        private readonly ConcurrentStack<int> _maxValStorage;
        private readonly ConcurrentStack<int> _minValStorage;

        public DataStorageService()
        {
            _maxValStorage = new ConcurrentStack<int>();
            _minValStorage = new ConcurrentStack<int>();

            _maxValStorage.Push(int.MaxValue);
            _minValStorage.Push(int.MinValue);
        }

        public int GetMaxLastValue()
        {
            _maxValStorage.TryPop(out var res);
            return res;
        }

        public void AddMax(int val) => _maxValStorage.Push(val);

        public int GetMinLastValue()
        {
            _minValStorage.TryPop(out var res);
            return res;
        }

        public void AddMin(int val) => _minValStorage.Push(val);
    }
}
