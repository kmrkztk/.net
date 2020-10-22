using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Lib.Sign
{
    public class Certificate : IDisposable
    {
        readonly RSACryptoServiceProvider _provider;
        readonly HashAlgorithm _algorithm;
        public static HashAlgorithm SHA1 => new SHA1CryptoServiceProvider();
        public static HashAlgorithm SHA256 => new SHA256CryptoServiceProvider();
        public static HashAlgorithm SHA384 => new SHA384CryptoServiceProvider();
        public static HashAlgorithm SHA512 => new SHA512CryptoServiceProvider();
        public AsymmetricAlgorithm Key => _provider;
        public Certificate(string filename) : this(filename,  SHA256) { }
        public Certificate(string filename, HashAlgorithm algorithm) : this(new X509Certificate2(filename), algorithm) { }
        public Certificate(string filename, string password) : this(filename, password, SHA256) { }
        public Certificate(string filename, string password, HashAlgorithm algorithm) : this(new X509Certificate2(filename, password, X509KeyStorageFlags.Exportable), algorithm) { }
        public Certificate(string filename, SecureString password) : this(filename, password, SHA256) { }
        public Certificate(string filename, SecureString password, HashAlgorithm algorithm) : this(new X509Certificate2(filename, password, X509KeyStorageFlags.Exportable), algorithm) { }
        public Certificate(X509Certificate2 x509) : this(x509, SHA256) { }
        public Certificate(X509Certificate2 x509, HashAlgorithm algorithm) 
        {
            var key = x509.HasPrivateKey ?
            (RSACryptoServiceProvider)x509.PrivateKey :
            (RSACryptoServiceProvider)x509.PublicKey.Key;
            _provider = new RSACryptoServiceProvider(2048);
            _provider.FromXmlString(key.ToXmlString(true));
            _algorithm = algorithm;
        }
        public Certificate(RSACryptoServiceProvider provider) : this(provider, SHA256) { }
        public Certificate(RSACryptoServiceProvider provider, HashAlgorithm algorithm)
        {
            _provider = provider;
            _algorithm = algorithm;
        }
        public void Dispose()
        {
            _provider.Dispose();
            _algorithm.Dispose();
        }
        public byte[] Sign(byte[] value) => _provider.SignData(value, _algorithm);
        public byte[] Sign(byte[] value, int offset, int count) => _provider.SignData(value, offset, count, _algorithm);
        public byte[] Sign(XmlNode xml) => Sign(Encoding.UTF8.GetBytes(xml.OuterXml));
        public byte[] Sign(Stream stream) => _provider.SignData(stream, _algorithm);
        
        public bool Verify(byte[] value, byte[] sign) => _provider.VerifyData(value, _algorithm, sign);
        
    }
}
