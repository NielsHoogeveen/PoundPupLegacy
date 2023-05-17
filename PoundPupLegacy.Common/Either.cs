namespace PoundPupLegacy.Common;

public abstract record Either<TLeft, TRight>
{
    private Either() { }

    public abstract T Match<T>(Func<Left, T> left, Func<Right, T> right);

    public sealed record Left : Either<TLeft, TRight>
    {
        public TLeft Value { get; init; }
        public Left(TLeft value) 
        { 
            Value = value; 
        }
        public override T Match<T>(Func<Left, T> left, Func<Right, T> right) 
        { 
            return left(this); 
        }
    }
    public sealed record Right : Either<TLeft, TRight>
    {
        public TLeft Value { get; init; }
        public Right(TLeft value)
        {
            Value = value;
        }
        public override T Match<T>(Func<Left, T> left, Func<Right, T> right) 
        { 
            return right(this); 
        }
    }
}
