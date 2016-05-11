namespace Digipost.Signature.Api.Client.Portal.Enums
{
    public enum JobStatus
    {
        /// <summary>
        /// There has not been any changes since the last received status change.
        /// </summary>
        NoChanges,

        Failed,

        InProgress,

        /// <summary>
        /// When the client(s) confirms a job with this status, the job and its associated resources will become unavailable through the Signature API.
        /// </summary>
        CompletedSuccessfully
    }
}