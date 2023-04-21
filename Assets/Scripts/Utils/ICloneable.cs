namespace App.Utils
{
    public interface ICloneable<T> where T : class
    {
        public T Clone();
    }
}