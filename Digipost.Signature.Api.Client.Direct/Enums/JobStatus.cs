namespace Digipost.Signature.Api.Client.Direct.Enums
{
    public enum JobStatus
    {
        /// <summary>
        /// The document(s) of the job has been signed by the receiver.
        /// </summary>
        Failed,

        /// <summary>
        /// The signature job has been rejected by the receiver.
        /// </summary>
        Rejected,

        /// <summary>
        /// An error occured during the signing ceremony.
        /// </summary>
        Signed
    }
}