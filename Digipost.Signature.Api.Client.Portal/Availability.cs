using System;

namespace Digipost.Signature.Api.Client.Portal
{
    public class Availability
    {
        public DateTime? Activation { get; set; }

        public long? AvailableSeconds { get; private set; }

        public TimeSpan AvailableFor
        {
            set { AvailableSeconds = (long) value.TotalSeconds; }
        }

        public override string ToString()
        {
            return $"Activation: {Activation}, AvailableSeconds: {AvailableSeconds}";
        }
    }
}