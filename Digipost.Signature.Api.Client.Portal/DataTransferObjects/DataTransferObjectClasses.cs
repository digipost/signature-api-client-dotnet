namespace Digipost.Signature.Api.Client.Core
{
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1038.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://signering.posten.no/schema/v1", TypeName = "error")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://signering.posten.no/schema/v1", IsNullable = false, ElementName = "error")]
    public class DTOError
    {

        [System.Xml.Serialization.XmlElementAttribute("error-code", ElementName = "errorcode")]
        public string Errorcode { get; set; }
        [System.Xml.Serialization.XmlElementAttribute("error-message", ElementName = "errormessage")]
        public string Errormessage { get; set; }
        [System.Xml.Serialization.XmlElementAttribute("error-type", ElementName = "errortype")]
        public string Errortype { get; set; }
    }
}
namespace Digipost.Signature.Api.Client.Core
{
    using System.Collections.Generic;

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1038.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://signering.posten.no/schema/v1", TypeName = "portalsignaturejobrequest")]
    [System.Xml.Serialization.XmlRootAttribute("portal-signature-job-request", Namespace = "http://signering.posten.no/schema/v1", IsNullable = false, ElementName = "portalsignaturejobrequest")]
    public class DTOPortalsignaturejobrequest
    {

        [System.Xml.Serialization.XmlElementAttribute("reference")]
        public string Reference { get; set; }
        [System.Xml.Serialization.XmlArrayItemAttribute(IsNullable = false, ElementName = "signers")]
        public List<DTOSigner> Signers { get; set; }
        [System.Xml.Serialization.XmlElementAttribute("sender")]
        public DTOSender Sender { get; set; }
        [System.Xml.Serialization.XmlElementAttribute("distribution-time", ElementName = "distributiontime")]
        public System.DateTime Distributiontime { get; set; }

        public DTOPortalsignaturejobrequest()
        {
            this.Sender = new DTOSender();
            this.Signers = new List<DTOSigner>();
        }
    }
}
namespace Digipost.Signature.Api.Client.Core
{
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1038.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://signering.posten.no/schema/v1", TypeName = "signer")]
    [System.Xml.Serialization.XmlRootAttribute("signer")]
    public class DTOSigner
    {

        [System.Xml.Serialization.XmlElementAttribute("personal-identification-number", ElementName = "personalidentificationnumber")]
        public string Personalidentificationnumber { get; set; }
    }
}
namespace Digipost.Signature.Api.Client.Core
{
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1038.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://signering.posten.no/schema/v1", TypeName = "sender")]
    [System.Xml.Serialization.XmlRootAttribute("sender")]
    public class DTOSender
    {

        [System.Xml.Serialization.XmlElementAttribute("organization")]
        public string Organization { get; set; }
        [System.Xml.Serialization.XmlElementAttribute("sender-identifier", ElementName = "senderidentifier")]
        public string Senderidentifier { get; set; }
    }
}
namespace Digipost.Signature.Api.Client.Core
{
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1038.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://signering.posten.no/schema/v1", TypeName = "portalsignaturejobresponse")]
    [System.Xml.Serialization.XmlRootAttribute("portal-signature-job-response", Namespace = "http://signering.posten.no/schema/v1", IsNullable = false, ElementName = "portalsignaturejobresponse")]
    public class DTOPortalsignaturejobresponse
    {

        [System.Xml.Serialization.XmlElementAttribute("signature-job-id", ElementName = "signaturejobid")]
        public long Signaturejobid { get; set; }
    }
}
namespace Digipost.Signature.Api.Client.Core
{
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1038.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://signering.posten.no/schema/v1", TypeName = "portalsignaturejobstatuschangeresponse")]
    [System.Xml.Serialization.XmlRootAttribute("portal-signature-job-status-change-response", Namespace = "http://signering.posten.no/schema/v1", IsNullable = false, ElementName = "portalsignaturejobstatuschangeresponse")]
    public class DTOPortalsignaturejobstatuschangeresponse
    {

        [System.Xml.Serialization.XmlElementAttribute("signature-job-id", ElementName = "signaturejobid")]
        public long Signaturejobid { get; set; }
        [System.Xml.Serialization.XmlElementAttribute("status")]
        public Portalsignaturejobstatus Status { get; set; }
        [System.Xml.Serialization.XmlElementAttribute("confirmation-url", ElementName = "confirmationurl")]
        public string Confirmationurl { get; set; }
        [System.Xml.Serialization.XmlElementAttribute("signatures")]
        public DTOSignatures Signatures { get; set; }

        public DTOPortalsignaturejobstatuschangeresponse()
        {
            this.Signatures = new DTOSignatures();
        }
    }
}
namespace Digipost.Signature.Api.Client.Core
{
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1038.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "portalsignaturejobstatus", Namespace = "http://signering.posten.no/schema/v1")]
    [System.Xml.Serialization.XmlRootAttribute("portalsignaturejobstatus")]
    public enum Portalsignaturejobstatus
    {

        /// <remarks/>
        PARTIALLY_COMPLETED,

        /// <remarks/>
        COMPLETED,
    }
}
namespace Digipost.Signature.Api.Client.Core
{
    using System.Collections.Generic;

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1038.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://signering.posten.no/schema/v1", TypeName = "signatures")]
    [System.Xml.Serialization.XmlRootAttribute("signatures")]
    public class DTOSignatures
    {

        [System.Xml.Serialization.XmlElementAttribute("signature", ElementName = "signature")]
        public List<DTOSignature> DTOSignature { get; set; }
        [System.Xml.Serialization.XmlElementAttribute("pades-url", ElementName = "padesurl")]
        public string Padesurl { get; set; }

        public DTOSignatures()
        {
            this.DTOSignature = new List<DTOSignature>();
        }
    }
}
namespace Digipost.Signature.Api.Client.Core
{
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1038.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://signering.posten.no/schema/v1", TypeName = "signature")]
    [System.Xml.Serialization.XmlRootAttribute("signature")]
    public class DTOSignature
    {

        [System.Xml.Serialization.XmlElementAttribute("status")]
        public Signaturestatus Status { get; set; }
        [System.Xml.Serialization.XmlElementAttribute("personal-identification-number", ElementName = "personalidentificationnumber")]
        public string Personalidentificationnumber { get; set; }
        [System.Xml.Serialization.XmlElementAttribute("xades-url", ElementName = "xadesurl")]
        public string Xadesurl { get; set; }
    }
}
namespace Digipost.Signature.Api.Client.Core
{
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1038.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "signaturestatus", Namespace = "http://signering.posten.no/schema/v1")]
    [System.Xml.Serialization.XmlRootAttribute("signaturestatus")]
    public enum Signaturestatus
    {

        /// <remarks/>
        WAITING,

        /// <remarks/>
        REJECTED,

        /// <remarks/>
        SIGNED,
    }
}
