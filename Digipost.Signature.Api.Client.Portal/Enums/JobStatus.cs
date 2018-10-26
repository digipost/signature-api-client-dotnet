namespace Digipost.Signature.Api.Client.Portal.Enums
{
    public enum JobStatus
    {
        /// <summary>
        ///     There has not been any changes since the last received status change.
        /// </summary>
        NoChanges,

        /// <summary>
        ///     Indicates that the signature job failed. For details about the failure, see the <see cref="SignatureStatus" /> of
        ///     each signer. When the client confirms a job with this status, the job and its associated resources will become
        ///     unavailable through the Signature API.
        /// </summary>
        Failed,

        /// <summary>
        ///     Indicates that there has been a change to the job, but that it has not been signed by all signers yet. For details
        ///     about the state, see the <see cref="SignatureStatus" /> of each signer. When the client confirms a job with this
        ///     status, the job is removed from the queue and will not be returned upon subsequent polling, until the status has
        ///     changed again.
        /// </summary>
        InProgress,

        /// <summary>
        ///     When the client(s) confirms a job with this status, the job and its associated resources will become unavailable
        ///     through the Signature API.
        /// </summary>
        CompletedSuccessfully
    }
}
