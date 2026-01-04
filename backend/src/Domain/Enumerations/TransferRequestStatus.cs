using Domain.Common;

namespace Domain.Enumerations
{
    /// <summary>
    /// Enumeration representing the status of a transfer request
    /// </summary>
    public class TransferRequestStatus : Enumeration
    {
        public static readonly TransferRequestStatus Pending = new(1, "Pending");
        public static readonly TransferRequestStatus Accepted = new(2, "Accepted");
        public static readonly TransferRequestStatus Denied = new(3, "Denied");
        public static readonly TransferRequestStatus Cancelled = new(4, "Cancelled");

        private TransferRequestStatus(int id, string name) : base(id, name) { }

        public static TransferRequestStatus FromId(int id)
        {
            return FromValue<TransferRequestStatus>(id);
        }

        public static TransferRequestStatus FromName(string name)
        {
            return FromName<TransferRequestStatus>(name);
        }
    }
}
