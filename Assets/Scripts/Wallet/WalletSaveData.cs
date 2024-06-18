using System;

[Serializable]
public class WalletSaveData : ISaveable
{
    public string Id { get; set; }
    public int MoneyCount;
}