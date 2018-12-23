using CentennialTalk.Models;
using CentennialTalk.Models.DTOModels;
using CentennialTalk.ServiceContract;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public const string questionPublishedEvent = "questionPublished";
        public const string questionArchivedEvent = "questionArchived";

        public const string messageReactedEvent = "messageReacted";


        public List<string> ChatGroups;

        private readonly IChatService chatService;
        private readonly IMemberService memberService;
        private readonly IMessageService messageService;
        private readonly IUnitOfWorkService unitOfWorkService;

        public ChatHub(IChatService chatService,
                       IMemberService memberService,
                       IMessageService messageService,
                       IUnitOfWorkService unitOfWorkService)
        {
            this.chatService = chatService;
            this.memberService = memberService;
            this.messageService = messageService;
            this.unitOfWorkService = unitOfWorkService;
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

            memberService.DisconnectAllMembers();

            unitOfWorkService.SaveChanges();

            return Clients.All.SendAsync(connectionAbortedEvent, Context.ConnectionId);
        }

        public void JoinGroupChat(string chatData)
        {
            var data = JsonConvert.DeserializeObject<JoinChatDTO>(chatData);

            if (!ChatGroups.Contains(data.chatCode))
                ChatGroups.Add(data.chatCode);

            Groups.AddToGroupAsync(Context.ConnectionId, data.chatCode);

            MemberDTO mem = memberService.GetChatMembers(data.chatCode).FirstOrDefault().GetDTO();

            Clients.Group(data.chatCode).SendAsync(userJoinedEvent, mem);
        }

        public void LeaveGroupChat(string chatData)
        {
            JoinChatDTO data = JsonConvert.DeserializeObject<JoinChatDTO>(chatData);

            Groups.RemoveFromGroupAsync(Context.ConnectionId, data.chatCode);

            MemberDTO mem = memberService.GetChatMembers(data.chatCode).FirstOrDefault().GetDTO();

            Clients.Group(data.chatCode).SendAsync(userLeftEvent, mem);
        }

        public Task Send(string messageData)
        {
            MessageDTO data = JsonConvert.DeserializeObject<MessageDTO>(messageData);

            return Clients.Group(data.chatCode).SendAsync(recieveMessageEvent, data);
        }

        public Task PublishQuestion(string questionData)
        {
            QuestionDTO ques = JsonConvert.DeserializeObject<QuestionDTO>(questionData);

            ques.isPublished = true;
            ques.publishDate = DateTime.Now.ToString();

            return Clients.Group(ques.chatCode).SendAsync(questionPublishedEvent, ques);
        }

        public Task ArchiveQuestion(string questionData)
        {
            QuestionDTO ques = JsonConvert.DeserializeObject<QuestionDTO>(questionData);

            ques.isArchived = true;
            ques.archiveDate = DateTime.Now.ToString();

            return Clients.Group(ques.chatCode).SendAsync(questionArchivedEvent, ques);
        }

        public Task ReactToMessage(string reactiondata)
        {
            ReactionDTO reac = JsonConvert.DeserializeObject<ReactionDTO>(reactiondata);

            messageService.SaveReaction(reac);

            bool saved = unitOfWorkService.SaveChanges();

            if (saved)
                return Clients.Group(reac.chatCode).SendAsync(messageReactedEvent, reac);

            else return null;
        }
    }
}