namespace Digipost.Signature.Api.Client.Direct.Enums
{
    public enum JobStatus
    {
        /// <summary>
        ///     All signers have performed an action to the document, but at least one have a non successful status (e.g. rejected,
        ///     expired or failed).
        /// </summary>
        Failed,

        /// <summary>
        ///     At least one signer has not yet performed any action to the document.
        ///     For details about the state, see the <see cref="SignatureStatus">status</see> of each signer.
        /// </summary>
        InProgress,

        /// <summary>
        ///     All signers have successfully signed the document.
        /// </summary>
        CompletedSuccessfully,

        /// <summary>
        ///     There has not been any changes since the last received status change.
        /// </summary>
        NoChanges
    }
}
