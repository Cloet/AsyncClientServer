using System;
using System.Net.Security;
using System.Net.Sockets;
using System.Threading;

namespace SimpleSockets.Messaging.Metadata {

    public interface ISessionMetadata: ISessionInfo, IDisposable {

		new string ClientName { get; set; }

		new Guid Guid { get; set;  }

		new string OsVersion { get; set;  }

		new string UserDomainName { get; set; }

		PacketReceiver DataReceiver { get; }

        SslStream SslStream { get; set;  }

        ManualResetEventSlim ReceivingData { get; set; }

        ManualResetEventSlim Timeout { get; set; }

        ManualResetEventSlim WritingData { get; set; }

        Socket Listener { get; set; }

		void ChangeBufferSize(int size);

		string Info();

		void ChangeDataReceiver(PacketReceiver receiver);

		void ResetDataReceiver();

		ISessionMetadata Clone(int id);

    }

}