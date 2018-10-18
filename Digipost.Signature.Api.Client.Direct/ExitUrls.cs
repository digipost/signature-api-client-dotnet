using System;

namespace Digipost.Signature.Api.Client.Direct
{
    /// <summary>
    ///     Specifies the urls the user will be redirected to for different outcomes of a signing ceremony. When the user is
    ///     redirected, the urls will have an appended query parameter <see cref="StatusReference.StatusQueryTokenParamName" />
    ///     , which contains a token required to <see cref="DirectClient.GetStatus">query for the status of the job</see>.
    /// </summary>
    public class ExitUrls
    {
        public ExitUrls(Uri completionUrl, Uri rejectionUrl, Uri errorUrl)
        {
            CompletionUrl = completionUrl;
            RejectionUrl = rejectionUrl;
            ErrorUrl = errorUrl;
        }

        /// <summary>
        ///     The user will be redirected to this url after having successfully signed the document.
        /// </summary>
        public Uri CompletionUrl { get; set; }

        /// <summary>
        ///     The user will be redirected to this url if actively rejecting to sign the document.
        /// </summary>
        public Uri RejectionUrl { get; set; }

        /// <summary>
        ///     The user will be redirected to this url if any unexpected error happens during the signing ceremony.
        /// </summary>
        public Uri ErrorUrl { get; set; }

        public override string ToString()
        {
            return $"CompletionUrl: {CompletionUrl}, RejectionUrl: {RejectionUrl}, ErrorUrl: {ErrorUrl}";
        }
    }
}
