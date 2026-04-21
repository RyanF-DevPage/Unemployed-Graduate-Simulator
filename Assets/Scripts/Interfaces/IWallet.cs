using System;

public interface IWallet
{
    float Balance { get; }

    void AddFunds(float amount);
    bool TrySpend(float amount);    // Returns false if insufficient funds

    event Action<float> OnBalanceChanged;
}
