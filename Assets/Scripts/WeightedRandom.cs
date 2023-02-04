using System.Collections.Generic;

public class WeightedRandom<T>
{
    private struct Item
    {
        public T Value;

        public float Probability;

        public Item(T value, float probability)
        {
            this.Value = value;
            this.Probability = probability;
        }
    }

    private readonly System.Random _random;

    private readonly List<Item> _weightedValues;

    public WeightedRandom(int seed) : this(new System.Random(seed)) { }

    public WeightedRandom() : this(new System.Random()) { }

    public WeightedRandom(System.Random random)
    {
        this._random = random;
        this._weightedValues = new List<Item>();
    }

    public WeightedRandom<T> Clear()
    {
        this._weightedValues.Clear();
        this.SumOfProbabilities = 0f;

        return this;
    }

    public WeightedRandom<T> Add(T value, float probability)
    {
        if (probability <= 0f) return this;

        this._weightedValues.Add(new Item(value, probability));
        this.SumOfProbabilities += probability;

        return this;
    }

    public float SumOfProbabilities { get; private set; } = 0f;

    public bool HasNext => this.SumOfProbabilities > 0f;

    public T Next()
    {
        double p = this._random.NextDouble() * this.SumOfProbabilities;

        foreach (var v in this._weightedValues)
        {
            p -= v.Probability;
            if (p <= 0) return v.Value;
        }

        throw new System.Exception("No next item");
    }

}
