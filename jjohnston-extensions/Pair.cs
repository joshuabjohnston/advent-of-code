namespace org.jjohnston.extensions;

public class Pair<T>
{
    public T A { get; set; }
    public T B { get; set; }

    public Pair(T pA, T pB)
    {
        this.A = pA;
        this.B = pB;
    }
}