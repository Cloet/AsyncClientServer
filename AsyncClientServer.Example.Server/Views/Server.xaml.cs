﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using AsyncClientServer.Example.Server.ViewModel;
using AsyncClientServer.Server;
using AsyncClientServer.StateObject;

namespace AsyncClientServer.Example.Server
{
	/// <summary>
	/// Interaction logic for Server.xaml
	/// </summary>
	public partial class Server : Window
	{

		private IServerListener _listener;
		private ClientInfoViewModel _clientVM;

		public Server()
		{
			InitializeComponent();

			StartServer();
		}

		public void StartServer()
		{
			_listener = new AsyncSocketListener();
			_clientVM = (ClientInfoViewModel) ListViewClients.DataContext;
			_clientVM.Listener = _listener;
			BindEvents();

			new Thread(() =>
			{
				Thread.CurrentThread.IsBackground = true;
				/* run your code here */
				_listener.StartListening("127.0.0.1", 13000);
			}).Start();
		}

		public void BindEvents()
		{
			//Events
			_listener.ProgressFileReceived += new FileTransferProgressHandler(Progress);
			_listener.MessageReceived += new MessageReceivedHandler(MessageReceived);
			_listener.MessageSubmitted += new MessageSubmittedHandler(MessageSubmitted);
			_listener.ClientDisconnected += new ClientDisconnectedHandler(ClientDisconnected);
			_listener.ClientConnected += new ClientConnectedHandler(ClientConnected);
			_listener.FileReceived += new FileFromClientReceivedHandler(FileReceived);
			_listener.ServerHasStarted += new ServerHasStartedHandler(ServerHasStarted);
		}


		//*****Begin Events************///


		private void MessageReceived(int id, string header, string msg)
		{
			Model.Client client = _clientVM.ClientList.First(x => x.Id == id);
			client.Read(header + ": " + msg);
		}

		private void MessageSubmitted(int id, bool close)
		{
			Model.Client client = _clientVM.ClientList.First(x => x.Id == id);
			client.Read("Message submitted to client.");
		}

		private void FileReceived(int id, string path)
		{
			Model.Client client = _clientVM.ClientList.First(x => x.Id == id);
			client.Read("File/Folder has been received and stored at path " + path +".");
		}

		private void Progress(int id, int bytes, int messageSize)
		{

		}

		private void ServerHasStarted()
		{
		}

		private void ClientConnected(int id, IStateObject clientState)
		{
			Model.Client c = new Model.Client(id, clientState.LocalIPv4,clientState.LocalIPv6,clientState.RemoteIPv4,clientState.RemoteIPv6);
			c.Connected = true;


			Application.Current.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, 
				new Action(() =>
				{
					ClientInfoViewModel clientInfoVM = (ClientInfoViewModel)ListViewClients.DataContext;
					clientInfoVM.ClientList.Add(c);
				}));

		}

		private void ClientDisconnected(int id)
		{
			Model.Client client = _clientVM.ClientList.First(x => x.Id == id);
			client.Connected = false;
			client.Read("Client has disconnected from the server.");
		}


		//******END EVENTS************///

	}
}
