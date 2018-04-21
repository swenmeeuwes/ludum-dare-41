using System;

public abstract class Singleton<T> where T : new()
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
                _instance = new T();
            else
                throw new Exception(string.Format("{0} has already been instantiated. Please make sure you only have one in the scene!", typeof(T)));

            return _instance;
        }
    }
}
