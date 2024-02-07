namespace Code.Utils.Pooling
{
    public interface IPoolObject
    {
        public void OnObjectInit();
        public SimpleObjectPool Pool { get; set; }
    }
}