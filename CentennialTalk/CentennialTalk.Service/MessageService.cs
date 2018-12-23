using CentennialTalk.Models;
using CentennialTalk.Models.DTOModels;
using CentennialTalk.PersistenceContract;
using CentennialTalk.ServiceContract;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace CentennialTalk.Service
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository messageRepository;
        private readonly ILogger<MessageService> logger;

        public MessageService(IMessageRepository messageRepository, ILogger<MessageService> logger)
        {
            this.messageRepository = messageRepository;
            this.logger = logger;
        }

        public List<Message> GetChatMessages(string chatCode)
        {
            try
            {
                return messageRepository.GetChatMessages(chatCode);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);

                return null;
            }
        }

        public Message GetMessageById(Guid guid)
        {
            return messageRepository.FindById(guid);
        }

        public bool SaveMessage(MessageDTO messageData)
        {
            try
            {
                Message message = new Message(messageData);

                if (messageData.replyId != messageData.messageId
                    && messageData.replyId != Guid.Empty)
                    message.SetReplyId(messageData.replyId);

                return messageRepository.SaveMessage(message);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);

                return false;
            }
        }

        public bool SaveReaction(MessageDTO messageData)
        {
            try
            {
                Message message = new Message(messageData);

                if (messageData.replyId != messageData.messageId
                    && messageData.replyId != Guid.Empty)
                    message.SetReplyId(messageData.replyId);

                return messageRepository.SaveMessage(message);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);

                return false;
            }
        }

        public Message SaveReaction(ReactionDTO reactDto)
        {
            Message mem = messageRepository.FindById(Guid.Parse(reactDto.messageId));

            if (mem == null)
                return null;

            if (reactDto.reaction != 0)
                mem.Reactions.Add(new MessageReaction(reactDto.member, reactDto.reaction == 1 ? ReactType.LIKE : ReactType.DISLIKE));

            return mem;
        }
    }
}