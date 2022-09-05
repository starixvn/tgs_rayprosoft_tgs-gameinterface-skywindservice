namespace Tgs.GameInterface.SkywindService.Core.Exceptions;

public class DuplicateTransactionException : Exception
{
    public DuplicateTransactionException(string extTrxId) : base($"This transaction is repeated.({extTrxId})")
    {
    }
}
