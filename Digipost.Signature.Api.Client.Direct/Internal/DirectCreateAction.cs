using System;
using Digipost.Signature.Api.Client.Core.Internal;
using Digipost.Signature.Api.Client.Core.Internal.Asice;
using Digipost.Signature.Api.Client.Direct.DataTransferObjects;
using Digipost.Signature.Api.Client.Scripts.XsdToCode.Code;

namespace Digipost.Signature.Api.Client.Direct.Internal
{
    internal class DirectCreateAction : CreateAction
    {
        public static readonly Func<IRequestContent, string> SerializeFunc = content => SerializeUtility.Serialize(DataTransferObjectConverter.ToDataTransferObject((Job) content));
        public static readonly Func<string, JobResponse> DeserializeFunc = content => DataTransferObjectConverter.FromDataTransferObject(SerializeUtility.Deserialize<directsignaturejobresponse>(content));

        public DirectCreateAction(Job job, DocumentBundle documentBundle)
            : base(job, documentBundle, SerializeFunc)
        {
        }
    }
}