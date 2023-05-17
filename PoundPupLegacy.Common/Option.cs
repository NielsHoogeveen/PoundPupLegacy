namespace PoundPupLegacy.Common;

public abstract record Option<T>
{
    private Option() { }

    public abstract T2 Match<T2>(Func<Some, T2> some, Func<None, T2> none);
    public sealed record Some: Option<T>
    {
        public T Value { get; init; }
        public Some(T value)
        {
            Value = value; 
        }
        public override T2 Match<T2>(Func<Some, T2> some, Func<None, T2> none) { return some(this); }
    }
    public sealed record None : Option<T> 
    {
        public override T2 Match<T2>(Func<Some, T2> some, Func<None, T2> none) { return none(this); }
    }
}
