﻿using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using AsyncClientServer.Helper;
using AsyncClientServer.Model.ClientState;

namespace AsyncClientServer.Model
{

	/// <summary>
	/// This class is used to keep track of certain values for server/client
	/// <para>This is needed because the client and server work async</para>
	/// <para>Implements
	///<seealso cref="IStateObject"/>
	/// </para>
	/// </summary>
	public class StateObject: IStateObject
	{

		/* Contains the state information. */

		private const int Buffer_Size = 1024;
		private StringBuilder _sb;

		/// <summary>
		/// Constructor for StateObject
		/// </summary>
		/// <param name="listener"></param>
		/// <param name="id"></param>
		public StateObject(Socket listener, int id = -1)
		{
			Listener = listener;
			Id = id;
			Close = false;
			Reset();
		}

		public int Read { get; private set; }
		public int Flag { get; set; }
		public string Header { get; set; }

		/// <summary>
		/// Get the id
		/// </summary>
		public int Id { get; }

		/// <summary>
		/// Return of set close boolean
		/// <para>This parameter is used to check if the socket has to be closed.</para>
		/// </summary>
		public bool Close { get; set; }

		/// <summary>
		/// Gets the buffersize
		/// </summary>
		public int BufferSize => Buffer_Size;

		public int MessageSize { get; set; }
		public int HeaderSize { get; set; }

		/// <summary>
		/// Gets the amount of bytes in the buffer
		/// </summary>
		public byte[] Buffer { get; set; } = new byte[Buffer_Size];

		/// <summary>
		/// Returns the listener socket
		/// </summary>
		public Socket Listener { get; }

		/// <summary>
		/// Returns the text from stringbuilder
		/// </summary>
		public string Text => this._sb.ToString();

		/// <summary>
		/// Add text to stringbuilder
		/// </summary>
		/// <param name="text"></param>
		public void Append(string text)
		{
			_sb.Append(text);
		}

		/// <summary>
		/// Appends how much bytes have been read
		/// </summary>
		/// <param name="length"></param>
		public void AppendRead(int length)
		{
			Read += length;
		}

		/// <summary>
		/// Removes some bytes that have been read
		/// </summary>
		/// <param name="length"></param>
		public void SubtractRead(int length)
		{
			Read -= length;
		}

		public void ChangeBuffer(byte[] test)
		{
			Buffer = test;
		}

		public StateObjectState CurrentState { get; set; }

		public byte[] PreviousRead { get; set; } = null;

		/// <summary>
		/// Resets the stringbuilder and other properties
		/// </summary>
		public void Reset()
		{
			Header = "";
			MessageSize = 0;
			HeaderSize = 0;
			Read = 0;
			Flag = 0;
			_sb = new StringBuilder();
		}

	}
}
