namespace M3C.Finance.BinanceSdk.Enumerations
{
    public class WalletType
    {
        private WalletType(string value) { Value = value; }

        private string Value { get; }

        public static WalletType Binance => new WalletType("Bianance");
        public static WalletType Bittrex => new WalletType("Bittrex");
        public static WalletType Wallet => new WalletType("Wallet");

        public static implicit operator string(WalletType type) => type.Value;
        public override string ToString() => Value;
    }
}
