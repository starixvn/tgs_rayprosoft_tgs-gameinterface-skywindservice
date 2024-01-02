namespace Tgs.GameInterface.SkywindService.Core.Exceptions;

public class DuplicateTransactionException(string extTrxId) : Exception($"This transaction is repeated.({extTrxId})")
{
}
