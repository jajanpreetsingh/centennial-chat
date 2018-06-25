using CentennialTalk.Models.DTOModels;
using CentennialTalk.Persistence;
using CentennialTalk.ServiceContract;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CentennialTalk.Main
{
    public class ChatHub : Hub
    {
        public const string connectionStartEvent = "connectionStarted";
        public const string connectionAbortedEvent = "connectionAborted";
        public const string userJoinedEvent = "userJoined";
        public const string userLeftEvent = "userLeft";
        public const string recieveMessageEvent = "messageReceived";

        public List<string> ChatGroups;

        private readonly IChatService chatService;
        private readonly ChatDBContext dBContext;

        public ChatHub(IChatService chatService,
                        ChatDBContext dBContext)
        {
            this.chatService = chatService;
            this.dBContext = dBContext;

            ChatGroups = new List<string>();
        }

        public override Task OnConnectedAsync()
        {
            base.OnConnectedAsync();

            return Clients.All.SendAsync(connectionStartEvent, Context.ConnectionId);
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            base.OnDisconnectedAsync(exception);

            return Clients.All.SendAsync(connectionAbortedEvent, Context.ConnectionId);
        }

        public void JoinGroupChat(string chatData)
        {
            JoinChatDTO data = JsonConvert.DeserializeObject<JoinChatDTO>(chatData);

            Groups.AddToGroupAsync(Context.ConnectionId, data.chatCode);

            Clients.Group(data.chatCode).SendAsync(userJoinedEvent, data.username);

            if (!ChatGroups.Contains(data.chatCode))
                ChatGroups.Add(data.chatCode);
        }

        public void LeaveGroupChat(string chatData)
        {
            JoinChatDTO data = JsonConvert.DeserializeObject<JoinChatDTO>(chatData);

            Groups.RemoveFromGroupAsync(Context.ConnectionId, data.chatCode);

            Clients.Group(data.chatCode).SendAsync(userLeftEvent, data.username);
        }

        public Task Send(string messageData)
        {
            MessageDTO data = JsonConvert.DeserializeObject<MessageDTO>(messageData);

            return Clients.Group(data.chatCode).SendAsync(recieveMessageEvent, data);
        }
    }
}