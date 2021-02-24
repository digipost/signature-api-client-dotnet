﻿using System;

namespace Digipost.Signature.Api.Client.Core.Internal.Extensions
{
    internal static class FileTypeExtensions
    {
        public static string ToMimeType(this FileType fileType)
        {
            string mimeType;

            switch (fileType)
            {
                case FileType.Pdf:
                    mimeType = "application/pdf";
                    break;
                case FileType.Txt:
                    mimeType = "text/plain";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(fileType), fileType, null);
            }

            return mimeType;
        }
        
        public static string GetExtension(this FileType fileType)
        {
            string extension;

            switch (fileType)
            {
                case FileType.Pdf:
                    extension = ".pdf";
                    break;
                case FileType.Txt:
                    extension = ".txt";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(fileType), fileType, null);
            }

            return extension;
        }
    }
}
