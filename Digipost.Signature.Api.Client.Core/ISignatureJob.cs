using Digipost.Signature.Api.Client.Core.Enums;

namespace Digipost.Signature.Api.Client.Core
{
    public interface ISignatureJob
    {
        Sender Sender { get; }

        AbstractDocument Document { get; }

        /// <summary>
        ///     Specify the minimum level of authentication for the signer(s) of this job. This
        ///     includes the required authentication both in order to <em>view</em> the document, as well
        ///     as it will limit which <em>authentication mechanisms offered at the time of signing</em>
        ///     of the document.
        /// </summary>
        AuthenticationLevel? AuthenticationLevel { get; set; }

        /// <summary>
        ///     Specify how the signer(s) of this job should be identified in the signed documents (XAdES and PAdES);
        ///     by
        ///     <see cref="Enums.IdentifierInSignedDocuments.PersonalIdentificationNumberAndName">personal identification number</see>
        ///     ,
        ///     by <see cref="Enums.IdentifierInSignedDocuments.DateOfBirthAndName">date of birth and name</see> or
        ///     by <see cref="Enums.IdentifierInSignedDocuments.Name">name only</see>.
        ///     Not all options are available to every sender, and this is detailed in the service's
        ///     <a href="http://digipost.github.io/signature-api-specification">functional documentation</a>.
        /// </summary>
        IdentifierInSignedDocuments? IdentifierInSignedDocuments { get; set; }

        /// <summary>
        ///     A custom reference that is attached to the job.
        /// </summary>
        string Reference { get; }
    }
}