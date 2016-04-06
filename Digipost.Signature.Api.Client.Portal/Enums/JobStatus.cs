namespace Digipost.Signature.Api.Client.Portal.Enums
{
    public enum JobStatus
    {
        /// <summary>
        /// There has not been any changes since the last received status change.
        /// </summary>
        NoChanges,

        /// <summary>
        /// When the client confirms a job with this status, the job is removed from the queue and will not be returned upon subsequent polling until the status has changed again.
        /// </summary>
        PartiallyCompleted,

        /// <summary>
        /// When the client confirms a job with this status, the job and its associated resources will become unavailable through the Signature API.
        /// </summary>
        Completed
    }
}