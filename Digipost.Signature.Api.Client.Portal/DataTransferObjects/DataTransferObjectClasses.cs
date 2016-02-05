using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

/*
Auto generated DTO classes code
*/

#pragma warning disable
namespace Digipost.Signature.Api.Client.Portal.DataTransferObjects
{
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1038.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://signering.posten.no/schema/v1")]
    [XmlRoot(Namespace = "http://signering.posten.no/schema/v1", IsNullable = false)]
    internal class error
    {

        private string _errorcode;

        private string _errormessage;

        private string _errortype;

        [XmlElement("error-code")]
        public string errorcode
        {
            get
            {
                return this._errorcode;
            }
            set
            {
                this._errorcode = value;
            }
        }

        [XmlElement("error-message")]
        public string errormessage
        {
            get
            {
                return this._errormessage;
            }
            set
            {
                this._errormessage = value;
            }
        }

        [XmlElement("error-type")]
        public string errortype
        {
            get
            {
                return this._errortype;
            }
            set
            {
                this._errortype = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1038.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://signering.posten.no/schema/v1")]
    [XmlRoot("portal-signature-job-request", Namespace = "http://signering.posten.no/schema/v1", IsNullable = false)]
    internal class portalsignaturejobrequest
    {

        private string _reference;

        private List<signer> _signers;

        private sender _sender;

        private document _primarydocument;

        public portalsignaturejobrequest()
        {
            this._primarydocument = new document();
            this._sender = new sender();
            this._signers = new List<signer>();
        }

        public string reference
        {
            get
            {
                return this._reference;
            }
            set
            {
                this._reference = value;
            }
        }

        [XmlArrayItem(IsNullable = false)]
        public List<signer> signers
        {
            get
            {
                return this._signers;
            }
            set
            {
                this._signers = value;
            }
        }

        public sender sender
        {
            get
            {
                return this._sender;
            }
            set
            {
                this._sender = value;
            }
        }

        [XmlElement("primary-document")]
        public document primarydocument
        {
            get
            {
                return this._primarydocument;
            }
            set
            {
                this._primarydocument = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1038.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [DesignerCategory("code")]
    [XmlType(Namespace = "http://signering.posten.no/schema/v1")]
    internal class signer
    {

        private string _personalidentificationnumber;

        [XmlElement("personal-identification-number")]
        public string personalidentificationnumber
        {
            get
            {
                return this._personalidentificationnumber;
            }
            set
            {
                this._personalidentificationnumber = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1038.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [DesignerCategory("code")]
    [XmlType(Namespace = "http://signering.posten.no/schema/v1")]
    internal class sender
    {

        private string _organization;

        private string _senderidentifier;

        public string organization
        {
            get
            {
                return this._organization;
            }
            set
            {
                this._organization = value;
            }
        }

        [XmlElement("sender-identifier")]
        public string senderidentifier
        {
            get
            {
                return this._senderidentifier;
            }
            set
            {
                this._senderidentifier = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1038.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [DesignerCategory("code")]
    [XmlType(Namespace = "http://signering.posten.no/schema/v1")]
    internal class document
    {

        private string _title;

        private string _description;

        private string _href;

        private string _mime;

        public string title
        {
            get
            {
                return this._title;
            }
            set
            {
                this._title = value;
            }
        }

        public string description
        {
            get
            {
                return this._description;
            }
            set
            {
                this._description = value;
            }
        }

        [XmlAttribute()]
        public string href
        {
            get
            {
                return this._href;
            }
            set
            {
                this._href = value;
            }
        }

        [XmlAttribute()]
        public string mime
        {
            get
            {
                return this._mime;
            }
            set
            {
                this._mime = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1038.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://signering.posten.no/schema/v1")]
    [XmlRoot("portal-signature-job-status-change-response", Namespace = "http://signering.posten.no/schema/v1", IsNullable = false)]
    internal class portalsignaturejobstatuschangeresponse
    {

        private long _signaturejobid;

        private portalsignaturejobstatus _status;

        private string _confirmationurl;

        private signatures _signatures;

        public portalsignaturejobstatuschangeresponse()
        {
            this._signatures = new signatures();
        }

        [XmlElement("signature-job-id")]
        public long signaturejobid
        {
            get
            {
                return this._signaturejobid;
            }
            set
            {
                this._signaturejobid = value;
            }
        }

        public portalsignaturejobstatus status
        {
            get
            {
                return this._status;
            }
            set
            {
                this._status = value;
            }
        }

        [XmlElement("confirmation-url")]
        public string confirmationurl
        {
            get
            {
                return this._confirmationurl;
            }
            set
            {
                this._confirmationurl = value;
            }
        }

        public signatures signatures
        {
            get
            {
                return this._signatures;
            }
            set
            {
                this._signatures = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1038.0")]
    [Serializable()]
    [XmlType(TypeName = "portal-signature-job-status", Namespace = "http://signering.posten.no/schema/v1")]
    public enum portalsignaturejobstatus
    {

        /// <remarks/>
        PARTIALLY_COMPLETED,

        /// <remarks/>
        COMPLETED,
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1038.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [DesignerCategory("code")]
    [XmlType(Namespace = "http://signering.posten.no/schema/v1")]
    internal class signatures
    {

        private List<signature> _signature;

        private string _padesurl;

        public signatures()
        {
            this._signature = new List<signature>();
        }

        [XmlElement("signature")]
        public List<signature> signature
        {
            get
            {
                return this._signature;
            }
            set
            {
                this._signature = value;
            }
        }

        [XmlElement("pades-url")]
        public string padesurl
        {
            get
            {
                return this._padesurl;
            }
            set
            {
                this._padesurl = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1038.0")]
    [Serializable()]
    [DebuggerStepThrough()]
    [DesignerCategory("code")]
    [XmlType(Namespace = "http://signering.posten.no/schema/v1")]
    internal class signature
    {

        private signaturestatus _status;

        private string _personalidentificationnumber;

        private string _xadesurl;

        public signaturestatus status
        {
            get
            {
                return this._status;
            }
            set
            {
                this._status = value;
            }
        }

        [XmlElement("personal-identification-number")]
        public string personalidentificationnumber
        {
            get
            {
                return this._personalidentificationnumber;
            }
            set
            {
                this._personalidentificationnumber = value;
            }
        }

        [XmlElement("xades-url")]
        public string xadesurl
        {
            get
            {
                return this._xadesurl;
            }
            set
            {
                this._xadesurl = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1038.0")]
    [Serializable()]
    [XmlType(TypeName = "signature-status", Namespace = "http://signering.posten.no/schema/v1")]
    public enum signaturestatus
    {

        /// <remarks/>
        WAITING,

        /// <remarks/>
        SIGNED,
    }
}
#pragma warning restore
